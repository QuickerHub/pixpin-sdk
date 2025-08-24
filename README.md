# PixPin .NET SDK

这是一个为 PixPin 截图工具开发的 .NET SDK，它将 PixPin 的 JavaScript API 转换为 .NET 方法调用，使您可以通过 .NET 代码自动化 PixPin 的各种功能。

## 功能特性

- 🖼️ **截图功能**: 支持静态截图、长截图、GIF 录制
- 📌 **贴图管理**: 贴图创建、保存、显示/隐藏控制
- 🔧 **系统集成**: 命令行执行、快捷键控制
- 🎯 **精确定位**: 鼠标位置、窗口区域、特定坐标截图
- 📝 **OCR 识别**: 截图文字识别和转换
- 🚀 **扩展方法**: 提供便捷的批量操作和工作流

## 快速开始

### 1. 基本使用

```csharp
using PixPin.Core;
using PixPin.Core.Enums;

// 创建 PixPin 客户端（自动检测进程）
var pixpin = new PixPinClient();

// 检查 PixPin 是否可用
if (pixpin.IsAvailable)
{
    // 截图并复制到剪贴板
    pixpin.ScreenShot(ShotAction.Copy);

    // 直接截图指定区域
    var rect = pixpin.GenRect(100, 100, 500, 300);
    pixpin.DirectScreenShot(rect, ShotAction.Pin);
}
else
{
    // 确保 PixPin 正在运行
    pixpin.EnsurePixPinRunning();
}
```

### 2. 扩展方法使用

```csharp
using PixPin.Core.Extensions;

// 截图整个屏幕
pixpin.CaptureFullScreen();

// 截图鼠标所在窗口并贴图
pixpin.CaptureAndPinWindowUnderMouse();

// 截图鼠标周围区域
pixpin.CaptureAroundMouse(400, 300, ShotAction.Save);

// OCR 识别文字
pixpin.CaptureAndOcr();
```

### 3. 批量操作

```csharp
// 批量截图
pixpin.CaptureMultiple(
    (pixpin.GenRect(100, 100, 200, 150), ShotAction.Pin),
    (pixpin.GenRect(400, 200, 300, 200), ShotAction.Copy),
    (pixpin.GenRect(800, 300, 250, 180), ShotAction.Save)
);

// 批量 GIF 录制
pixpin.StartMultipleGifRecordings(
    pixpin.GenRect(100, 100, 400, 300),
    pixpin.GenRect(600, 200, 500, 400)
);
```

## API 参考

### 核心类

#### `PixPin` (抽象基类)
提供所有 PixPin 功能的 .NET 方法接口。

#### `PixPinClient` (完整实现)
通过命令行与 PixPin 集成的完整实现，支持自动进程检测和性能优化。

**性能特性**:
- 🚀 **智能缓存**: 可执行文件路径只检测一次并缓存
- ⚡ **高效执行**: 脚本执行时无需重复进程查询
- 🔍 **轻量检查**: 快速的进程存在性检查
- 🔄 **手动刷新**: 提供 `RefreshExecutablePath()` 供特殊情况使用

```csharp
// 自动检测正在运行的 PixPin 进程路径（推荐）
var client = new PixPinClient();

// 或指定回退路径（当进程检测失败时使用）
var client = new PixPinClient("path/to/pixpin.exe");

// 如果 PixPin 重启或路径变更，可手动刷新
client.RefreshExecutablePath();
```

#### `PixPinConfiguration` (配置类)
配置 PixPin 客户端的行为。

```csharp
var config = PixPinConfiguration.Development();
var client = new PixPinClient(config.ExecutablePath);
```

### 枚举类型

#### `ShotAction` - 截图后动作
- `Copy` - 复制到剪贴板
- `Pin` - 贴图
- `Save` - 保存文件
- `QuickSave` - 快速保存
- `LongShot` - 长截图
- `GifShot` - GIF 录制
- `CopyOcrText` - OCR 文字识别
- `Translate` - 翻译 (需要 API Key 或会员)
- `OcrTable` - 表格识别 (会员功能)
- `Close` - 关闭

#### `SpecialRectType` - 特殊区域类型
- `ScreenUnderMouse` - 鼠标所在屏幕
- `AllScreen` - 所有屏幕
- `WindowUnderMouse` - 鼠标所在窗口
- `LastShotRect` - 上次截图区域

### 主要方法

#### 截图相关

```csharp
// 打开截图界面
pixpin.ScreenShotAndEdit();

// 截图并指定后续动作
pixpin.ScreenShot(ShotAction.Copy);

// 直接截图指定区域
pixpin.DirectScreenShot(rect, ShotAction.Pin);

// 长截图
pixpin.OpenLongScreenShot(x, y, width, height);

// GIF 录制
pixpin.OpenGifScreenShot(x, y, width, height);
```

#### 贴图相关

```csharp
// 从剪贴板贴图
pixpin.PinFromClipBoard();

// 保存所有贴图
pixpin.SaveAllPinImageTo("C:/Screenshots");

// 显示/隐藏所有贴图
pixpin.HideOrShowAllPin();

// 关闭所有贴图
pixpin.CloseAllPin();
```

#### 系统相关

```csharp
// 执行系统命令
pixpin.RunSystem("calc");

// 禁用/启用快捷键
pixpin.DisableShortcuts(true);

// 检查快捷键状态
bool disabled = pixpin.IsDisableShortcuts();
```

## 工作流示例

### 文档截图工作流

```csharp
public static void DocumentationWorkflow(PixPinClient pixpin)
{
    // 1. 截图 UI 元素
    pixpin.ScreenShot(ShotAction.Pin);
    
    // 2. 手动添加注释 (用户操作)
    
    // 3. 保存注释后的截图
    pixpin.SaveAllPinImageTo("C:/Documentation/Screenshots");
    
    // 4. 清理
    pixpin.CloseAllPin();
}
```

