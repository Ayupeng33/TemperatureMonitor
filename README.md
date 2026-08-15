# TemperatureMonitor 温度监控上位机系统

> 基于 C# / WinForms / .NET Framework 4.8 的工业设备上位机软件，面向半导体 / 锂电行业中小设备厂的应用场景。

## 项目简介

TemperatureMonitor 是一套完整的设备上位机监控软件，实现了**数据采集 → 实时显示 → 持久化存储 → 历史查询 → 报警联动**的完整业务闭环。系统通过 **Modbus TCP** 协议采集 PLC / 温控器等设备的多通道温度数据，并集成 **SECS/GEM（HSMS）** 通讯模块用于半导体设备间通信，可作为设备上位机软件的参考实现。

**项目特点：**

- 完整工程结构：UI 层 / 业务层 / 数据层分离，接口清晰
- 多线程异步采集，界面不卡顿，防重入保护
- 断线自动重连机制（最多 3 次，间隔 2 秒）
- 双协议通讯：Modbus TCP（采集）+ SECS/GEM HSMS（半导体标准）
- 数据持久化：SQLite + Dapper，支持按时间段查询与 CSV 导出
- 可配置化：IP / 端口 / 采样间隔 / 报警阈值全部 JSON 配置
- 完整的 MSI 安装包部署方案

---

## 技术栈

| 分类 | 技术 | 说明 |
|------|------|------|
| 语言 / 框架 | C# / WinForms / .NET Framework 4.8 | 上位机主流组合 |
| 通讯协议 | Modbus TCP（NModbus4） | 温度数据采集 |
| 通讯协议 | SECS/GEM（Secs4Net） | 半导体设备通信 |
| 数据库 | SQLite（System.Data.SQLite） | 本地嵌入式存储 |
| ORM | Dapper | 轻量高性能数据访问 |
| 配置 | Newtonsoft.Json | JSON 配置文件读写 |
| 日志 | NLog | 文件 + 控制台双输出 |
| 图表 | System.Windows.Forms.DataVisualization.Chart | 实时曲线 |
| 打包 | Visual Studio Installer Projects | MSI 安装包 + 桌面快捷方式 |

---

## 功能清单

### 实时监控
- 4 通道温度实时采集与显示（Modbus TCP 保持寄存器）
- 实时曲线图（Chart 控件，Spline 曲线，滚动窗口 300 点 / 5 分钟）
- 每通道独立报警指示灯（超上限 / 低下限），报警闪烁 + 系统提示音
- 整体报警状态灯 + 状态栏（已连接 / 通信中断 / 未连接）

### 数据采集引擎
- `System.Windows.Forms.Timer` 定时触发（间隔可配，默认 1000ms）
- `async/await` + `Task.Run` 后台线程执行阻塞 IO，**UI 不卡顿**
- 布尔标志位防重入，避免采集任务堆积
- 读取失败自动重连（最多 3 次，间隔 2 秒），状态栏实时提示"通信中断"
- 每 5 秒批量写入温度日志（事务保护）

### 报警联动
- 温度超上限 / 低于下限触发报警
- 报警跳变检测（正常→报警）时：播放系统提示音 + 写入报警表 + 指示灯变红
- 报警恢复自动复位指示灯
- 报警闪烁 Timer 实现视觉强调

### 数据持久化（SQLite + Dapper）
- `TemperatureLog` 表：Id / ChannelNo / Value / Timestamp
- `AlarmLog` 表：Id / ChannelNo / AlarmType / Value / Timestamp
- 首次运行自动建表（`DatabaseInitializer`）
- 仓储模式（Repository）封装插入 / 查询，方法：
  - `Insert` / `BatchInsert`
  - `GetAll` / `GetByChannel` / `GetByDateRange`

### 历史查询
- 按时间段查询温度 / 报警记录（DataGridView 展示）
- 支持导出 CSV（UTF-8 编码，中文逗号 / 引号 / 换行自动转义，Excel 直接打开不乱码）

### SECS/GEM 通讯测试（半导体专用）
- 基于 Secs4Net 2.x（HSMS + SecsGem）
- 连接参数（IP / 端口）可配置，支持主动连接（Host 模式）
- **发送 S1F1（Are You There）**：发送后接收 S1F2 回复并显示报文内容
- **启动事件报告**：模拟设备端主动上报 S6F11 事件（DATAID / CEID / RPTID / 温度湿度数据）
- 后台异步接收设备主动消息，自动应答避免 T3 超时
- 报文以结构化格式（List / U4 / F4 / ASCII 等）递归显示在日志框
- Secs4Net 内部日志接入 NLog

### 系统配置
- 配置窗体（FrmConfig）：PLC IP / 端口 / 采样间隔 / 报警上下限
- JSON 配置文件（config.json），启动加载、保存时更新内存
- 配置即时生效：修改后下次采集使用新参数

