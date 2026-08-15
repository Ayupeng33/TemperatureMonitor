using System;
using System.Windows.Forms;

namespace TemperatureMonitor
{
    /// <summary>
    /// 系统设置窗体。可配置 PLC IP、端口、采样间隔和报警上下限。
    /// </summary>
    public partial class FrmConfig : Form
    {
        private readonly ConfigManager _configManager;

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="configManager">全局配置管理器，用于读取和持久化设置</param>
        public FrmConfig(ConfigManager configManager)
        {
            InitializeComponent();
            _configManager = configManager;

            // 加载当前配置到控件
            LoadConfigToUI();
        }

        /// <summary>将配置模型的值填充到界面上</summary>
        private void LoadConfigToUI()
        {
            var cfg = _configManager.Config;
            txtIpAddress.Text = cfg.PlcIpAddress;
            nudPort.Value = cfg.PlcPort;
            nudSamplingInterval.Value = cfg.SamplingIntervalMs;
            nudAlarmUpper.Value = (decimal)cfg.AlarmUpperLimit;
            nudAlarmLower.Value = (decimal)cfg.AlarmLowerLimit;
        }

        /// <summary>将界面上各控件的值写回配置模型</summary>
        private void SaveUIToConfig()
        {
            var cfg = _configManager.Config;
            cfg.PlcIpAddress = txtIpAddress.Text.Trim();
            cfg.PlcPort = (int)nudPort.Value;
            cfg.SamplingIntervalMs = (int)nudSamplingInterval.Value;
            cfg.AlarmUpperLimit = (double)nudAlarmUpper.Value;
            cfg.AlarmLowerLimit = (double)nudAlarmLower.Value;
        }

        // ---- 按钮事件 ----

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 基本校验
            if (string.IsNullOrWhiteSpace(txtIpAddress.Text))
            {
                MessageBox.Show("请输入有效的 IP 地址。", "输入错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIpAddress.Focus();
                return;
            }

            if (nudAlarmLower.Value >= nudAlarmUpper.Value)
            {
                MessageBox.Show("报警下限必须小于上限。", "输入错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudAlarmLower.Focus();
                return;
            }

            // 保存到内存并持久化到文件
            SaveUIToConfig();
            _configManager.Save();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
