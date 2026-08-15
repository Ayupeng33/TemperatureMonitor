using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TemperatureMonitor
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // ---- 控件声明 ----
        private System.Windows.Forms.Timer timerDataCollection;
        private System.Windows.Forms.Timer timerAlarmBlink;
        private Panel pnlTitleBar;
        private Label lblTitle;
        private Button btnMinimize;
        private Button btnMaximize;
        private Button btnClose;
        private Panel pnlAlarm;
        private Panel pnlAlarm1;
        private Panel pnlAlarm2;
        private Panel pnlAlarm3;
        private Panel pnlAlarm4;
        private Label lblAlarmStatus;
        private Label lblChannel1;
        private Label lblChannel2;
        private Label lblChannel3;
        private Label lblChannel4;
        private Panel pnlCard1;
        private Panel pnlCard2;
        private Panel pnlCard3;
        private Panel pnlCard4;
        private Chart chartTemperature;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel tsslConnectionStatus;
        private Button btnConnect;
        private Button btnConfig;
        private Button btnQueryHistory;
        private Button btnSecsTest;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsmiSecsTest;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
      System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
      System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
      System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
      System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
      System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
      this.timerDataCollection = new System.Windows.Forms.Timer(this.components);
      this.timerAlarmBlink = new System.Windows.Forms.Timer(this.components);
      this.pnlTitleBar = new System.Windows.Forms.Panel();
      this.lblTitle = new System.Windows.Forms.Label();
      this.btnMinimize = new System.Windows.Forms.Button();
      this.btnMaximize = new System.Windows.Forms.Button();
      this.btnClose = new System.Windows.Forms.Button();
      this.pnlAlarm = new System.Windows.Forms.Panel();
      this.pnlAlarm1 = new System.Windows.Forms.Panel();
      this.pnlAlarm2 = new System.Windows.Forms.Panel();
      this.pnlAlarm3 = new System.Windows.Forms.Panel();
      this.pnlAlarm4 = new System.Windows.Forms.Panel();
      this.lblAlarmStatus = new System.Windows.Forms.Label();
      this.btnConnect = new System.Windows.Forms.Button();
      this.btnConfig = new System.Windows.Forms.Button();
      this.btnQueryHistory = new System.Windows.Forms.Button();
      this.btnSecsTest = new System.Windows.Forms.Button();
      this.menuStrip = new System.Windows.Forms.MenuStrip();
      this.tsmiSecsTest = new System.Windows.Forms.ToolStripMenuItem();
      this.lblChannel1 = new System.Windows.Forms.Label();
      this.lblChannel2 = new System.Windows.Forms.Label();
      this.lblChannel3 = new System.Windows.Forms.Label();
      this.lblChannel4 = new System.Windows.Forms.Label();
      this.pnlCard1 = new System.Windows.Forms.Panel();
      this.pnlCard2 = new System.Windows.Forms.Panel();
      this.pnlCard3 = new System.Windows.Forms.Panel();
      this.pnlCard4 = new System.Windows.Forms.Panel();
      this.chartTemperature = new System.Windows.Forms.DataVisualization.Charting.Chart();
      this.statusStrip = new System.Windows.Forms.StatusStrip();
      this.tsslConnectionStatus = new System.Windows.Forms.ToolStripStatusLabel();
      this.pnlTitleBar.SuspendLayout();
      this.menuStrip.SuspendLayout();
      this.pnlCard1.SuspendLayout();
      this.pnlCard2.SuspendLayout();
      this.pnlCard3.SuspendLayout();
      this.pnlCard4.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.chartTemperature)).BeginInit();
      this.statusStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // timerDataCollection
      // 
      this.timerDataCollection.Interval = 1000;
      this.timerDataCollection.Tick += new System.EventHandler(this.timerDataCollection_Tick);
      // 
      // timerAlarmBlink
      // 
      this.timerAlarmBlink.Interval = 500;
      this.timerAlarmBlink.Tick += new System.EventHandler(this.timerAlarmBlink_Tick);
      // 
      // pnlTitleBar
      // 
      this.pnlTitleBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pnlTitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
      this.pnlTitleBar.Controls.Add(this.lblTitle);
      this.pnlTitleBar.Controls.Add(this.btnMinimize);
      this.pnlTitleBar.Controls.Add(this.btnMaximize);
      this.pnlTitleBar.Controls.Add(this.btnClose);
      this.pnlTitleBar.Location = new System.Drawing.Point(0, 0);
      this.pnlTitleBar.Name = "pnlTitleBar";
      this.pnlTitleBar.Size = new System.Drawing.Size(900, 42);
      this.pnlTitleBar.TabIndex = 17;
      this.pnlTitleBar.DoubleClick += new System.EventHandler(this.TitleBar_DoubleClick);
      this.pnlTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseDown);
      this.pnlTitleBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseMove);
      this.pnlTitleBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseUp);
      // 
      // lblTitle
      // 
      this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
      this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
      this.lblTitle.Location = new System.Drawing.Point(14, 9);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new System.Drawing.Size(320, 26);
      this.lblTitle.TabIndex = 0;
      this.lblTitle.Text = "温度监测系统";
      this.lblTitle.DoubleClick += new System.EventHandler(this.TitleBar_DoubleClick);
      this.lblTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseDown);
      this.lblTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseMove);
      this.lblTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseUp);
      // 
      // btnMinimize
      // 
      this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnMinimize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
      this.btnMinimize.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
      this.btnMinimize.FlatAppearance.BorderSize = 0;
      this.btnMinimize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(228)))), ((int)(((byte)(234)))));
      this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnMinimize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
      this.btnMinimize.Location = new System.Drawing.Point(782, 6);
      this.btnMinimize.Name = "btnMinimize";
      this.btnMinimize.Size = new System.Drawing.Size(36, 30);
      this.btnMinimize.TabIndex = 18;
      this.btnMinimize.TabStop = false;
      this.btnMinimize.UseVisualStyleBackColor = false;
      this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
      this.btnMinimize.Paint += new System.Windows.Forms.PaintEventHandler(this.SysBtnPaint);
      this.btnMinimize.MouseEnter += new System.EventHandler(this.SysBtn_MouseEnter);
      this.btnMinimize.MouseLeave += new System.EventHandler(this.SysBtn_MouseLeave);
      // 
      // btnMaximize
      // 
      this.btnMaximize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnMaximize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
      this.btnMaximize.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
      this.btnMaximize.FlatAppearance.BorderSize = 0;
      this.btnMaximize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(228)))), ((int)(((byte)(234)))));
      this.btnMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnMaximize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
      this.btnMaximize.Location = new System.Drawing.Point(820, 6);
      this.btnMaximize.Name = "btnMaximize";
      this.btnMaximize.Size = new System.Drawing.Size(36, 30);
      this.btnMaximize.TabIndex = 24;
      this.btnMaximize.TabStop = false;
      this.btnMaximize.UseVisualStyleBackColor = false;
      this.btnMaximize.Click += new System.EventHandler(this.btnMaximize_Click);
      this.btnMaximize.Paint += new System.Windows.Forms.PaintEventHandler(this.SysBtnPaint);
      this.btnMaximize.MouseEnter += new System.EventHandler(this.SysBtn_MouseEnter);
      this.btnMaximize.MouseLeave += new System.EventHandler(this.SysBtn_MouseLeave);
      // 
      // btnClose
      // 
      this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
      this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
      this.btnClose.FlatAppearance.BorderSize = 0;
      this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(17)))), ((int)(((byte)(35)))));
      this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
      this.btnClose.Location = new System.Drawing.Point(858, 6);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(36, 30);
      this.btnClose.TabIndex = 19;
      this.btnClose.TabStop = false;
      this.btnClose.UseVisualStyleBackColor = false;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      this.btnClose.Paint += new System.Windows.Forms.PaintEventHandler(this.SysBtnPaint);
      this.btnClose.MouseEnter += new System.EventHandler(this.SysBtn_MouseEnter);
      this.btnClose.MouseLeave += new System.EventHandler(this.SysBtn_MouseLeave);
      // 
      // pnlAlarm
      // 
      this.pnlAlarm.BackColor = System.Drawing.Color.LimeGreen;
      this.pnlAlarm.Location = new System.Drawing.Point(16, 88);
      this.pnlAlarm.Name = "pnlAlarm";
      this.pnlAlarm.Size = new System.Drawing.Size(30, 30);
      this.pnlAlarm.TabIndex = 0;
      this.pnlAlarm.Paint += new System.Windows.Forms.PaintEventHandler(this.AlarmLightPaint);
      // 
      // pnlAlarm1
      // 
      this.pnlAlarm1.BackColor = System.Drawing.Color.LimeGreen;
      this.pnlAlarm1.Location = new System.Drawing.Point(16, 37);
      this.pnlAlarm1.Name = "pnlAlarm1";
      this.pnlAlarm1.Size = new System.Drawing.Size(24, 24);
      this.pnlAlarm1.TabIndex = 10;
      this.pnlAlarm1.Paint += new System.Windows.Forms.PaintEventHandler(this.AlarmLightPaint);
      // 
      // pnlAlarm2
      // 
      this.pnlAlarm2.BackColor = System.Drawing.Color.LimeGreen;
      this.pnlAlarm2.Location = new System.Drawing.Point(16, 37);
      this.pnlAlarm2.Name = "pnlAlarm2";
      this.pnlAlarm2.Size = new System.Drawing.Size(24, 24);
      this.pnlAlarm2.TabIndex = 11;
      this.pnlAlarm2.Paint += new System.Windows.Forms.PaintEventHandler(this.AlarmLightPaint);
      // 
      // pnlAlarm3
      // 
      this.pnlAlarm3.BackColor = System.Drawing.Color.LimeGreen;
      this.pnlAlarm3.Location = new System.Drawing.Point(16, 37);
      this.pnlAlarm3.Name = "pnlAlarm3";
      this.pnlAlarm3.Size = new System.Drawing.Size(24, 24);
      this.pnlAlarm3.TabIndex = 12;
      this.pnlAlarm3.Paint += new System.Windows.Forms.PaintEventHandler(this.AlarmLightPaint);
      // 
      // pnlAlarm4
      // 
      this.pnlAlarm4.BackColor = System.Drawing.Color.LimeGreen;
      this.pnlAlarm4.Location = new System.Drawing.Point(16, 37);
      this.pnlAlarm4.Name = "pnlAlarm4";
      this.pnlAlarm4.Size = new System.Drawing.Size(24, 24);
      this.pnlAlarm4.TabIndex = 13;
      this.pnlAlarm4.Paint += new System.Windows.Forms.PaintEventHandler(this.AlarmLightPaint);
      // 
      // lblAlarmStatus
      // 
      this.lblAlarmStatus.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
      this.lblAlarmStatus.ForeColor = System.Drawing.Color.LimeGreen;
      this.lblAlarmStatus.Location = new System.Drawing.Point(58, 90);
      this.lblAlarmStatus.Name = "lblAlarmStatus";
      this.lblAlarmStatus.Size = new System.Drawing.Size(150, 26);
      this.lblAlarmStatus.TabIndex = 1;
      this.lblAlarmStatus.Text = "系统正常";
      // 
      // btnConnect
      // 
      this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(108)))), ((int)(((byte)(176)))));
      this.btnConnect.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(108)))), ((int)(((byte)(176)))));
      this.btnConnect.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(126)))), ((int)(((byte)(207)))));
      this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnConnect.Font = new System.Drawing.Font("微软雅黑", 9.5F);
      this.btnConnect.ForeColor = System.Drawing.Color.White;
      this.btnConnect.Location = new System.Drawing.Point(804, 86);
      this.btnConnect.Name = "btnConnect";
      this.btnConnect.Size = new System.Drawing.Size(92, 32);
      this.btnConnect.TabIndex = 2;
      this.btnConnect.Text = "连接";
      this.btnConnect.UseVisualStyleBackColor = false;
      this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
      // 
      // btnConfig
      // 
      this.btnConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnConfig.BackColor = System.Drawing.Color.White;
      this.btnConfig.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(213)))), ((int)(((byte)(221)))));
      this.btnConfig.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
      this.btnConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnConfig.Font = new System.Drawing.Font("微软雅黑", 9.5F);
      this.btnConfig.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
      this.btnConfig.Location = new System.Drawing.Point(704, 86);
      this.btnConfig.Name = "btnConfig";
      this.btnConfig.Size = new System.Drawing.Size(92, 32);
      this.btnConfig.TabIndex = 14;
      this.btnConfig.Text = "设置";
      this.btnConfig.UseVisualStyleBackColor = false;
      this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
      // 
      // btnQueryHistory
      // 
      this.btnQueryHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnQueryHistory.BackColor = System.Drawing.Color.White;
      this.btnQueryHistory.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(213)))), ((int)(((byte)(221)))));
      this.btnQueryHistory.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
      this.btnQueryHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnQueryHistory.Font = new System.Drawing.Font("微软雅黑", 9.5F);
      this.btnQueryHistory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
      this.btnQueryHistory.Location = new System.Drawing.Point(592, 86);
      this.btnQueryHistory.Name = "btnQueryHistory";
      this.btnQueryHistory.Size = new System.Drawing.Size(104, 32);
      this.btnQueryHistory.TabIndex = 15;
      this.btnQueryHistory.Text = "查询历史";
      this.btnQueryHistory.UseVisualStyleBackColor = false;
      this.btnQueryHistory.Click += new System.EventHandler(this.btnQueryHistory_Click);
      // 
      // btnSecsTest
      // 
      this.btnSecsTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSecsTest.BackColor = System.Drawing.Color.White;
      this.btnSecsTest.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(213)))), ((int)(((byte)(221)))));
      this.btnSecsTest.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
      this.btnSecsTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnSecsTest.Font = new System.Drawing.Font("微软雅黑", 9.5F);
      this.btnSecsTest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
      this.btnSecsTest.Location = new System.Drawing.Point(474, 86);
      this.btnSecsTest.Name = "btnSecsTest";
      this.btnSecsTest.Size = new System.Drawing.Size(112, 32);
      this.btnSecsTest.TabIndex = 25;
      this.btnSecsTest.Text = "SECS/GEM 测试";
      this.btnSecsTest.UseVisualStyleBackColor = false;
      this.btnSecsTest.Click += new System.EventHandler(this.tsmiSecsTest_Click);
      // 
      // menuStrip
      // 
      this.menuStrip.BackColor = System.Drawing.Color.White;
      this.menuStrip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
      this.menuStrip.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
      this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
      this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSecsTest});
      this.menuStrip.Location = new System.Drawing.Point(0, 0);
      this.menuStrip.Name = "menuStrip";
      this.menuStrip.Size = new System.Drawing.Size(900, 32);
      this.menuStrip.TabIndex = 16;
      this.menuStrip.Text = "menuStrip";
      // 
      // tsmiSecsTest
      // 
      this.tsmiSecsTest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
      this.tsmiSecsTest.Name = "tsmiSecsTest";
      this.tsmiSecsTest.Size = new System.Drawing.Size(194, 28);
      this.tsmiSecsTest.Text = "SECS/GEM 通讯测试";
      this.tsmiSecsTest.Click += new System.EventHandler(this.tsmiSecsTest_Click);
      // 
      // lblChannel1
      // 
      this.lblChannel1.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
      this.lblChannel1.ForeColor = System.Drawing.Color.DodgerBlue;
      this.lblChannel1.Location = new System.Drawing.Point(48, 25);
      this.lblChannel1.Name = "lblChannel1";
      this.lblChannel1.Size = new System.Drawing.Size(148, 48);
      this.lblChannel1.TabIndex = 3;
      this.lblChannel1.Text = "通道 1：--- ℃";
      // 
      // lblChannel2
      // 
      this.lblChannel2.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
      this.lblChannel2.ForeColor = System.Drawing.Color.OrangeRed;
      this.lblChannel2.Location = new System.Drawing.Point(48, 25);
      this.lblChannel2.Name = "lblChannel2";
      this.lblChannel2.Size = new System.Drawing.Size(148, 48);
      this.lblChannel2.TabIndex = 4;
      this.lblChannel2.Text = "通道 2：--- ℃";
      // 
      // lblChannel3
      // 
      this.lblChannel3.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
      this.lblChannel3.ForeColor = System.Drawing.Color.ForestGreen;
      this.lblChannel3.Location = new System.Drawing.Point(48, 25);
      this.lblChannel3.Name = "lblChannel3";
      this.lblChannel3.Size = new System.Drawing.Size(148, 48);
      this.lblChannel3.TabIndex = 5;
      this.lblChannel3.Text = "通道 3：--- ℃";
      // 
      // lblChannel4
      // 
      this.lblChannel4.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
      this.lblChannel4.ForeColor = System.Drawing.Color.Purple;
      this.lblChannel4.Location = new System.Drawing.Point(48, 25);
      this.lblChannel4.Name = "lblChannel4";
      this.lblChannel4.Size = new System.Drawing.Size(148, 48);
      this.lblChannel4.TabIndex = 6;
      this.lblChannel4.Text = "通道 4：--- ℃";
      // 
      // pnlCard1
      // 
      this.pnlCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
      this.pnlCard1.Controls.Add(this.pnlAlarm1);
      this.pnlCard1.Controls.Add(this.lblChannel1);
      this.pnlCard1.Location = new System.Drawing.Point(14, 130);
      this.pnlCard1.Name = "pnlCard1";
      this.pnlCard1.Size = new System.Drawing.Size(208, 98);
      this.pnlCard1.TabIndex = 20;
      this.pnlCard1.Paint += new System.Windows.Forms.PaintEventHandler(this.CardPaint);
      // 
      // pnlCard2
      // 
      this.pnlCard2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
      this.pnlCard2.Controls.Add(this.pnlAlarm2);
      this.pnlCard2.Controls.Add(this.lblChannel2);
      this.pnlCard2.Location = new System.Drawing.Point(236, 130);
      this.pnlCard2.Name = "pnlCard2";
      this.pnlCard2.Size = new System.Drawing.Size(208, 98);
      this.pnlCard2.TabIndex = 21;
      this.pnlCard2.Paint += new System.Windows.Forms.PaintEventHandler(this.CardPaint);
      // 
      // pnlCard3
      // 
      this.pnlCard3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
      this.pnlCard3.Controls.Add(this.pnlAlarm3);
      this.pnlCard3.Controls.Add(this.lblChannel3);
      this.pnlCard3.Location = new System.Drawing.Point(458, 130);
      this.pnlCard3.Name = "pnlCard3";
      this.pnlCard3.Size = new System.Drawing.Size(208, 98);
      this.pnlCard3.TabIndex = 22;
      this.pnlCard3.Paint += new System.Windows.Forms.PaintEventHandler(this.CardPaint);
      // 
      // pnlCard4
      // 
      this.pnlCard4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
      this.pnlCard4.Controls.Add(this.pnlAlarm4);
      this.pnlCard4.Controls.Add(this.lblChannel4);
      this.pnlCard4.Location = new System.Drawing.Point(680, 130);
      this.pnlCard4.Name = "pnlCard4";
      this.pnlCard4.Size = new System.Drawing.Size(208, 98);
      this.pnlCard4.TabIndex = 23;
      this.pnlCard4.Paint += new System.Windows.Forms.PaintEventHandler(this.CardPaint);
      // 
      // chartTemperature
      // 
      this.chartTemperature.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      chartArea1.AxisX.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      chartArea1.AxisX.LabelStyle.Format = "MM-dd HH:mm";
      chartArea1.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Minutes;
      chartArea1.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(205)))), ((int)(((byte)(214)))));
      chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(232)))), ((int)(((byte)(236)))));
      chartArea1.AxisX.Title = "时间";
      chartArea1.AxisX.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      chartArea1.AxisY.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      chartArea1.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(205)))), ((int)(((byte)(214)))));
      chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(232)))), ((int)(((byte)(236)))));
      chartArea1.AxisY.Maximum = 150D;
      chartArea1.AxisY.Minimum = 0D;
      chartArea1.AxisY.Title = "温度 (℃)";
      chartArea1.AxisY.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      chartArea1.BackColor = System.Drawing.Color.White;
      chartArea1.Name = "DefaultArea";
      this.chartTemperature.ChartAreas.Add(chartArea1);
      legend1.BackColor = System.Drawing.Color.Transparent;
      legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
      legend1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
      legend1.Name = "DefaultLegend";
      this.chartTemperature.Legends.Add(legend1);
      this.chartTemperature.Location = new System.Drawing.Point(14, 240);
      this.chartTemperature.Name = "chartTemperature";
      series1.BorderWidth = 2;
      series1.ChartArea = "DefaultArea";
      series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
      series1.Color = System.Drawing.Color.DodgerBlue;
      series1.Legend = "DefaultLegend";
      series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
      series1.Name = "通道 1";
      series2.BorderWidth = 2;
      series2.ChartArea = "DefaultArea";
      series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
      series2.Color = System.Drawing.Color.OrangeRed;
      series2.Legend = "DefaultLegend";
      series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
      series2.Name = "通道 2";
      series3.BorderWidth = 2;
      series3.ChartArea = "DefaultArea";
      series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
      series3.Color = System.Drawing.Color.ForestGreen;
      series3.Legend = "DefaultLegend";
      series3.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
      series3.Name = "通道 3";
      series4.BorderWidth = 2;
      series4.ChartArea = "DefaultArea";
      series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
      series4.Color = System.Drawing.Color.Purple;
      series4.Legend = "DefaultLegend";
      series4.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
      series4.Name = "通道 4";
      this.chartTemperature.Series.Add(series1);
      this.chartTemperature.Series.Add(series2);
      this.chartTemperature.Series.Add(series3);
      this.chartTemperature.Series.Add(series4);
      this.chartTemperature.Size = new System.Drawing.Size(872, 289);
      this.chartTemperature.TabIndex = 7;
      // 
      // statusStrip
      // 
      this.statusStrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.statusStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
      this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
      this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
      this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslConnectionStatus});
      this.statusStrip.Location = new System.Drawing.Point(0, 529);
      this.statusStrip.Name = "statusStrip";
      this.statusStrip.Size = new System.Drawing.Size(81, 31);
      this.statusStrip.TabIndex = 0;
      // 
      // tsslConnectionStatus
      // 
      this.tsslConnectionStatus.Font = new System.Drawing.Font("微软雅黑", 9F);
      this.tsslConnectionStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
      this.tsslConnectionStatus.Name = "tsslConnectionStatus";
      this.tsslConnectionStatus.Size = new System.Drawing.Size(64, 24);
      this.tsslConnectionStatus.Text = "未连接";
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.White;
      this.ClientSize = new System.Drawing.Size(900, 560);
      this.Controls.Add(this.pnlTitleBar);
      this.Controls.Add(this.menuStrip);
      this.Controls.Add(this.pnlAlarm);
      this.Controls.Add(this.lblAlarmStatus);
      this.Controls.Add(this.btnConnect);
      this.Controls.Add(this.btnConfig);
      this.Controls.Add(this.btnQueryHistory);
      this.Controls.Add(this.btnSecsTest);
      this.Controls.Add(this.pnlCard1);
      this.Controls.Add(this.pnlCard2);
      this.Controls.Add(this.pnlCard3);
      this.Controls.Add(this.pnlCard4);
      this.Controls.Add(this.chartTemperature);
      this.Controls.Add(this.statusStrip);
      this.Font = new System.Drawing.Font("微软雅黑", 9F);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.MainMenuStrip = this.menuStrip;
      this.Name = "MainForm";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "温度监测系统";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
      this.Resize += new System.EventHandler(this.MainForm_Resize);
      this.pnlTitleBar.ResumeLayout(false);
      this.menuStrip.ResumeLayout(false);
      this.menuStrip.PerformLayout();
      this.pnlCard1.ResumeLayout(false);
      this.pnlCard2.ResumeLayout(false);
      this.pnlCard3.ResumeLayout(false);
      this.pnlCard4.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.chartTemperature)).EndInit();
      this.statusStrip.ResumeLayout(false);
      this.statusStrip.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

        }
    }
}
