using System;
using System.IO;
using Newtonsoft.Json;
using NLog;

namespace TemperatureMonitor
{
    /// <summary>
    /// 配置管理器。负责从 config.json 加载配置，
    /// 提供全局共享的 AppConfig 实例，并支持保存回文件。
    /// </summary>
    public class ConfigManager
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly string _filePath;

        /// <summary>当前内存中的配置（单例）</summary>
        public AppConfig Config { get; private set; }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="filePath">config.json 完整路径；省略时默认为程序目录下的 config.json</param>
        public ConfigManager(string filePath = null)
        {
            _filePath = filePath ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            Config = new AppConfig();
        }

        /// <summary>
        /// 从 JSON 文件加载配置。若文件不存在则使用默认配置并自动创建文件。
        /// </summary>
        public void Load()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);
                    Config = JsonConvert.DeserializeObject<AppConfig>(json) ?? new AppConfig();
                    Logger.Info($"配置已加载：{_filePath}");
                }
                else
                {
                    Logger.Warn($"配置文件不存在 {_filePath}，使用默认配置。");
                    Save(); // 生成默认配置文件
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"加载配置文件失败 {_filePath}，使用默认配置。");
                Config = new AppConfig();
            }
        }

        /// <summary>
        /// 将当前内存中的配置保存到 JSON 文件。
        /// </summary>
        public void Save()
        {
            try
            {
                string json = JsonConvert.SerializeObject(Config, Formatting.Indented);
                File.WriteAllText(_filePath, json);
                Logger.Info($"配置已保存：{_filePath}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"保存配置文件失败 {_filePath}。");
                throw;
            }
        }
    }
}
