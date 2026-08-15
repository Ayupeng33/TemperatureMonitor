using System;

namespace TemperatureMonitor.Models
{
    /// <summary>
    /// 报警记录数据模型，对应数据库 AlarmLog 表。
    /// </summary>
    public class AlarmLog
    {
        /// <summary>主键ID，自增</summary>
        public int Id { get; set; }

        /// <summary>通道号，标识发生报警的传感器</summary>
        public int ChannelNo { get; set; }

        /// <summary>报警类型，例如"超温告警"、"低温告警"等</summary>
        public string AlarmType { get; set; }

        /// <summary>触发报警时的温度值</summary>
        public double Value { get; set; }

        /// <summary>报警发生时间</summary>
        public DateTime Timestamp { get; set; }
    }
}
