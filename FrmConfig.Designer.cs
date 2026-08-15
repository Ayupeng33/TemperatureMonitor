using System.Drawing;
using System.Windows.Forms;

namespace TemperatureMonitor
{
    partial class FrmConfig
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblIpAddress;
        private TextBox txtIpAddress;
        private Label lblPort;
        private NumericUpDown nudPort;
        private Label lblSamplingInterval;
        private NumericUpDown nudSamplingInterval;
        private Label lblSamplingUnit;
        private Label lblAlarmUpper;
        private NumericUpDown nudAlarmUpper;
        private Label lblAlarmUpperUnit;
        private Label lblAlarmLower;
        private NumericUpDown nudAlarmLower;
        private Label lblAlarmLowerUnit;
        private Button btnSave;
        private Button btnCancel;
        private GroupBox gbPlc;
        private GroupBox gbSampling;
        private GroupBox gbAlarm;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // ========== lblIpAddress ==========
            this.lblIpAddress = new Label();
            this.lblIpAddress.Text = "PLC IP 地址：";
            this.lblIpAddress.Location = new Point(20, 28);
            this.lblIpAddress.Size = new Size(100, 22);
            this.lblIpAddress.Font = new Font("微软雅黑", 9F);

            // ========== txtIpAddress ==========
            this.txtIpAddress = new TextBox();
            this.txtIpAddress.Text = "127.0.0.1";
            this.txtIpAddress.Location = new Point(125, 25);
            this.txtIpAddress.Size = new Size(150, 24);
            this.txtIpAddress.Font = new Font("微软雅黑", 9F);

            // ========== lblPort ==========
            this.lblPort = new Label();
            this.lblPort.Text = "端 口 号：";
            this.lblPort.Location = new Point(20, 60);
            this.lblPort.Size = new Size(100, 22);
            this.lblPort.Font = new Font("微软雅黑", 9F);

            // ========== nudPort ==========
            this.nudPort = new NumericUpDown();
            this.nudPort.Minimum = 1;
            this.nudPort.Maximum = 65535;
            this.nudPort.Value = 502;
            this.nudPort.Location = new Point(125, 58);
            this.nudPort.Size = new Size(80, 24);
            this.nudPort.Font = new Font("微软雅黑", 9F);

            // ========== gbPlc ==========
            this.gbPlc = new GroupBox();
            this.gbPlc.Text = "PLC 连接参数";
            this.gbPlc.Location = new Point(12, 12);
            this.gbPlc.Size = new Size(300, 100);
            this.gbPlc.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
            this.gbPlc.Controls.Add(this.lblIpAddress);
            this.gbPlc.Controls.Add(this.txtIpAddress);
            this.gbPlc.Controls.Add(this.lblPort);
            this.gbPlc.Controls.Add(this.nudPort);

            // ========== lblSamplingInterval ==========
            this.lblSamplingInterval = new Label();
            this.lblSamplingInterval.Text = "采样间隔：";
            this.lblSamplingInterval.Location = new Point(20, 28);
            this.lblSamplingInterval.Size = new Size(100, 22);
            this.lblSamplingInterval.Font = new Font("微软雅黑", 9F);

            // ========== nudSamplingInterval ==========
            this.nudSamplingInterval = new NumericUpDown();
            this.nudSamplingInterval.Minimum = 100;
            this.nudSamplingInterval.Maximum = 60000;
            this.nudSamplingInterval.Value = 1000;
            this.nudSamplingInterval.Increment = 100;
            this.nudSamplingInterval.Location = new Point(125, 26);
            this.nudSamplingInterval.Size = new Size(80, 24);
            this.nudSamplingInterval.Font = new Font("微软雅黑", 9F);

            // ========== lblSamplingUnit ==========
            this.lblSamplingUnit = new Label();
            this.lblSamplingUnit.Text = "毫秒";
            this.lblSamplingUnit.Location = new Point(210, 28);
            this.lblSamplingUnit.Size = new Size(50, 22);
            this.lblSamplingUnit.Font = new Font("微软雅黑", 9F);

            // ========== gbSampling ==========
            this.gbSampling = new GroupBox();
            this.gbSampling.Text = "采集参数";
            this.gbSampling.Location = new Point(12, 120);
            this.gbSampling.Size = new Size(300, 66);
            this.gbSampling.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
            this.gbSampling.Controls.Add(this.lblSamplingInterval);
            this.gbSampling.Controls.Add(this.nudSamplingInterval);
            this.gbSampling.Controls.Add(this.lblSamplingUnit);

