using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Dapper;
using NLog;
using TemperatureMonitor.Models;

namespace TemperatureMonitor
{
  /// <summary>
  /// 温度记录仓库类。使用 Dapper 对 TemperatureLog 表进行数据库操作。
  /// </summary>
  public class TemperatureLogRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly string _connectionString;

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="connectionString">SQLite 数据库连接字符串</param>
        public TemperatureLogRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// 插入一条温度记录。
        /// </summary>
        /// <param name="log">温度记录对象（通道号、温度值、时间戳）</param>
        public void Insert(TemperatureLog log)
        {
            try
            {
                // 使用 Dapper 的 Execute 扩展方法执行 INSERT
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    const string sql = @"
                        INSERT INTO TemperatureLog (ChannelNo, Value, Timestamp)
                        VALUES (@ChannelNo, @Value, @Timestamp);";

                    connection.Execute(sql, new
                    {
                        log.ChannelNo,
                        log.Value,
                        log.Timestamp
                    });

                    Logger.Debug($"写入温度记录：通道={log.ChannelNo}，值={log.Value}");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "写入温度记录失败。");
                throw;
            }
        }

        /// <summary>
        /// 批量插入多条温度记录（事务保护）。
        /// </summary>
        /// <param name="logs">温度记录集合（每条包含通道号、温度值、时间戳）</param>
        public void BatchInsert(IEnumerable<TemperatureLog> logs)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        const string sql = @"
                            INSERT INTO TemperatureLog (ChannelNo, Value, Timestamp)
                            VALUES (@ChannelNo, @Value, @Timestamp);";

                        foreach (var log in logs)
                        {
                            connection.Execute(sql, new
                            {
                                log.ChannelNo,
                                log.Value,
                                log.Timestamp
                            }, transaction);
                        }

                        transaction.Commit();
                        Logger.Debug($"批量写入 {logs.Count()} 条温度记录成功。");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "批量写入温度记录失败。");
                throw;
            }
        }

        /// <summary>
        /// 获取所有温度记录，按时间倒序排列。
        /// </summary>
        /// <returns>温度记录集合</returns>
        public IEnumerable<TemperatureLog> GetAll()
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    const string sql = "SELECT * FROM TemperatureLog ORDER BY Timestamp DESC;";
                    return connection.Query<TemperatureLog>(sql);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "查询温度记录失败。");
                throw;
            }
        }

        /// <summary>
        /// 按通道号查询温度记录。
        /// </summary>
        /// <param name="channelNo">通道号</param>
        /// <returns>该通道的温度记录集合</returns>
        public IEnumerable<TemperatureLog> GetByChannel(int channelNo)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    const string sql = "SELECT * FROM TemperatureLog WHERE ChannelNo = @ChannelNo ORDER BY Timestamp DESC;";
                    return connection.Query<TemperatureLog>(sql, new { ChannelNo = channelNo });
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"按通道号 {channelNo} 查询温度记录失败。");
                throw;
            }
        }

        /// <summary>
        /// 按日期范围查询温度记录（升序排列）。
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns>指定时间范围内的温度记录集合</returns>
        public IEnumerable<TemperatureLog> GetByDateRange(DateTime start, DateTime end)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    const string sql = "SELECT * FROM TemperatureLog WHERE Timestamp >= @Start AND Timestamp <= @End ORDER BY Timestamp ASC;";
                    return connection.Query<TemperatureLog>(sql, new { Start = start, End = end });
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "按日期范围查询温度记录失败。");
                throw;
            }
        }
    }
}
