using System;
using System.Net.Sockets;
using System.Threading;
using Modbus.Device;
using Modbus.Utility;
using NLog;

namespace TemperatureMonitor
{
  /// <summary>
  /// Modbus TCP 客户端，封装 NModbus4 库。
  /// 提供连接、断开、读取保持寄存器（4 路浮点值）功能，并内置自动重连机制。
  /// </summary>
  public class ModbusClient
  {
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    // 重连参数
    private const int MaxRetryCount = 3;//最大重试次数
    private const int RetryIntervalMs = 2000;//重试间隔毫秒数

    // 要读取的保持寄存器起始地址和数量（4 个 float = 8 个寄存器）
    private const ushort RegisterStartAddress = 0;//寄存器起始地址
    private const ushort RegisterCount = 8;//寄存器数量

    private TcpClient _tcpClient;//底层TCP网络连接 
    private ModbusIpMaster _master;//Modbus TCP 主站 

    /// <summary>Modbus TCP 服务器 IP 地址</summary>
    public string IpAddress { get; set; } = "127.0.0.1";

    /// <summary>Modbus TCP 服务器端口号</summary>
    public int Port { get; set; } = 502;

    /// <summary>当前是否已连接</summary>
    public bool IsConnected => _tcpClient?.Connected ?? false;

    /// <summary>
    /// 连接到 Modbus TCP 服务器。
    /// </summary>
    public void Connect()
    {
      Disconnect(); // 确保先断开旧连接

      Logger.Info($"正在连接 Modbus 服务器 {IpAddress}:{Port}...");
      _tcpClient = new TcpClient();
      _tcpClient.Connect(IpAddress, Port);
      _master = ModbusIpMaster.CreateIp(_tcpClient);
      Logger.Info($"Modbus 服务器 {IpAddress}:{Port} 连接成功。");
    }

    /// <summary>
    /// 断开与 Modbus TCP 服务器的连接，释放资源。
    /// </summary>
    public void Disconnect()
    {
      if (_master != null)
      {
        _master.Dispose();
        _master = null;
      }

      if (_tcpClient != null)
      {
        if (_tcpClient.Connected)
          _tcpClient.Close();

        _tcpClient.Dispose();
        _tcpClient = null;
      }

      Logger.Info("Modbus 连接已断开。");
    }

    /// <summary>
    /// 从保持寄存器中读取 4 个浮点值。
    /// 内部自动将 8 个 16 位寄存器两两拼合为 float。
    /// 读取失败时自动尝试重连重试（最多 3 次，间隔 2 秒）。
    /// </summary>
    /// <returns>包含 4 个浮点值的数组，分别对应 4 个通道</returns>
    public float[] ReadRegisters()
    {
      int attempt = 0;

      while (attempt <= MaxRetryCount)
      {
        try
        {
          if (attempt > 0)
            Logger.Warn($"第 {attempt}/{MaxRetryCount} 次重试读取...");

          // 确保连接有效
          if (!IsConnected || _master == null)
            Connect();

          // 读取 8 个保持寄存器
          ushort[] registers = _master.ReadHoldingRegisters(RegisterStartAddress, RegisterCount);

          // 将 8 个 ushort 两两组合为 4 个 float（Modbus 大端序）
          float[] values = new float[4];
          for (int i = 0; i < 4; i++)
          {
            // Modbus 大端：高字在前，低字在后
            ushort high = registers[i * 2];
            ushort low = registers[i * 2 + 1];
            values[i] = ModbusUtility.GetSingle(high, low);
          }

          Logger.Debug($"读取保持寄存器成功：V0={values[0]:F3}  V1={values[1]:F3}  V2={values[2]:F3}  V3={values[3]:F3}");
          return values;
        }
        catch (Exception ex)
        {
          Logger.Warn(ex, $"读取保持寄存器失败（第 {attempt + 1} 次）。");

          if (attempt < MaxRetryCount)
          {
            attempt++;
            Logger.Info($"{RetryIntervalMs / 1000} 秒后尝试重连...");
            Thread.Sleep(RetryIntervalMs);

            // 尝试重新连接
            try
            {
              Connect();
            }
            catch (Exception connEx)
            {
              Logger.Error(connEx, "重连失败。");
            }
          }
          else
          {
            Logger.Error(ex, $"已达最大重试次数（{MaxRetryCount} 次），读取保持寄存器最终失败。");
            throw; // 重试耗尽，向上抛异常
          }
        }
      }

      // 不应到达此处
      throw new InvalidOperationException("读取保持寄存器时发生未知错误。");
    }
  }
}
