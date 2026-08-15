using System.Drawing;
using System.Windows.Forms;

namespace TemperatureMonitor
{
    partial class FrmQuery
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblStart;
        private DateTimePicker dtpStart;
        private Label lblEnd;
        private DateTimePicker dtpEnd;
        private GroupBox gbTimeRange;
        private GroupBox gbType;
        private RadioButton rbTemperature;
        private RadioButton rbAlarm;
        private Button btnQuery;
        private Button btnExportCsv;
        private DataGridView dgvResult;
        private Label lblRecordCount;
        private SaveFileDialog saveFileDialog;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.saveFileDialog = new SaveFileDialog();

            // ========== gbTimeRange ==========
            this.lblStart = new Label();
            this.lblStart.Text = "开始时间：";
            this.lblStart.Location = new Point(14, 24);
            this.lblStart.Size = new Size(70, 22);

            this.dtpStart = new DateTimePicker();
            this.dtpStart.Format = DateTimePickerFormat.Custom;
            this.dtpStart.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpStart.ShowUpDown = false;
            this.dtpStart.Location = new Point(88, 22);
            this.dtpStart.Size = new Size(175, 24);

            this.lblEnd = new Label();
            this.lblEnd.Text = "结束时间：";
            this.lblEnd.Location = new Point(275, 24);
            this.lblEnd.Size = new Size(70, 22);

            this.dtpEnd = new DateTimePicker();
            this.dtpEnd.Format = DateTimePickerFormat.Custom;
            this.dtpEnd.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpEnd.ShowUpDown = false;
            this.dtpEnd.Location = new Point(349, 22);
            this.dtpEnd.Size = new Size(175, 24);

            this.gbTimeRange = new GroupBox();
            this.gbTimeRange.Text = "查询时间段";
            this.gbTimeRange.Location = new Point(12, 12);
            this.gbTimeRange.Size = new Size(545, 60);
            this.gbTimeRange.Controls.Add(this.lblStart);
            this.gbTimeRange.Controls.Add(this.dtpStart);
            this.gbTimeRange.Controls.Add(this.lblEnd);
            this.gbTimeRange.Controls.Add(this.dtpEnd);

            // ========== gbType ==========
            this.rbTemperature = new RadioButton();
            this.rbTemperature.Text = "温度记录";
            this.rbTemperature.Location = new Point(14, 24);
            this.rbTemperature.Size = new Size(90, 22);
            this.rbTemperature.Checked = true;

            this.rbAlarm = new RadioButton();
            this.rbAlarm.Text = "报警记录";
            this.rbAlarm.Location = new Point(120, 24);
            this.rbAlarm.Size = new Size(90, 22);

            this.gbType = new GroupBox();
            this.gbType.Text = "查询类型";
            this.gbType.Location = new Point(570, 12);
            this.gbType.Size = new Size(225, 60);
            this.gbType.Controls.Add(this.rbTemperature);
            this.gbType.Controls.Add(this.rbAlarm);

            // ========== btnQuery ==========
            this.btnQuery = new Button();
            this.btnQuery.Text = "查询";
            this.btnQuery.Location = new Point(12, 83);
            this.btnQuery.Size = new Size(100, 30);
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);

            // ========== btnExportCsv ==========
            this.btnExportCsv = new Button();
            this.btnExportCsv.Text = "导出 CSV";
            this.btnExportCsv.Location = new Point(122, 83);
            this.btnExportCsv.Size = new Size(100, 30);
            this.btnExportCsv.UseVisualStyleBackColor = true;
            this.btnExportCsv.Click += new System.EventHandler(this.btnExportCsv_Click);

            // ========== lblRecordCount ==========
            this.lblRecordCount = new Label();
            this.lblRecordCount.Text = "共 0 条记录";
            this.lblRecordCount.Location = new Point(235, 88);
            this.lblRecordCount.Size = new Size(200, 22);
            this.lblRecordCount.ForeColor = Color.Gray;

            // ========== dgvResult ==========
            this.dgvResult = new DataGridView();
            this.dgvResult.Location = new Point(12, 120);
            this.dgvResult.Size = new Size(783, 310);
            this.dgvResult.Anchor = AnchorStyles.Top | AnchorStyles.Left |
                                    AnchorStyles.Right | AnchorStyles.Bottom;
            this.dgvResult.AllowUserToAddRows = false;
            this.dgvResult.AllowUserToDeleteRows = false;
            this.dgvResult.ReadOnly = true;
            this.dgvResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvResult.BackgroundColor = SystemColors.Window;
            this.dgvResult.RowHeadersVisible = false;

            // ========== FrmQuery ==========
            this.AutoScaleDimensions = new SizeF(7F, 17F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(810, 445);
            this.Controls.Add(this.gbTimeRange);
            this.Controls.Add(this.gbType);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.btnExportCsv);
            this.Controls.Add(this.lblRecordCount);
            this.Controls.Add(this.dgvResult);
            this.Font = new Font("微软雅黑", 9F);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "历史数据查询";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
