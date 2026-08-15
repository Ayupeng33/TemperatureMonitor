using Newtonsoft.Json;

namespace TemperatureMonitor
{
    /// <summary>
    /// 应用程序配置模型，对应 config.json 文件结构。
    /// </summary>
    public class AppConfig
    {
        /// <summary>PLC (Modbus TCP) IP 地址</summary>
        [JsonProperty("plc_ip_address")]
        public string PlcIpAddress { get; set; } = "127.0.0.1";

        /// <summary>PLC (Modbus TCP) 端口号</summary>
        [JsonProperty("plc_port")]
        public int PlcPort { get; set; } = 502;

        /// <summary>数据采集间隔（毫秒）</summary>
        [JsonProperty("sampling_interval_ms")]
        public int SamplingIntervalMs { get; set; } = 1000;

        /// <summary>报警温度上限（℃），超过此值触发超温报警</summary>
        [JsonProperty("alarm_upper_limit")]
        public double AlarmUpperLimit { get; set; } = 80.0;

        /// <summary>报警温度下限（℃），低于此值触发低温报警</summary>
        [JsonProperty("alarm_lower_limit")]
        public double AlarmLowerLimit { get; set; } = 0.0;
    }
}