### Bug 报告工作流

```csharp
public static void BugReportingWorkflow(PixPinClient pixpin)
{
    // 1. 截图问题区域
    pixpin.CaptureAndPinWindowUnderMouse();
    
    // 2. 录制问题重现过程
    var windowRect = pixpin.GetSpRect(SpecialRectType.WindowUnderMouse);
    pixpin.GifScreenShot(windowRect);
    
    // 3. 保存所有内容用于 Bug 报告
    pixpin.SaveAllPinImageWithDialog();
}
```

### 设计评审工作流

```csharp
public static void DesignReviewWorkflow(PixPinClient pixpin)
{
    // 1. 截图多个设计方案
    pixpin.CaptureMultiple(
        (pixpin.GenRect(100, 100, 400, 600), ShotAction.Pin),  // 设计 A
        (pixpin.GenRect(600, 100, 400, 600), ShotAction.Pin),  // 设计 B
        (pixpin.GenRect(1100, 100, 400, 600), ShotAction.Pin)  // 设计 C
    );
    
    // 2. 贴图对比 (已自动贴图)
    
    // 3. 保存选中的设计
    pixpin.SaveAllPinImageTo("C:/DesignReview/Selected");
    
    // 4. 清理
    pixpin.CloseAllPin();
}
```

## 配置选项

```csharp
var config = new PixPinConfiguration
{
    ExecutablePath = "C:/Program Files/PixPin/pixpin.exe",
    ExecutionTimeoutMs = 30000,
    AutoStartPixPin = true,
    StartupDelayMs = 2000,
    ThrowOnExecutionError = true,
    DefaultShotAction = ShotAction.Copy,
    DefaultSavePath = "C:/Screenshots",
    LogScripts = false
};
```

### 预设配置

```csharp
// 开发环境配置
var devConfig = PixPinConfiguration.Development();

// 生产环境配置
var prodConfig = PixPinConfiguration.Production();

// 默认配置
var defaultConfig = PixPinConfiguration.Default();
```

## 自定义实现

如果您需要自定义脚本执行逻辑，可以继承 `PixPin` 基类：

```csharp
public class MyPixPinImplementation : PixPin
{
    protected override void ExecuteScript(string script)
    {
        // 自定义脚本执行逻辑
        // 例如：通过 IPC、网络请求等方式与 PixPin 通信
    }

    protected override T ExecuteScriptWithReturn<T>(string script)
    {
        // 自定义带返回值的脚本执行逻辑
        return default(T);
    }
}
```

## 快速测试

我们提供了一个完整的控制台测试程序来验证 SDK 功能：

```cmd
# 快速启动测试（推荐）
run-test.cmd

# 或者手动运行
dotnet run --project src/PixPin.Console/PixPin.Console.csproj
```

测试程序提供交互式菜单，包含：
- 📸 基础截图功能测试
- 📌 贴图功能测试  
- 🚀 扩展方法测试
- 📋 工作流示例测试
- ⚙️ 系统功能测试
- 🔄 批量操作测试

详细使用说明请参考：[PixPin.Console/README.md](src/PixPin.Console/README.md)

## 项目结构

```
src/
├── PixPin.Core/                 # 核心 SDK 库
│   ├── Class1.cs                # 主要的 PixPin 基类
│   ├── PixPinClient.cs          # 完整的命令行集成实现
│   ├── Enums/
│   │   ├── ShotAction.cs        # 截图动作枚举
│   │   └── SpecialRectType.cs   # 特殊区域类型枚举
│   ├── Models/
│   │   ├── PixRect.cs           # 矩形区域模型
│   │   └── SystemCommandResult.cs # 系统命令执行结果
│   ├── Extensions/
│   │   └── PixPinExtensions.cs  # 扩展方法
│   ├── Configuration/
│   │   └── PixPinConfiguration.cs # 配置类
│   └── Examples/
│       └── PixPinExample.cs     # 使用示例
└── PixPin.Console/              # 测试控制台程序
    ├── Program.cs               # 主测试程序
    ├── TestConfiguration.cs     # 测试配置
    └── README.md               # 测试使用说明
```

## 依赖要求

- .NET 8.0+
- PixPin 软件 (需要先安装并运行)
- Windows 操作系统 (PixPin 命令行功能仅支持 Windows)

## 注意事项

1. **性能优化**: `PixPinClient` 使用路径缓存机制，避免频繁的进程查询，提供优秀的执行性能
2. **自动进程检测**: 会自动检测正在运行的 PixPin 进程并获取可执行文件路径，无需手动配置
3. **PixPin 进程**: 如果 PixPin 未运行，可使用 `EnsurePixPinRunning()` 自动启动
4. **可用性检查**: 使用 `IsAvailable` 属性检查 PixPin 客户端是否可用
5. **路径刷新**: 通常无需手动刷新路径，但如果 PixPin 重启到不同位置，可调用 `RefreshExecutablePath()`
6. **路径转义**: 传递文件路径时注意使用正确的路径分隔符
7. **异常处理**: 建议在调用方法时添加适当的异常处理
8. **权限**: 某些功能可能需要管理员权限
9. **会员功能**: 部分高级功能需要 PixPin 会员订阅

## 贡献

欢迎提交 Issue 和 Pull Request 来改进这个 SDK。

## 许可证

[MIT License](LICENSE)

## 相关链接

- [PixPin 官网](https://pixpin.cn/)
- [PixPin 文档](https://pixpin.cn/docs/)
- [PixPin 脚本文档](https://pixpin.cn/docs/configuration/script.html)