### 打包部署
- MSI 安装包（Visual Studio Installer Projects）
- 包含主程序 + 依赖 DLL + SQLite.Interop.dll（x86 / x64）+ NLog.config + config.json + 初始数据库
- 桌面快捷方式，安装后开箱即用

---

## 架构设计

```
┌────────────────────────────────────────────────────┐
│                     UI 层 (WinForms)               │
│  MainForm │ FrmConfig │ FrmQuery │ FrmSecsTest     │
└──────────────┬─────────────────────────────────────┘
               │ 调用
┌──────────────▼─────────────────────────────────────┐
│                  业务层 / 采集引擎                   │
│  ModbusClient（重连/日志）  SecsGem（SECS/GEM）      │
│  ConfigManager（配置）      Timer + async/await     │
└──────────────┬─────────────────────────────────────┘
               │ 调用
┌──────────────▼─────────────────────────────────────┐
│                  数据层 (Dapper)                    │
│  TemperatureLogRepository  AlarmLogRepository       │
│  DatabaseInitializer（建表）                        │
└──────────────┬─────────────────────────────────────┘
               │
┌──────────────▼─────────────────────────────────────┐
│                SQLite 数据库                        │
│  TemperatureLog / AlarmLog                          │
└────────────────────────────────────────────────────┘
```

**关键设计点：**

1. **UI 与业务解耦**：窗体只负责展示与用户交互，采集逻辑封装在 `ModbusClient`，数据库操作封装在 Repository
2. **异步不阻塞 UI**：`await Task.Run(() => _modbusClient.ReadRegisters())` 将阻塞 IO 放到线程池，`await` 回到 UI 线程更新控件，天然线程安全
3. **防重入**：`_isReading` 标志位防止采集任务在上一次未完成时重叠执行
4. **配置驱动**：所有可调参数（IP/端口/间隔/阈值）集中在一个 `AppConfig`，运行时热更新
5. **重连状态机**：读取失败 → 等待 2 秒 → 重连 → 重试（最多 3 次）→ 最终失败抛异常 → 状态栏提示

---

## 核心代码速览

### Modbus 采集（ModbusClient.cs）

```csharp
public float[] ReadRegisters()
{
    int attempt = 0;
    while (attempt <= MaxRetryCount)   // 最多重试 3 次
    {
        try
        {
            if (!IsConnected || _master == null) Connect();   // 断线自动重连
            ushort[] registers = _master.ReadHoldingRegisters(0, 8);  // 读 8 个保持寄存器
            // 两两组合为 4 个 float（Modbus 大端序）
            float[] values = new float[4];
            for (int i = 0; i < 4; i++)
                values[i] = ModbusUtility.GetSingle(registers[i * 2], registers[i * 2 + 1]);
            return values;
        }
        catch (Exception ex)
        {
            if (attempt < MaxRetryCount)
            {
                attempt++;
                Thread.Sleep(RetryIntervalMs);   // 2 秒
                try { Connect(); } catch { /* 重连失败继续循环 */ }
            }
            else throw;   // 重试耗尽向上抛
        }
    }
    throw new InvalidOperationException("读取失败");
}
```

### 异步采集不卡 UI（MainForm.cs）

```csharp
private async void timerDataCollection_Tick(object sender, EventArgs e)
{
    if (_isReading) return;   // 防重入
    _isReading = true;
    try
    {
        float[] temps = await Task.Run(() => _modbusClient.ReadRegisters());  // 后台读，UI 不卡
        UpdateTemperatureDisplay(temps);   // 回到 UI 线程更新
        UpdateChart(temps);
        UpdateAlarmIndicator(temps);
    }
    catch { tsslConnectionStatus.Text = "通信中断"; }
    finally { _isReading = false; }
}
```

### SECS/GEM 发送 S1F1（FrmSecsTest.cs）

```csharp
using var s1f1 = new SecsMessage(1, 1) { SecsItem = Item.L() };   // S1F1 Are You There
var reply = await _secsGem.SendAsync(s1f1, _cts.Token);           // 等待 S1F2
// reply 即为对方返回的 S1F2，含 MDLN/MDST 等数据项
```

### Dapper 仓储（TemperatureLogRepository.cs）

```csharp
public void Insert(TemperatureLog log)
{
    using var connection = new SQLiteConnection(_connectionString);
    const string sql = @"INSERT INTO TemperatureLog (ChannelNo, Value, Timestamp)
                         VALUES (@ChannelNo, @Value, @Timestamp);";
    connection.Execute(sql, new { log.ChannelNo, log.Value, log.Timestamp });
}
```

---

## 运行与演示（面试前必看）

### 环境要求
- Windows 10 / 11（150% 缩放已适配，PerMonitorV2 DPI 感知）
- .NET Framework 4.8（Win10/11 自带或自动安装）

