using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Dapper;
using NLog;
using TemperatureMonitor.Models;

namespace TemperatureMonitor
{
    /// <summary>
    /// 报警记录仓库类。使用 Dapper 对 AlarmLog 表进行数据库操作。
    /// </summary>
    public class AlarmLogRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly string _connectionString;

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="connectionString">SQLite 数据库连接字符串</param>
        public AlarmLogRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// 插入一条报警记录。
        /// </summary>
        /// <param name="log">报警记录对象（通道号、报警类型、温度值、时间戳）</param>
        public void Insert(AlarmLog log)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    const string sql = @"
                        INSERT INTO AlarmLog (ChannelNo, AlarmType, Value, Timestamp)
                        VALUES (@ChannelNo, @AlarmType, @Value, @Timestamp);";

                    connection.Execute(sql, new
                    {
                        log.ChannelNo,
                        log.AlarmType,
                        log.Value,
                        log.Timestamp
                    });

                    Logger.Debug($"写入报警记录：通道={log.ChannelNo}，类型={log.AlarmType}，值={log.Value}");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "写入报警记录失败。");
                throw;
            }
        }

        /// <summary>
        /// 获取所有报警记录，按时间倒序排列。
        /// </summary>
        /// <returns>报警记录集合</returns>
        public IEnumerable<AlarmLog> GetAll()
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    const string sql = "SELECT * FROM AlarmLog ORDER BY Timestamp DESC;";
                    return connection.Query<AlarmLog>(sql);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "查询报警记录失败。");
                throw;
            }
        }

        /// <summary>
        /// 按通道号查询报警记录。
        /// </summary>
        /// <param name="channelNo">通道号</param>
        /// <returns>该通道的报警记录集合</returns>
        public IEnumerable<AlarmLog> GetByChannel(int channelNo)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    const string sql = "SELECT * FROM AlarmLog WHERE ChannelNo = @ChannelNo ORDER BY Timestamp DESC;";
                    return connection.Query<AlarmLog>(sql, new { ChannelNo = channelNo });
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"按通道号 {channelNo} 查询报警记录失败。");
                throw;
            }
        }

        /// <summary>
        /// 按日期范围查询报警记录（升序排列）。
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns>指定时间范围内的报警记录集合</returns>
        public IEnumerable<AlarmLog> GetByDateRange(DateTime start, DateTime end)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    const string sql = "SELECT * FROM AlarmLog WHERE Timestamp >= @Start AND Timestamp <= @End ORDER BY Timestamp ASC;";
                    return connection.Query<AlarmLog>(sql, new { Start = start, End = end });
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "按日期范围查询报警记录失败。");
                throw;
            }
        }
    }
}
