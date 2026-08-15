using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;

namespace TemperatureMonitor
{
    /// <summary>
    /// 主窗体。负责温度数据的定时采集、界面更新和报警指示。
    /// </summary>
    public partial class MainForm : Form
    {
        private static readonly NLog.Logger Logger = LogManager.GetCurrentClassLogger();

        // ---------- 常量 ----------
        private const int SaveIntervalTicks = 5;        // 每 N 次采集批量存一次温度日志
        private const int MaxDataPoints = 300;          // 图表最多保留点数（5 分钟）

        // ---------- 字段 ----------
        private readonly TemperatureLogRepository _temperatureRepo;
        private readonly AlarmLogRepository _alarmRepo;
        private readonly DatabaseInitializer _dbInit;
        private readonly ModbusClient _modbusClient;
        private readonly ConfigManager _configManager;

        private bool _isConnected;          // 当前连接状态
        private bool _isReading;            // 是否正在读取中（防重入）

        // 报警相关字段
        private readonly bool[] _channelAlarmStates = new bool[4];  // 当前各通道报警状态
        private readonly bool[] _prevAlarmStates   = new bool[4];   // 上次各通道报警状态（用于检测跳变）
        private bool _blinkOn;                                       // 闪烁状态切换标志
        private int _saveTickCounter;                                // 采集计数，用于 5 秒批量存库

        // 标题栏拖拽相关（无边框窗体）
        private bool _titleDragging;
        private Point _dragOffset;

        /// <summary>
        /// 构造函数：初始化组件、加载配置、初始化数据库和 Modbus 客户端。
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            // 设计模式下不执行数据库和硬件初始化，避免设计器崩溃
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;

            // 浅色菜单渲染（白色主题）
            menuStrip.Renderer = new LightMenuRenderer();

            // ---- 加载系统配置 ----
            _configManager = new ConfigManager();
            _configManager.Load();
            ApplyConfigToUI();

            // ---- 初始化 SQLite 数据库 ----
            string dbPath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "temperature_monitor.db");
            string connectionString = $"Data Source={dbPath};Version=3;";

            _dbInit = new DatabaseInitializer(connectionString);
            _temperatureRepo = new TemperatureLogRepository(connectionString);
            _alarmRepo = new AlarmLogRepository(connectionString);
            _dbInit.Initialize();

            // ---- 创建 Modbus 客户端并从配置中读取 IP 和端口 ----
            var cfg = _configManager.Config;
            _modbusClient = new ModbusClient
            {
                IpAddress = cfg.PlcIpAddress,
                Port = cfg.PlcPort
            };