### 方式一：直接运行（无需模拟器，可看 UI）
```
运行 bin\Debug\TemperatureMonitor.exe
```
- 可看到主界面（菜单栏 / 温度显示 / 曲线 / 报警灯 / 状态栏）
- 点"连接"会尝试连 127.0.0.1:502，无设备则状态栏显示"通信中断"（正好演示断线处理）

### 方式二：完整演示（Modbus 从站模拟器 + SECS/GEM 模拟器）

**准备工具：**
- Modbus 从站模拟器：**Modbus Slave**（Witte Software，试用版即可）或 **ModRSsim2**（免费）
- SECS/GEM 模拟器：**Secs4Net 官方 Simulator** 或 **SECS/GEM Simulator 类工具**

**Modbus 采集演示：**
1. 打开 Modbus Slave，新建从站，Slave ID=1，功能码 03（保持寄存器）
2. 起始地址 0，寄存器数量 8（4 通道温度，每通道 2 寄存器 float 大端）
3. 手动输入 4 组温度值（如 25.5 / 26.3 / 24.8 / 25.1）
4. 运行本程序 → 点"连接"（默认 127.0.0.1:502）→ 观察 4 通道温度和曲线实时更新
5. 把一个通道值改成 120（超上限 80）→ 观察报警灯变红 + 闪烁 + 提示音
6. 关闭模拟器 → 观察状态栏变"通信中断"，重启模拟器 → 观察自动重连恢复

**SECS/GEM 演示：**
1. 打开 SECS/GEM 模拟器，作为被动端（被动模式）监听端口
2. 本程序菜单 → "SECS/GEM 通讯测试" → 填 IP/端口 → 点"连接"
3. 点"发送 S1F1" → 模拟器回 S1F2，日志框显示回复报文
4. 点"启动事件报告" → 发送 S6F11 事件，模拟器收到显示
5. 演示 HSMS 握手：连接建立后观察状态"已连接（会话已建立）"

---

## 打包部署

1. 在 Visual Studio 中打开 `TemperatureMonitor.sln`
2. 配置切换 **Release | x86**
3. 右键 `TemperatureMonitor` → 重新生成
4. 右键 `TemperatureMonitorSetup` → **生成**（必须在 VS GUI 中构建）
5. 输出 MSI：`TemperatureMonitorSetup\Release\TemperatureMonitorSetup.msi`
6. 安装后桌面生成"Temperature Monitor"快捷方式，程序自动建库、写日志

> 注意：SQLite 依赖 `SQLite.Interop.dll`，安装包已包含 `x86\` 与 `x64\` 子目录版本，避免 DllNotFoundException。

---

## 项目文件结构

```
TemperatureMonitor/
├── TemperatureMonitor.sln          # 解决方案
├── TemperatureMonitor.csproj       # 项目文件
├── app.manifest                    # DPI 感知（PerMonitorV2）
├── App.config                      # NLog 等配置
├── Program.cs                      # 入口 + 全局异常处理
├── MainForm.cs / .Designer.cs      # 主窗体（监控主界面）
├── FrmConfig.cs / .Designer.cs     # 系统设置窗体
├── FrmQuery.cs / .Designer.cs      # 历史查询窗体
├── FrmSecsTest.cs / .Designer.cs   # SECS/GEM 通讯测试窗体
├── ModbusClient.cs                 # Modbus TCP 采集 + 自动重连
├── AppConfig.cs                    # 配置模型
├── ConfigManager.cs                # JSON 配置读写
├── DatabaseInitializer.cs          # 建表
├── TemperatureLogRepository.cs     # 温度仓储（Dapper）
├── AlarmLogRepository.cs           # 报警仓储（Dapper）
├── Models/
│   ├── TemperatureLog.cs
│   └── AlarmLog.cs
└── setup_files/                    # 打包辅助文件
```

---

## 技术亮点（面试可主动展开）

1. **多线程 + 异步**：WinForms UI 线程模型、async/await 上下文捕获、Task.Run 的正确使用、防重入
2. **工业通讯**：Modbus 协议帧结构、保持寄存器与 float 转换、大端序；SECS/GEM 的 HSMS 连接与消息收发
3. **数据库设计**：SQLite 嵌入式、Dapper 参数化防注入、仓储模式、事务批量写入
4. **健壮性**：断线重连、异常日志、全局异常捕获（Program.cs）、配置热更新
5. **工程化**：MSI 打包、NuGet 依赖管理、NLog 日志归档（按天、保留 7 份）
6. **高 DPI 适配**：PerMonitorV2 + AutoScaleMode.Font，适配 Win11 150% 缩放

---

## 后续可扩展方向（面试谈发展空间）

- 配方管理（Recipe）：温度曲线配方编辑 / 下发 / 执行
- 用户权限：操作员 / 工程师 / 管理员三级登录
- 报表：Excel 班次汇总报表导出
- 更多协议：串口 RS232/RS485、西门子 S7、三菱 MC 协议
- 实时数据库 + Web 看板 / 远程监控
