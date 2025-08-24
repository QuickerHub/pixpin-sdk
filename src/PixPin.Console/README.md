# PixPin Console 测试程序

这是一个用于测试 PixPin .NET SDK 功能的控制台应用程序。它提供了一个交互式菜单界面，让您可以测试 PixPin SDK 的各种功能。

## 快速开始

### 方法一：使用批处理脚本（推荐）

1. 确保 PixPin 已安装在您的系统中
2. 在项目根目录运行：
   ```cmd
   run-test.cmd
   ```

### 方法二：手动构建和运行

1. 构建项目：
   ```cmd
   dotnet build src/PixPin.Console/PixPin.Console.csproj
   ```

2. 运行测试程序：
   ```cmd
   dotnet run --project src/PixPin.Console/PixPin.Console.csproj
   ```

## 测试功能

### 主菜单选项

```
========================================
              主菜单
========================================
1. 基础截图功能测试
2. 贴图功能测试
3. 扩展方法测试
4. 工作流示例测试
5. 系统功能测试
6. 批量操作测试
0. 退出程序
========================================
```

### 1. 基础截图功能测试

测试以下功能：
- ✅ 打开截图界面
- ✅ 截图并复制到剪贴板
- ✅ 直接截图指定区域
- ✅ 长截图
- ✅ GIF 录制

### 2. 贴图功能测试

测试以下功能：
- ✅ 从剪贴板贴图
- ✅ 检查贴图状态
- ✅ 切换贴图显示/隐藏
- ✅ 缩略图模式切换
- ✅ 保存所有贴图

### 3. 扩展方法测试

测试以下扩展功能：
- ✅ 截图整个屏幕
- ✅ 截图鼠标所在窗口并贴图
- ✅ 截图鼠标周围区域
- ✅ OCR 文字识别
- ✅ 切换贴图可见性

### 4. 工作流示例测试

测试以下实际工作流：
- ✅ 文档截图工作流
- ✅ Bug 报告工作流
- ✅ 设计评审工作流

### 5. 系统功能测试

测试以下系统集成功能：
- ✅ 启动外部程序（计算器）
- ✅ 快捷键控制
- ✅ 配置窗口
- ✅ 贴图组切换

### 6. 批量操作测试

测试以下批量功能：
- ✅ 批量截图
- ✅ 批量 GIF 录制
- ✅ 批量保存和关闭

## 前置要求

### 必需软件

1. **PixPin 软件** - 必须已安装
   - 下载地址：https://pixpin.cn/
   - 建议使用最新版本

2. **.NET 8.0 SDK** - 必须已安装
   - 下载地址：https://dotnet.microsoft.com/download

### PixPin 自动检测

程序使用智能进程检测技术：
- ✅ **自动检测**: 自动查找正在运行的 PixPin 进程并获取可执行文件路径
- ✅ **无需配置**: 不需要手动指定 PixPin 安装路径
- ✅ **智能启动**: 如果 PixPin 未运行，会自动尝试启动
- ✅ **路径获取**: 直接从进程信息获取准确的可执行文件路径

这种方式比传统的路径枚举更可靠，无论 PixPin 安装在何处都能正确检测。

## 测试配置

可以通过修改 `TestConfiguration.cs` 文件来自定义测试参数：

```csharp
public class TestConfiguration
{
    // PixPin 可执行文件路径
    public string[] PixPinPaths { get; set; } = { ... };
    
    // 测试操作间延迟时间（毫秒）
    public int TestDelayMs { get; set; } = 1000;
    
    // PixPin 启动超时时间（毫秒）
    public int StartupTimeoutMs { get; set; } = 5000;
    
    // 临时文件目录
    public string TempDirectory { get; set; } = ...;
    
    // 是否自动清理测试文件
    public bool AutoCleanup { get; set; } = true;
    
    // 截图坐标和尺寸
    public TestCoordinates Coordinates { get; set; } = new();
    
    // 是否显示详细输出
    public bool VerboseOutput { get; set; } = true;
}
```

## 使用说明

### 启动流程

1. 程序启动时会自动尝试初始化 PixPin 客户端
2. 检测 PixPin 是否正在运行
3. 如果未运行，会尝试启动 PixPin
4. 显示主菜单供选择测试功能

### 测试建议

1. **首次使用**：建议先运行 "基础截图功能测试" 确保 SDK 正常工作
2. **功能验证**：每个菜单选项都会发送相应的命令到 PixPin，观察 PixPin 的响应
3. **错误排查**：如果出现错误，检查 PixPin 是否正在运行，路径是否正确

### 预期行为

- ✅ **成功情况**：命令发送后 PixPin 会响应对应操作（打开界面、截图等）
- ❌ **失败情况**：程序会显示错误信息，通常是 PixPin 未运行或路径不正确

## 故障排除

### 常见问题

1. **"无法找到 PixPin 可执行文件"**
   - 确保 PixPin 已正确安装
   - 检查安装路径是否在配置的路径列表中
   - 尝试将 PixPin 添加到系统 PATH

2. **"PixPin 脚本执行失败"**
   - 确保 PixPin 正在运行
   - 检查 PixPin 版本是否支持脚本功能
   - 尝试手动启动 PixPin

3. **权限问题**
   - 以管理员身份运行测试程序
   - 确保对临时目录有写入权限

### 调试模式

如果需要查看详细的脚本执行信息，可以：

1. 修改 `TestConfiguration.cs` 中的 `VerboseOutput = true`
2. 或者直接在代码中设置断点调试

## 输出文件

测试过程中会在以下位置生成文件：

- 临时截图：`%TEMP%\PixPinTest\`
- 批量测试：`%TEMP%\PixPinBatchTest\`
- 文档工作流：`%TEMP%\Documentation\Screenshots\`
- 设计评审：`%TEMP%\DesignReview\Selected\`

## 扩展开发

如果需要添加新的测试功能：

1. 在 `Program.cs` 中添加新的测试方法
2. 在主菜单中添加对应选项
3. 更新此文档说明新功能

## 技术支持

如果遇到问题，请检查：

1. PixPin 官方文档：https://pixpin.cn/docs/
2. PixPin 脚本文档：https://pixpin.cn/docs/configuration/script.html
3. 项目 GitHub Issues（如果有的话）

---

🎯 **提示**：第一次运行建议选择 "基础截图功能测试" 来验证 SDK 是否正常工作！
