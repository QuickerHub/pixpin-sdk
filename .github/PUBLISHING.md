# 📦 发布指南

本文档说明如何使用 GitHub Actions 自动发布 PixPin .NET SDK 到 NuGet.org。

## 🔧 前置设置

### 1. 配置 NuGet API Key

1. 登录到 [NuGet.org](https://www.nuget.org/)
2. 进入 [API Keys](https://www.nuget.org/account/apikeys) 页面
3. 创建新的 API Key：
   - **Key Name**: `PixPin-SDK-GitHub-Actions`
   - **Owner**: 选择您的账户或组织
   - **Scopes**: `Push new packages and package versions`
   - **Packages**: `PixPin.Core`

4. 复制生成的 API Key

### 2. 设置 GitHub Secrets

在 GitHub 仓库中设置以下 Secret：

1. 进入仓库 → Settings → Secrets and variables → Actions
2. 添加 Repository Secret：
   - **Name**: `NUGET_API_KEY`
   - **Value**: 粘贴您的 NuGet API Key

## 🚀 发布方式

### 方式一：创建 Release 自动发布（推荐）

1. **创建标签**:
   ```bash
   git tag v1.0.1
   git push origin v1.0.1
   ```

2. **创建 Release**:
   - 进入 GitHub 仓库 → Releases → Create a new release
   - 选择标签 `v1.0.0`
   - 填写发布标题和说明
   - 点击 "Publish release"

3. **自动流程**:
   - ✅ 触发 `release.yml` 工作流创建 Release
   - ✅ 触发 `publish.yml` 工作流发布到 NuGet
   - ✅ 自动生成版本号和变更日志

### 方式二：手动触发发布

1. 进入 GitHub 仓库 → Actions → Publish to NuGet
2. 点击 "Run workflow"
3. 输入参数：
   - **Version**: 例如 `1.0.0`
   - **Prerelease**: 是否为预发布版本
4. 点击 "Run workflow"

### 方式三：命令行发布

```bash
# 创建标签并推送
git tag v1.0.0
git push origin v1.0.0

# GitHub Actions 会自动处理后续流程
```

## 📋 发布流程详解

### 1. CI Build and Test (`ci.yml`)

**触发条件**: Push 到 main/develop 分支或 PR 到 main 分支

**执行步骤**:
- ✅ 多版本 .NET 构建 (6.0, 7.0, 8.0)
- ✅ 运行单元测试
- ✅ 生成构建产物
- ✅ 创建 NuGet 包 (仅 main 分支)

### 2. Create Release (`release.yml`)

**触发条件**: 推送 `v*.*.*` 标签或手动触发

**执行步骤**:
- ✅ 确定版本号
- ✅ 生成变更日志
- ✅ 创建 GitHub Release

### 3. Publish to NuGet (`publish.yml`)

**触发条件**: 发布 Release 或手动触发

**执行步骤**:
- ✅ 确定包版本
- ✅ 更新项目文件版本
- ✅ 构建和测试
- ✅ 创建 NuGet 包
- ✅ 发布到 NuGet.org
- ✅ 生成发布摘要

### 4. Code Quality (`code-quality.yml`)

**触发条件**: Push 到 main/develop 分支或 PR

**执行步骤**:
- ✅ 静态代码分析
- ✅ 代码格式检查
- ✅ 安全扫描
- ✅ 包验证

## 📊 版本管理策略

### 语义化版本

我们遵循 [语义化版本](https://semver.org/lang/zh-CN/) 规范：

- **主版本号**: 不兼容的 API 修改
- **次版本号**: 向下兼容的功能性新增
- **修订号**: 向下兼容的问题修正

### 版本示例

- `1.0.0` - 首个稳定版本
- `1.1.0` - 新增功能
- `1.1.1` - Bug 修复
- `2.0.0` - 破坏性更改
- `1.2.0-beta.1` - Beta 预发布版本

### 分支策略

- `main` - 生产就绪代码，自动发布
- `develop` - 开发分支，CI 构建但不发布
- `feature/*` - 功能分支
- `release/*` - 发布准备分支

## 🔍 发布检查清单

发布前请确认：

- [ ] 代码已通过所有测试
- [ ] 更新了版本号
- [ ] 更新了 CHANGELOG.md
- [ ] 更新了文档
- [ ] 检查了多目标框架兼容性
- [ ] 验证了 NuGet 包内容

## 📈 发布后验证

发布完成后请检查：

1. **NuGet 包状态**:
   - 访问 [PixPin.Core NuGet 页面](https://www.nuget.org/packages/PixPin.Core)
   - 确认新版本已显示
   - 检查下载统计

2. **安装测试**:
   ```bash
   # 创建测试项目
   dotnet new console -n TestPixPinSDK
   cd TestPixPinSDK
   
   # 安装最新版本
   dotnet add package PixPin.Core
   
   # 验证安装
   dotnet list package
   ```

3. **功能验证**:
   - 测试基本 API 调用
   - 验证多目标框架支持
   - 检查性能和兼容性

## ⚠️ 故障排除

### 常见问题

**发布失败**: 
- 检查 NuGet API Key 是否正确
- 确认包版本号未重复
- 检查网络连接

**构建失败**:
- 检查代码编译错误
- 验证测试是否通过
- 检查依赖项版本

**版本冲突**:
- 确保标签版本与项目文件版本一致
- 检查是否已存在相同版本

### 手动回滚

如需回滚发布：

1. **NuGet 包**:
   - 登录 NuGet.org
   - 进入包管理页面
   - 取消列出问题版本

2. **GitHub Release**:
   - 删除或编辑 Release
   - 删除对应标签

## 📞 支持

如遇到发布问题，请：

1. 检查 GitHub Actions 日志
2. 查看此文档的故障排除部分
3. 在仓库中创建 Issue
4. 联系项目维护者

---

💡 **提示**: 建议在非工作时间进行重大版本发布，以便有时间处理可能出现的问题。
