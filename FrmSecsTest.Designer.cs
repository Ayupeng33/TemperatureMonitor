using System.Drawing;
using System.Windows.Forms;

namespace TemperatureMonitor
{
    partial class FrmSecsTest
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblIp;
        private TextBox txtIp;
        private Label lblPort;
        private NumericUpDown nudPort;
        private Button btnConnect;
        private Button btnSendS1F1;
        private Button btnStartEventReport;
        private TextBox txtLog;
        private GroupBox gbConnection;
        private GroupBox gbActions;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // ========== lblIp ==========
            this.lblIp = new Label();
            this.lblIp.Text = "IP 地址：";
            this.lblIp.Location = new Point(16, 26);
            this.lblIp.Size = new Size(70, 22);
            this.lblIp.Font = new Font("微软雅黑", 9F);

            // ========== txtIp ==========
            this.txtIp = new TextBox();
            this.txtIp.Text = "127.0.0.1";
            this.txtIp.Location = new Point(90, 24);
            this.txtIp.Size = new Size(140, 24);
            this.txtIp.Font = new Font("微软雅黑", 9F);

            // ========== lblPort ==========
            this.lblPort = new Label();
            this.lblPort.Text = "端口：";
            this.lblPort.Location = new Point(240, 26);
            this.lblPort.Size = new Size(50, 22);
            this.lblPort.Font = new Font("微软雅黑", 9F);

            // ========== nudPort ==========
            this.nudPort = new NumericUpDown();
            this.nudPort.Minimum = 1;
            this.nudPort.Maximum = 65535;
            this.nudPort.Value = 5000;
            this.nudPort.Location = new Point(290, 24);
            this.nudPort.Size = new Size(80, 24);
            this.nudPort.Font = new Font("微软雅黑", 9F);

            // ========== btnConnect ==========
            this.btnConnect = new Button();
            this.btnConnect.Text = "连接";
            this.btnConnect.Location = new Point(390, 22);
            this.btnConnect.Size = new Size(80, 28);
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Font = new Font("微软雅黑", 9F);
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);

            // ========== gbConnection ==========
            this.gbConnection = new GroupBox();
            this.gbConnection.Text = "连接参数";
            this.gbConnection.Location = new Point(12, 12);
            this.gbConnection.Size = new Size(490, 62);
            this.gbConnection.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
            this.gbConnection.Controls.Add(this.lblIp);
            this.gbConnection.Controls.Add(this.txtIp);
            this.gbConnection.Controls.Add(this.lblPort);
            this.gbConnection.Controls.Add(this.nudPort);
            this.gbConnection.Controls.Add(this.btnConnect);

            // ========== btnSendS1F1 ==========
            this.btnSendS1F1 = new Button();
            this.btnSendS1F1.Text = "发送 S1F1";
            this.btnSendS1F1.Location = new Point(16, 24);
            this.btnSendS1F1.Size = new Size(130, 30);
            this.btnSendS1F1.UseVisualStyleBackColor = true;
            this.btnSendS1F1.Font = new Font("微软雅黑", 9F);
            this.btnSendS1F1.Enabled = false;
            this.btnSendS1F1.Click += new System.EventHandler(this.btnSendS1F1_Click);

            // ========== btnStartEventReport ==========
            this.btnStartEventReport = new Button();
            this.btnStartEventReport.Text = "启动事件报告";
            this.btnStartEventReport.Location = new Point(160, 24);
            this.btnStartEventReport.Size = new Size(130, 30);
            this.btnStartEventReport.UseVisualStyleBackColor = true;
            this.btnStartEventReport.Font = new Font("微软雅黑", 9F);
            this.btnStartEventReport.Enabled = false;
            this.btnStartEventReport.Click += new System.EventHandler(this.btnStartEventReport_Click);

            // ========== gbActions ==========
            this.gbActions = new GroupBox();
            this.gbActions.Text = "通讯操作";
            this.gbActions.Location = new Point(12, 82);
            this.gbActions.Size = new Size(490, 66);
            this.gbActions.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
            this.gbActions.Controls.Add(this.btnSendS1F1);
            this.gbActions.Controls.Add(this.btnStartEventReport);

            // ========== txtLog ==========
            this.txtLog = new TextBox();
            this.txtLog.Multiline = true;
            this.txtLog.ScrollBars = ScrollBars.Vertical;
            this.txtLog.ReadOnly = true;
            this.txtLog.WordWrap = false;
            this.txtLog.Location = new Point(12, 156);
            this.txtLog.Size = new Size(490, 260);
            this.txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Left |
                                AnchorStyles.Right | AnchorStyles.Bottom;
            this.txtLog.Font = new Font("Consolas", 9F);

            // ========== FrmSecsTest ==========
            this.AutoScaleDimensions = new SizeF(7F, 17F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(514, 428);
            this.Controls.Add(this.gbConnection);
            this.Controls.Add(this.gbActions);
            this.Controls.Add(this.txtLog);
            this.Font = new Font("微软雅黑", 9F);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "SECS/GEM 通讯测试";
            this.FormClosing += new FormClosingEventHandler(this.FrmSecsTest_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
