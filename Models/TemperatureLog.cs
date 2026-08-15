using System;

namespace TemperatureMonitor.Models
{
    /// <summary>
    /// 温度记录数据模型，对应数据库 TemperatureLog 表。
    /// </summary>
    public class TemperatureLog
    {
        /// <summary>主键ID，自增</summary>
        public int Id { get; set; }

        /// <summary>通道号，标识第几路温度传感器</summary>
        public int ChannelNo { get; set; }

        /// <summary>温度值，单位：℃</summary>
        public double Value { get; set; }

        /// <summary>采集时间戳</summary>
        public DateTime Timestamp { get; set; }
    }
}
