using System;
using System.Windows.Forms;
using NLog;

namespace TemperatureMonitor
{
    /// <summary>
    /// 应用程序入口类。
    /// </summary>
    internal static class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 应用程序主入口点。
        /// 启用视觉样式后启动 MainForm 主窗体，全局异常捕获防止程序静默崩溃。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // ---- 全局异常捕获 ----
            // 非 UI 线程（后台线程）未处理异常
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, "应用程序发生未处理异常。");
                MessageBox.Show("程序发生未知错误，已记录日志。", "程序错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>非 UI 线程（后台线程）未处理异常处理程序</summary>
        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Fatal(e.ExceptionObject as Exception, "非 UI 线程发生未处理异常。");
            MessageBox.Show("程序发生未知错误，已记录日志。", "程序错误",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
