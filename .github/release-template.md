## 🎉 PixPin SDK v{VERSION} 发布

### ✨ 新增功能
- 

### 🐛 Bug 修复
- 

### 🔧 改进优化
- 

### 📦 技术更新
- 

### 🌐 多版本支持
此版本支持以下 .NET 框架：
- ✅ .NET 8.0 (推荐)
- ✅ .NET 7.0
- ✅ .NET 6.0 (LTS)
- ✅ .NET Standard 2.1
- ✅ .NET Standard 2.0

### 📦 安装方式

#### NuGet Package Manager
```
Install-Package PixPin.Core -Version {VERSION}
```

#### .NET CLI
```bash
dotnet add package PixPin.Core --version {VERSION}
```

#### PackageReference
```xml
<PackageReference Include="PixPin.Core" Version="{VERSION}" />
```

### 🚀 快速开始

```csharp
using PixPin.Core;
using PixPin.Core.Enums;

// 创建 PixPin 客户端（自动检测进程）
var pixpin = new PixPinClient();

// 检查可用性
if (pixpin.IsAvailable)
{
    // 截图并复制到剪贴板
    pixpin.ScreenShot(ShotAction.Copy);
    
    // 直接截图指定区域并贴图
    var rect = pixpin.GenRect(100, 100, 500, 300);
    pixpin.DirectScreenShot(rect, ShotAction.Pin);
}
```

### 🔗 相关链接
- 📚 [完整文档](https://github.com/QuickerHub/pixpin-sdk/blob/main/README.md)
- 🎯 [.NET 版本支持指南](https://github.com/QuickerHub/pixpin-sdk/blob/main/.NET_SUPPORT.md)
- 🧪 [测试示例](https://github.com/QuickerHub/pixpin-sdk/tree/main/src/PixPin.Console)
- 🐛 [问题报告](https://github.com/QuickerHub/pixpin-sdk/issues)

### 💡 注意事项
- 需要先安装 PixPin 软件
- 仅支持 Windows 操作系统
- 建议使用 .NET 8.0 或 .NET 6.0 LTS 版本

### 🙏 致谢
感谢所有贡献者和使用者的支持！

---

**完整变更日志**: https://github.com/QuickerHub/pixpin-sdk/compare/v{PREV_VERSION}...v{VERSION}
