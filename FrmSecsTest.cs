using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Options;
using NLog;
using Secs4Net;

namespace TemperatureMonitor
{
    /// <summary>
    /// SECS/GEM 通讯测试窗体。
    /// 基于 Secs4Net 2.x（HsmsConnection + SecsGem），支持：
    ///  - 连接参数（IP/端口）可配置
    ///  - 发送 S1F1（Are You There）并显示 S1F2 回复
    ///  - 启动事件报告（作为模拟设备端主动上报 S6F11）
    /// 所有收发消息都会显示在文本框中。
    /// </summary>
    public partial class FrmSecsTest : Form
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // ---- SECS/GEM 通讯对象 ----
        private HsmsConnection _connector;
        private SecsGem _secsGem;
        private CancellationTokenSource _cts;

        private bool _isConnected;

        public FrmSecsTest()
        {
            InitializeComponent();

            // 默认连接参数
            txtIp.Text = "127.0.0.1";
            nudPort.Value = 5000;
            SetConnectedState(false);
        }

        // ===================================================================
        //  连接 / 断开
        // ===================================================================

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (_isConnected)
            {
                Disconnect();
                return;
            }

            string ip = txtIp.Text.Trim();
            if (!IPAddress.TryParse(ip, out var address))
            {
                MessageBox.Show("请输入有效的 IP 地址。", "输入错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                AppendLog($"正在连接 SECS/GEM 设备 {ip}:{(int)nudPort.Value} ...");

                var options = Options.Create(new SecsGemOptions
                {
                    DeviceId = 0,
                    IsActive = true,                       // 主动连接设备（Host 模式）
                    IpAddress = ip,
                    Port = (int)nudPort.Value,
                    T3 = 45_000,
                });

                var logger = new SecsGemNLogAdapter();
                _connector = new HsmsConnection(options, logger);
                _secsGem = new SecsGem(options, _connector, logger);

                _cts = new CancellationTokenSource();

                _connector.ConnectionChanged += OnConnectionChanged;
                _connector.Start(_cts.Token);

                // 启动后台任务：接收设备主动发来的消息（事件报告 S6F11 等）
                _ = ReceivePrimaryMessagesAsync(_cts.Token);

                AppendLog("连接已建立，等待设备响应（S1F1 握手）...");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "SECS/GEM 连接失败。");
                AppendLog($"连接失败：{ex.Message}");
            }
        }

        private void OnConnectionChanged(object sender, ConnectionState state)
        {
            // 回到 UI 线程更新界面
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OnConnectionChanged(sender, state)));
                return;
            }

            switch (state)
            {
                case ConnectionState.Connecting:
                    AppendLog("连接状态：连接中...");
                    break;
                case ConnectionState.Selected:
                    AppendLog("连接状态：已连接（会话已建立）");
                    SetConnectedState(true);
                    break;
                case ConnectionState.Retry:
                    AppendLog("连接状态：重试中...");
                    break;
                default:
                    AppendLog($"连接状态：{state}");
                    break;
            }
        }

        private async void Disconnect()
        {
            try
            {
                _cts?.Cancel();
                if (_connector != null)
                    await _connector.DisposeAsync();
                _secsGem?.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Warn(ex, "断开 SECS/GEM 时发生异常。");
            }
            finally
            {
                _secsGem = null;
                _connector = null;
                _cts = null;
            }

            SetConnectedState(false);
            AppendLog("已断开连接。");
        }

        private void SetConnectedState(bool connected)
        {
            _isConnected = connected;
            btnConnect.Text = connected ? "断开" : "连接";
            btnSendS1F1.Enabled = connected;
            btnStartEventReport.Enabled = connected;
            txtIp.Enabled = !connected;
            nudPort.Enabled = !connected;
        }

        // ===================================================================
        //  发送 S1F1（Are You There）
        // ===================================================================

        private async void btnSendS1F1_Click(object sender, EventArgs e)
        {
            if (_secsGem == null) return;

            try
            {
                AppendLog(">> 发送 S1F1 (Are You There)");

                using var s1f1 = new SecsMessage(1, 1) { SecsItem = Item.L() };
                var reply = await _secsGem.SendAsync(s1f1, _cts.Token);

                if (reply == null)
                {
                    AppendLog("<< 无回复（对方未应答）");
                    return;
                }

                using (reply)
                {
                    AppendLog($"<< 收到 S{reply.S}F{reply.F} 回复：");
                    AppendLog("   " + DescribeItem(reply.SecsItem, 2));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "S1F1 发送失败。");
                AppendLog($"S1F1 发送失败：{ex.Message}");
            }
        }

        // ===================================================================
        //  启动事件报告（模拟设备端主动上报 S6F11）
        // ===================================================================

        private async void btnStartEventReport_Click(object sender, EventArgs e)
        {
            if (_secsGem == null) return;

            try
            {
                AppendLog(">> 发送 S6F11 事件报告（模拟设备主动上报）");

                // S6F11 事件报告，replyExpected=false（GEM 规范中 S6F11 不期望回复）
                using var s6f11 = new SecsMessage(6, 11, replyExpected: false)
                {
                    Name = "EventReport",
                    SecsItem = Item.L(
                        Item.L(
                            Item.U4(0u),     // DATAID
                            Item.U4(1001u)),  // CEID：事件 ID（1001 = 自定义测试事件）
                        Item.L(
                            Item.U4(1u),     // RPTID
                            Item.L(
                                Item.F4(25.5f),  // 温度值
                                Item.F4(60.0f))) // 湿度值
                    ),
                };

                var reply = await _secsGem.SendAsync(s6f11, _cts.Token);

                if (reply == null)
                {
                    AppendLog("<< S6F11 已发送（无回复期望）");
                }
                else
                {
                    using (reply)
                    {
                        AppendLog($"<< 收到 S{reply.S}F{reply.F} 回复：");
                        AppendLog("   " + DescribeItem(reply.SecsItem, 2));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "S6F11 发送失败。");
                AppendLog($"S6F11 发送失败：{ex.Message}");
            }
        }

        // ===================================================================
        //  接收设备主动发来的消息（S6F11 等）
        // ===================================================================

        private async Task ReceivePrimaryMessagesAsync(CancellationToken token)
        {
            try
            {
                await foreach (var wrapper in _secsGem.GetPrimaryMessageAsync(token))
                {
                    using var msg = wrapper.PrimaryMessage;
                    AppendLog($"<< 收到设备主动消息 S{msg.S}F{msg.F}：");
                    AppendLog("   " + DescribeItem(msg.SecsItem, 2));

                    // 有回复期望的消息需要应答，否则对端会 T3 超时
                    if (msg.ReplyExpected)
                    {
                        using var ack = new SecsMessage(msg.S, (byte)(msg.F + 1)) { SecsItem = Item.B((byte)0) };
                        await wrapper.TryReplyAsync(ack, token);
                        AppendLog($">> 已回复 S{ack.S}F{ack.F} 确认");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // 正常断开
            }
            catch (Exception ex)
            {
                Logger.Warn(ex, "接收消息任务结束。");
            }
        }

        // ===================================================================
        //  辅助方法
        // ===================================================================

        /// <summary>将 SecsItem 结构转换为可读文本（递归）</summary>
        private string DescribeItem(Item item, int depth)
        {
            if (item == null) return "(null)";

            var sb = new StringBuilder();
            DescribeItem(item, depth, sb);
            return sb.ToString().TrimEnd();
        }

        private void DescribeItem(Item item, int depth, StringBuilder sb)
        {
            string indent = new string(' ', depth);

            switch (item.Format)
            {
                case SecsFormat.List:
                    sb.AppendLine($"{indent}List[{item.Count}]");
                    for (int i = 0; i < item.Count; i++)
                        DescribeItem(item[i], depth + 2, sb);
                    break;

                case SecsFormat.ASCII:
                    sb.AppendLine($"{indent}A: \"{item.GetString()}\"");
                    break;

                case SecsFormat.Binary:
                    var bytes = item.GetMemory<byte>().ToArray();
                    sb.AppendLine($"{indent}B: {string.Join(" ", bytes)}");
                    break;

                case SecsFormat.Boolean:
                    sb.AppendLine($"{indent}Boolean: {string.Join(" ", item.GetMemory<bool>().ToArray())}");
                    break;

                case SecsFormat.I1:
                    sb.AppendLine($"{indent}I1: {string.Join(" ", item.GetMemory<sbyte>().ToArray())}");
                    break;
                case SecsFormat.I2:
                    sb.AppendLine($"{indent}I2: {string.Join(" ", item.GetMemory<short>().ToArray())}");
                    break;
                case SecsFormat.I4:
                    sb.AppendLine($"{indent}I4: {string.Join(" ", item.GetMemory<int>().ToArray())}");
                    break;
                case SecsFormat.I8:
                    sb.AppendLine($"{indent}I8: {string.Join(" ", item.GetMemory<long>().ToArray())}");
                    break;

                case SecsFormat.U1:
                    sb.AppendLine($"{indent}U1: {string.Join(" ", item.GetMemory<byte>().ToArray())}");
                    break;
                case SecsFormat.U2:
                    sb.AppendLine($"{indent}U2: {string.Join(" ", item.GetMemory<ushort>().ToArray())}");
                    break;
                case SecsFormat.U4:
                    sb.AppendLine($"{indent}U4: {string.Join(" ", item.GetMemory<uint>().ToArray())}");
                    break;
                case SecsFormat.U8:
                    sb.AppendLine($"{indent}U8: {string.Join(" ", item.GetMemory<ulong>().ToArray())}");
                    break;

                case SecsFormat.F4:
                    sb.AppendLine($"{indent}F4: {string.Join(" ", item.GetMemory<float>().ToArray())}");
                    break;
                case SecsFormat.F8:
                    sb.AppendLine($"{indent}F8: {string.Join(" ", item.GetMemory<double>().ToArray())}");
                    break;

                default:
                    sb.AppendLine($"{indent}{item.Format}");
                    break;
            }
        }

        /// <summary>向文本框追加一行日志（自动滚动到底部）</summary>
        private void AppendLog(string text)
        {
            if (txtLog.IsDisposed) return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => AppendLog(text)));
                return;
            }

            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss.fff}] {text}{Environment.NewLine}");
        }

        // ===================================================================
        //  窗体关闭
        // ===================================================================

        private async void FrmSecsTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _cts?.Cancel();
                if (_connector != null)
                    await _connector.DisposeAsync();
                _secsGem?.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Warn(ex, "关闭窗体时清理 SECS/GEM 资源异常。");
            }
        }

        // ===================================================================
        //  NLog 适配器：把 Secs4Net 内部日志接到 NLog
        // ===================================================================

        private sealed class SecsGemNLogAdapter : ISecsGemLogger
        {
            private static readonly Logger Logger = LogManager.GetLogger("Secs4Net");

            public void Debug(string message) => Logger.Debug(message);
            public void Info(string message) => Logger.Info(message);

            public void MessageIn(SecsMessage message, int deviceId)
                => Logger.Info($"<-- 收 S{message.S}F{message.F} (device {deviceId})");

            public void MessageOut(SecsMessage message, int deviceId)
                => Logger.Info($"--> 发 S{message.S}F{message.F} (device {deviceId})");

            public void Warning(string message) => Logger.Warn(message);

            public void Error(string message) => Logger.Error(message);
            public void Error(string message, Exception exception) => Logger.Error(exception, message);
            public void Error(string message, SecsMessage secsMessage, Exception exception)
                => Logger.Error(exception, $"{message} (S{secsMessage?.S}F{secsMessage?.F})");
        }
    }
}