            // ========== lblAlarmUpper ==========
            this.lblAlarmUpper = new Label();
            this.lblAlarmUpper.Text = "报警上限：";
            this.lblAlarmUpper.Location = new Point(20, 28);
            this.lblAlarmUpper.Size = new Size(100, 22);
            this.lblAlarmUpper.Font = new Font("微软雅黑", 9F);

            // ========== nudAlarmUpper ==========
            this.nudAlarmUpper = new NumericUpDown();
            this.nudAlarmUpper.Minimum = 0;
            this.nudAlarmUpper.Maximum = 500;
            this.nudAlarmUpper.Value = 80;
            this.nudAlarmUpper.DecimalPlaces = 1;
            this.nudAlarmUpper.Increment = 0.5m;
            this.nudAlarmUpper.Location = new Point(125, 26);
            this.nudAlarmUpper.Size = new Size(80, 24);
            this.nudAlarmUpper.Font = new Font("微软雅黑", 9F);

            // ========== lblAlarmUpperUnit ==========
            this.lblAlarmUpperUnit = new Label();
            this.lblAlarmUpperUnit.Text = "℃";
            this.lblAlarmUpperUnit.Location = new Point(210, 28);
            this.lblAlarmUpperUnit.Size = new Size(30, 22);
            this.lblAlarmUpperUnit.Font = new Font("微软雅黑", 9F);

            // ========== lblAlarmLower ==========
            this.lblAlarmLower = new Label();
            this.lblAlarmLower.Text = "报警下限：";
            this.lblAlarmLower.Location = new Point(20, 58);
            this.lblAlarmLower.Size = new Size(100, 22);
            this.lblAlarmLower.Font = new Font("微软雅黑", 9F);

            // ========== nudAlarmLower ==========
            this.nudAlarmLower = new NumericUpDown();
            this.nudAlarmLower.Minimum = -50;
            this.nudAlarmLower.Maximum = 500;
            this.nudAlarmLower.Value = 0;
            this.nudAlarmLower.DecimalPlaces = 1;
            this.nudAlarmLower.Increment = 0.5m;
            this.nudAlarmLower.Location = new Point(125, 56);
            this.nudAlarmLower.Size = new Size(80, 24);
            this.nudAlarmLower.Font = new Font("微软雅黑", 9F);

            // ========== lblAlarmLowerUnit ==========
            this.lblAlarmLowerUnit = new Label();
            this.lblAlarmLowerUnit.Text = "℃";
            this.lblAlarmLowerUnit.Location = new Point(210, 58);
            this.lblAlarmLowerUnit.Size = new Size(30, 22);
            this.lblAlarmLowerUnit.Font = new Font("微软雅黑", 9F);

            // ========== gbAlarm ==========
            this.gbAlarm = new GroupBox();
            this.gbAlarm.Text = "报警阈值";
            this.gbAlarm.Location = new Point(12, 194);
            this.gbAlarm.Size = new Size(300, 96);
            this.gbAlarm.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
            this.gbAlarm.Controls.Add(this.lblAlarmUpper);
            this.gbAlarm.Controls.Add(this.nudAlarmUpper);
            this.gbAlarm.Controls.Add(this.lblAlarmUpperUnit);
            this.gbAlarm.Controls.Add(this.lblAlarmLower);
            this.gbAlarm.Controls.Add(this.nudAlarmLower);
            this.gbAlarm.Controls.Add(this.lblAlarmLowerUnit);

            // ========== btnSave ==========
            this.btnSave = new Button();
            this.btnSave.Text = "保存";
            this.btnSave.Location = new Point(65, 305);
            this.btnSave.Size = new Size(85, 30);
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Font = new Font("微软雅黑", 9F);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // ========== btnCancel ==========
            this.btnCancel = new Button();
            this.btnCancel.Text = "取消";
            this.btnCancel.Location = new Point(170, 305);
            this.btnCancel.Size = new Size(85, 30);
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Font = new Font("微软雅黑", 9F);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // ========== FrmConfig ==========
            this.AutoScaleDimensions = new SizeF(7F, 17F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(325, 350);
            this.Controls.Add(this.gbPlc);
            this.Controls.Add(this.gbSampling);
            this.Controls.Add(this.gbAlarm);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Font = new Font("微软雅黑", 9F);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "系统设置";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
