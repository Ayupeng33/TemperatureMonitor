using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TemperatureMonitor.Models;

namespace TemperatureMonitor
{
    /// <summary>
    /// 历史数据查询窗体。支持按时间段查询温度记录和报警记录，
    /// 结果在 DataGridView 中显示，并可导出为 CSV 文件。
    /// </summary>
    public partial class FrmQuery : Form
    {
        private readonly TemperatureLogRepository _temperatureRepo;
        private readonly AlarmLogRepository _alarmRepo;

        /// <summary>当前查询结果的 DataTable，导出 CSV 时使用</summary>
        private DataTable _currentTable;

        public FrmQuery(TemperatureLogRepository temperatureRepo, AlarmLogRepository alarmRepo)
        {
            InitializeComponent();

            _temperatureRepo = temperatureRepo;
            _alarmRepo = alarmRepo;

            // 默认时间段：最近 1 小时
            dtpStart.Value = DateTime.Now.AddHours(-1);
            dtpEnd.Value = DateTime.Now;

            // 默认选中温度记录
            rbTemperature.Checked = true;
        }

        // ===================================================================
        //  查询
        // ===================================================================

        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime start = dtpStart.Value;
                DateTime end = dtpEnd.Value;

                if (start >= end)
                {
                    MessageBox.Show("开始时间必须小于结束时间。", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (rbTemperature.Checked)
                    QueryTemperatureLogs(start, end);
                else
                    QueryAlarmLogs(start, end);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"查询失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>查询温度记录并绑定到 DataGridView</summary>
        private void QueryTemperatureLogs(DateTime start, DateTime end)
        {
            var list = _temperatureRepo.GetByDateRange(start, end);

            _currentTable = new DataTable("温度记录");
            _currentTable.Columns.Add("编号", typeof(int));
            _currentTable.Columns.Add("通道号", typeof(int));
            _currentTable.Columns.Add("温度值(℃)", typeof(double));
            _currentTable.Columns.Add("采集时间", typeof(DateTime));

            foreach (var item in list)
            {
                _currentTable.Rows.Add(item.Id, item.ChannelNo, item.Value, item.Timestamp);
            }

            dgvResult.DataSource = _currentTable;
            lblRecordCount.Text = $"共 {list.Count()} 条记录";
        }

        /// <summary>查询报警记录并绑定到 DataGridView</summary>
        private void QueryAlarmLogs(DateTime start, DateTime end)
        {
            var list = _alarmRepo.GetByDateRange(start, end);

            _currentTable = new DataTable("报警记录");
            _currentTable.Columns.Add("编号", typeof(int));
            _currentTable.Columns.Add("通道号", typeof(int));
            _currentTable.Columns.Add("报警类型", typeof(string));
            _currentTable.Columns.Add("温度值(℃)", typeof(double));
            _currentTable.Columns.Add("发生时间", typeof(DateTime));

            foreach (var item in list)
            {
                _currentTable.Rows.Add(item.Id, item.ChannelNo, item.AlarmType, item.Value, item.Timestamp);
            }

            dgvResult.DataSource = _currentTable;
            lblRecordCount.Text = $"共 {list.Count()} 条记录";
        }

        // ===================================================================
        //  导出 CSV
        // ===================================================================

        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            if (_currentTable == null || _currentTable.Rows.Count == 0)
            {
                MessageBox.Show("没有数据可导出，请先执行查询。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV 文件 (*.csv)|*.csv|所有文件 (*.*)|*.*";
                sfd.FileName = $"{_currentTable.TableName}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                sfd.DefaultExt = "csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ExportToCsv(sfd.FileName);
                }
            }
        }

        /// <summary>
        /// 将 DataTable 写入 CSV 文件。
        /// 对每个字段做双引号包裹，内部双引号转义，以正确处理中文逗号、换行等特殊字符。
        /// </summary>
        private void ExportToCsv(string filePath)
        {
            try
            {
                using (var sw = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    // 写入表头
                    var headerCols = new List<string>();
                    foreach (DataColumn col in _currentTable.Columns)
                        headerCols.Add(EscapeCsvField(col.ColumnName));
                    sw.WriteLine(string.Join(",", headerCols));

                    // 写入数据行
                    foreach (DataRow row in _currentTable.Rows)
                    {
                        var fields = new List<string>();
                        foreach (DataColumn col in _currentTable.Columns)
                        {
                            object val = row[col];
                            string text = val == null || val == DBNull.Value ? "" : val.ToString();
                            fields.Add(EscapeCsvField(text));
                        }
                        sw.WriteLine(string.Join(",", fields));
                    }
                }

                MessageBox.Show($"导出成功：{filePath}", "完成",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 对 CSV 字段进行转义处理：
        /// - 如果包含逗号（包括中文逗号）、双引号、换行符，则用双引号包裹
        /// - 字段内部的双引号用两个双引号转义
        /// </summary>
        private static string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
                return "";

            // 只要包含逗号（含中文逗号）、双引号、换行符，就需要包裹
            if (field.Contains(",") || field.Contains("，") ||
                field.Contains("\"") || field.Contains("\n") ||
                field.Contains("\r"))
            {
                // 双引号转义："" 
                field = field.Replace("\"", "\"\"");
                return $"\"{field}\"";
            }

            return field;
        }
    }
}
