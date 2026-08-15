using System;
using System.Data.SQLite;
using NLog;

namespace TemperatureMonitor
{
    /// <summary>
    /// 数据库初始化器。负责首次运行时自动创建 SQLite 数据库表。
    /// </summary>
    public class DatabaseInitializer
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly string _connectionString;

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="connectionString">SQLite 数据库连接字符串</param>
        public DatabaseInitializer(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// 执行初始化：创建 TemperatureLog 和 AlarmLog 两张表（如果不存在）。
        /// </summary>
        public void Initialize()
        {
            try
            {
                // 打开数据库连接，using 确保连接自动释放
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    // 建表 SQL —— 温度记录表
                    string createTemperatureLogTable = @"
                        CREATE TABLE IF NOT EXISTS TemperatureLog (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,  -- 自增主键
                            ChannelNo INT NOT NULL,                -- 通道号
                            Value REAL NOT NULL,                   -- 温度值
                            Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP  -- 采集时间，默认当前时间
                        );";

                    // 建表 SQL —— 报警记录表
                    string createAlarmLogTable = @"
                        CREATE TABLE IF NOT EXISTS AlarmLog (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,  -- 自增主键
                            ChannelNo INT NOT NULL,                -- 通道号
                            AlarmType TEXT NOT NULL,               -- 报警类型
                            Value REAL NOT NULL,                   -- 触发报警的温度值
                            Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP  -- 报警时间，默认当前时间
                        );";

                    // 执行建表语句
                    using (var cmd = new SQLiteCommand(createTemperatureLogTable, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd = new SQLiteCommand(createAlarmLogTable, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    logger.Info("数据库表初始化成功。");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "数据库表初始化失败。");
                throw; // 初始化失败直接向上抛，阻止程序继续运行
            }
        }
    }
}