            // ---- 设置定时器间隔 ----
            timerDataCollection.Interval = cfg.SamplingIntervalMs;
        }

        /// <summary>将配置值应用到界面上的状态栏提示等</summary>
        private void ApplyConfigToUI()
        {
            var cfg = _configManager.Config;
            tsslConnectionStatus.Text = $"未连接（{cfg.PlcIpAddress}:{cfg.PlcPort}）";
        }

        // ===================================================================
        //  设置窗体
        // ===================================================================

        /// <summary>"设置"按钮：打开配置窗体，用户保存后重新应用配置</summary>
        private void btnConfig_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmConfig(_configManager))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    // 用户点击了保存 → 重新应用配置
                    ApplyConfigToUI();
                    var cfg = _configManager.Config;

                    // 更新 Modbus 客户端连接参数
                    _modbusClient.IpAddress = cfg.PlcIpAddress;
                    _modbusClient.Port = cfg.PlcPort;

                    // 更新定时器间隔
                    timerDataCollection.Interval = cfg.SamplingIntervalMs;

                    Logger.Info("配置已更新，下次采集将使用新参数。");
                }
            }
        }

        /// <summary>"查询历史"按钮：打开历史数据查询窗体</summary>
        private void btnQueryHistory_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmQuery(_temperatureRepo, _alarmRepo))
            {
                frm.ShowDialog(this);
            }
        }

        /// <summary>菜单项"SECS/GEM 通讯测试"：打开通讯测试窗体</summary>
        private void tsmiSecsTest_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmSecsTest())
            {
                frm.ShowDialog(this);
            }
        }

        // ===================================================================
        //  连接 / 断开
        // ===================================================================

        /// <summary>连接按钮单击事件：切换连接状态</summary>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (_isConnected)
                DisconnectDevice();
            else
                ConnectDevice();
        }

        /// <summary>连接到 Modbus 服务器，成功后启动定时采集</summary>
        private void ConnectDevice()
        {
            try
            {
                _modbusClient.Connect();
                _isConnected = true;
                btnConnect.Text = "断开";
                tsslConnectionStatus.Text = "已连接";
                tsslConnectionStatus.ForeColor = Color.Green;
                timerDataCollection.Start();
                Logger.Info("Modbus 连接成功，开始采集。");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"连接失败：{ex.Message}", "连接错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Error(ex, "Modbus 连接失败。");
            }
        }

        /// <summary>断开 Modbus 连接并停止采集</summary>
        private void DisconnectDevice()
        {
            timerDataCollection.Stop();
            _modbusClient.Disconnect();
            _isConnected = false;
            btnConnect.Text = "连接";
            var cfg = _configManager.Config;
            tsslConnectionStatus.Text = $"已断开（{cfg.PlcIpAddress}:{cfg.PlcPort}）";
            tsslConnectionStatus.ForeColor = Color.Gray;
            Logger.Info("Modbus 连接已断开，采集停止。");
        }

        // ===================================================================
        //  定时采集（Timer 驱动）
        // ===================================================================

        /// <summary>定时器 Tick：在后台线程读取 Modbus 数据，然后回 UI 线程更新界面</summary>
        private async void timerDataCollection_Tick(object sender, EventArgs e)
        {
            // 防重入：上次读取还没完成则跳过本次
            if (_isReading) return;
            _isReading = true;

            try
            {
                // 在后台线程执行阻塞的 Modbus 读取操作（不卡界面）
                float[] temps = await Task.Run(() => _modbusClient.ReadRegisters());

                // 读取成功 → 回到 UI 线程更新界面
                UpdateTemperatureDisplay(temps);
                UpdateAlarmIndicator(temps);
                UpdateChart(temps);
                tsslConnectionStatus.Text = "已连接";
                tsslConnectionStatus.ForeColor = Color.Green;

                // 每 5 秒批量存一次温度日志
                _saveTickCounter++;
                if (_saveTickCounter >= SaveIntervalTicks)
                {
                    _saveTickCounter = 0;
                    float[] snap = (float[])temps.Clone();
                    _ = Task.Run(() => BatchSaveTemperatureLogs(snap));
                }
            }
            catch (Exception ex)
            {
                // 读取失败 → 更新状态栏
                tsslConnectionStatus.Text = "通信中断";
                tsslConnectionStatus.ForeColor = Color.Red;
                Logger.Warn(ex, "定时采集数据失败。");
            }
            finally
            {
                _isReading = false;
            }
        }

        // ===================================================================
        //  UI 更新方法（均在 UI 线程调用）
        // ===================================================================

        /// <summary>更新 4 个通道的温度 Label</summary>
        private void UpdateTemperatureDisplay(float[] temps)
        {
            lblChannel1.Text = $"通道 1：{temps[0]:F2} ℃";
            lblChannel2.Text = $"通道 2：{temps[1]:F2} ℃";
            lblChannel3.Text = $"通道 3：{temps[2]:F2} ℃";
            lblChannel4.Text = $"通道 4：{temps[3]:F2} ℃";
        }

        /// <summary>
        /// 检查每路温度是否超上限或低于下限，更新对应通道报警指示灯。
        /// 检测到新报警时播放系统提示音并写入报警记录。
        /// 有报警时启动闪烁 Timer，全部恢复时停止。
        /// </summary>
        private void UpdateAlarmIndicator(float[] temps)
        {
            var cfg = _configManager.Config;
            bool anyAlarm = false;
            Panel[] alarmPanels = { pnlAlarm1, pnlAlarm2, pnlAlarm3, pnlAlarm4 };

            for (int i = 0; i < 4; i++)
            {
                bool overHigh = temps[i] > cfg.AlarmUpperLimit;
                bool belowLow  = temps[i] < cfg.AlarmLowerLimit;
                _channelAlarmStates[i] = overHigh || belowLow;

                // 检测到新的报警（从正常→报警的跳变）
                if (_channelAlarmStates[i] && !_prevAlarmStates[i])
                {
                    PlayAlarmSound();

                    string alarmType = overHigh ? "超温报警" : "低温报警";
                    _alarmRepo.Insert(new Models.AlarmLog
                    {
                        ChannelNo = i + 1,
                        AlarmType = alarmType,
                        Value = temps[i],
                        Timestamp = DateTime.Now
                    });
                }

                // 非报警状态的 Panel 保持绿色
                if (!_channelAlarmStates[i])
                    alarmPanels[i].BackColor = Color.LimeGreen;

                anyAlarm = anyAlarm || _channelAlarmStates[i];
            }

            // 复制当前状态到上一次状态（供下次检测跳变）
            Array.Copy(_channelAlarmStates, _prevAlarmStates, 4);

            // --- 整体状态指示（顶部 pnlAlarm + lblAlarmStatus） ---
            if (anyAlarm)
            {
                pnlAlarm.BackColor = Color.Red;
                lblAlarmStatus.Text = "!! 有报警 !!";
                lblAlarmStatus.ForeColor = Color.Red;

                // 启动闪烁（如尚未启动）
                if (!timerAlarmBlink.Enabled)
                    timerAlarmBlink.Start();
            }
            else
            {
                pnlAlarm.BackColor = Color.LimeGreen;
                lblAlarmStatus.Text = "系统正常";
                lblAlarmStatus.ForeColor = Color.Green;

                // 全部恢复 → 停止闪烁，确保所有 Panel 为绿色
                timerAlarmBlink.Stop();
                _blinkOn = false;
                foreach (var p in alarmPanels)
                    p.BackColor = Color.LimeGreen;
            }
        }

        /// <summary>报警闪烁 Timer Tick：在报警通道上切换红色/暗红色实现闪烁效果</summary>
        private void timerAlarmBlink_Tick(object sender, EventArgs e)
        {
            _blinkOn = !_blinkOn;
            Panel[] alarmPanels = { pnlAlarm1, pnlAlarm2, pnlAlarm3, pnlAlarm4 };

            for (int i = 0; i < 4; i++)
            {
                if (_channelAlarmStates[i])
                    alarmPanels[i].BackColor = _blinkOn ? Color.Red : Color.DarkRed;
            }
        }

        /// <summary>播放系统提示音</summary>
        private void PlayAlarmSound()
        {
            System.Media.SystemSounds.Beep.Play();
        }

        /// <summary>向图表的 4 条曲线追加新数据点，超出上限则移除旧点</summary>
        private void UpdateChart(float[] temps)
        {
            DateTime now = DateTime.Now;

            for (int i = 0; i < 4; i++)
            {
                chartTemperature.Series[i].Points.AddXY(now, temps[i]);
                while (chartTemperature.Series[i].Points.Count > MaxDataPoints)
                    chartTemperature.Series[i].Points.RemoveAt(0);
            }

            chartTemperature.ChartAreas[0].AxisX.Minimum = now.AddSeconds(-MaxDataPoints).ToOADate();
            chartTemperature.ChartAreas[0].AxisX.Maximum = now.ToOADate();
        }

        // ===================================================================
        //  数据库写入（后台线程调用）
        // ===================================================================

        /// <summary>批量将 4 路温度记录保存到 TemperatureLog 表（事务保护）</summary>
        private void BatchSaveTemperatureLogs(float[] temps)
        {
            try
            {
                DateTime now = DateTime.Now;
                var logs = new Models.TemperatureLog[4];

                for (int i = 0; i < 4; i++)
                {
                    logs[i] = new Models.TemperatureLog
                    {
                        ChannelNo = i + 1,
                        Value = temps[i],
                        Timestamp = now
                    };
                }

                _temperatureRepo.BatchInsert(logs);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "批量保存温度数据失败。");
            }
        }

        // ===================================================================
        //  窗体关闭
        // ===================================================================

        /// <summary>窗体关闭时清理资源：停止定时器并断开 Modbus</summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerDataCollection.Stop();
            timerAlarmBlink.Stop();
            _modbusClient?.Disconnect();
            Logger.Info("应用程序关闭。");
        }

        // ===================================================================
        //  标题栏交互（无边框窗体自定义标题栏）
        // ===================================================================

        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _titleDragging = true;
                _dragOffset = e.Location;
            }
        }

        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (_titleDragging)
            {
                Point screen = PointToScreen(e.Location);
                Location = new Point(screen.X - _dragOffset.X, screen.Y - _dragOffset.Y);
            }
        }

        private void TitleBar_MouseUp(object sender, MouseEventArgs e)
        {
            _titleDragging = false;
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // ===================================================================
        //  圆形指示灯绘制（报警灯）
        // ===================================================================

        /// <summary>将报警灯 Panel 绘制为圆形发光指示灯，颜色取自 BackColor，BackColor 变化自动触发重绘</summary>
        private void AlarmLightPaint(object sender, PaintEventArgs e)
        {
            var p = sender as Panel;
            if (p == null) return;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Color c = p.BackColor;
            Rectangle rect = p.ClientRectangle;

            // 外圈光晕
            using (var halo = new SolidBrush(Color.FromArgb(60, c)))
                g.FillEllipse(halo, rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);

            // 主圆
            Rectangle core = new Rectangle(rect.X + 5, rect.Y + 5, rect.Width - 10, rect.Height - 10);
            using (var brush = new SolidBrush(c))
                g.FillEllipse(brush, core);

            // 顶部高光
            using (var hi = new SolidBrush(Color.FromArgb(110, 255, 255, 255)))
                g.FillEllipse(hi, core.X + core.Width / 4, core.Y + core.Height / 5, core.Width / 2, core.Height / 3);

            // 底部暗部
            using (var lo = new SolidBrush(Color.FromArgb(50, 0, 0, 0)))
                g.FillEllipse(lo, core.X + core.Width / 6, core.Y + core.Height * 3 / 5, core.Width * 2 / 3, core.Height * 2 / 5);
        }

        // ===================================================================
        //  通道卡片圆角背景绘制
        // ===================================================================

        private void CardPaint(object sender, PaintEventArgs e)
        {
            var p = sender as Panel;
            if (p == null) return;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var b = new SolidBrush(p.BackColor))
                g.FillPath(b, RoundedRect(p.ClientRectangle, 10));
        }

        private static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        // ===================================================================
        //  深色菜单渲染器
        // ===================================================================

        private sealed class LightMenuRenderer : ToolStripProfessionalRenderer
        {
            public LightMenuRenderer() : base(new LightColorTable()) { }
        }

        private sealed class LightColorTable : ProfessionalColorTable
        {
            public override Color MenuItemSelected => Color.FromArgb(235, 238, 242);
            public override Color MenuItemBorder => Color.FromArgb(235, 238, 242);
            public override Color MenuItemSelectedGradientBegin => Color.FromArgb(235, 238, 242);
            public override Color MenuItemSelectedGradientEnd => Color.FromArgb(235, 238, 242);
            public override Color ToolStripDropDownBackground => Color.White;
            public override Color ImageMarginGradientBegin => Color.White;
            public override Color ImageMarginGradientMiddle => Color.White;
            public override Color ImageMarginGradientEnd => Color.White;
            public override Color MenuBorder => Color.FromArgb(208, 213, 221);
        }

        // ===================================================================
        //  系统按钮自绘图标（最小化 / 最大化 / 关闭）
        // ===================================================================

        private bool _sysBtnHover;

        private void SysBtn_MouseEnter(object sender, EventArgs e)
        {
            _sysBtnHover = true;
            ((Control)sender).Invalidate();
        }

        private void SysBtn_MouseLeave(object sender, EventArgs e)
        {
            _sysBtnHover = false;
            ((Control)sender).Invalidate();
        }

        private void SysBtnPaint(object sender, PaintEventArgs e)
        {
            var b = sender as Button;
            if (b == null) return;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            bool closeHover = b.Name == "btnClose" && _sysBtnHover;
            using (var pen = new Pen(closeHover ? Color.White : Color.FromArgb(90, 90, 90), 1.6f))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                int cx = b.ClientSize.Width / 2;
                int cy = b.ClientSize.Height / 2;
                if (b.Name == "btnMinimize")
                {
                    g.DrawLine(pen, cx - 7, cy, cx + 7, cy);
                }
                else if (b.Name == "btnMaximize")
                {
                    if (WindowState == FormWindowState.Maximized)
                    {
                        // 还原样式：两个小方框
                        g.DrawRectangle(pen, cx - 6, cy - 5, 9, 9);
                        g.DrawRectangle(pen, cx - 3, cy - 2, 9, 9);
                    }
                    else
                    {
                        g.DrawRectangle(pen, cx - 7, cy - 6, 14, 12);
                    }
                }
                else if (b.Name == "btnClose")
                {
                    g.DrawLine(pen, cx - 6, cy - 6, cx + 6, cy + 6);
                    g.DrawLine(pen, cx + 6, cy - 6, cx - 6, cy + 6);
                }
            }
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                WindowState = FormWindowState.Normal;
            else
                WindowState = FormWindowState.Maximized;
            btnMaximize.Invalidate();
        }

        private void TitleBar_DoubleClick(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                WindowState = FormWindowState.Normal;
            else
                WindowState = FormWindowState.Maximized;
            btnMaximize.Invalidate();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            btnMaximize?.Invalidate();
        }

        // ===================================================================
        //  最大化时限制窗口不超过工作区（不遮任务栏）
        // ===================================================================

        private const int WM_GETMINMAXINFO = 0x0024;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_GETMINMAXINFO)
            {
                var mmi = (MINMAXINFO)System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, typeof(MINMAXINFO));
                Rectangle wa = Screen.FromHandle(Handle).WorkingArea;
                mmi.ptMaxPosition.X = wa.X;
                mmi.ptMaxPosition.Y = wa.Y;
                mmi.ptMaxSize.X = wa.Width;
                mmi.ptMaxSize.Y = wa.Height;
                System.Runtime.InteropServices.Marshal.StructureToPtr(mmi, m.LParam, true);
            }
            base.WndProc(ref m);
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct MINMAXINFO
        {
            public Point ptReserved;
            public Point ptMaxSize;
            public Point ptMaxPosition;
            public Point ptMinTrackSize;
            public Point ptMaxTrackSize;
        }
    }
}
