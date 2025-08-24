#!/usr/bin/env pwsh

<#
.SYNOPSIS
    准备发布 PixPin .NET SDK 到 NuGet
.DESCRIPTION
    此脚本帮助准备发布新版本，包括版本检查、构建验证和发布指导
.PARAMETER Version
    要发布的版本号 (例如: 1.0.0)
.PARAMETER Prerelease
    是否为预发布版本
.EXAMPLE
    .\prepare-release.ps1 -Version "1.0.0"
    .\prepare-release.ps1 -Version "1.1.0-beta.1" -Prerelease
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$Version,
    
    [Parameter(Mandatory = $false)]
    [switch]$Prerelease
)

# 颜色输出函数
function Write-Success { param($Message) Write-Host "✅ $Message" -ForegroundColor Green }
function Write-Warning { param($Message) Write-Host "⚠️ $Message" -ForegroundColor Yellow }
function Write-Error { param($Message) Write-Host "❌ $Message" -ForegroundColor Red }
function Write-Info { param($Message) Write-Host "ℹ️ $Message" -ForegroundColor Cyan }

Write-Host "🚀 准备发布 PixPin .NET SDK v$Version" -ForegroundColor Magenta
Write-Host "========================================" -ForegroundColor Magenta

# 1. 验证版本格式
Write-Info "验证版本格式..."
if ($Version -notmatch '^\d+\.\d+\.\d+(\-.+)?$') {
    Write-Error "版本格式无效。请使用语义化版本格式 (例如: 1.0.0 或 1.0.0-beta.1)"
    exit 1
}
Write-Success "版本格式有效: $Version"

# 2. 检查工作目录
Write-Info "检查工作目录..."
if (-not (Test-Path "src/PixPin.Core/PixPin.Core.csproj")) {
    Write-Error "请在项目根目录运行此脚本"
    exit 1
}
Write-Success "工作目录正确"

# 3. 检查 Git 状态
Write-Info "检查 Git 状态..."
$gitStatus = git status --porcelain
if ($gitStatus) {
    Write-Warning "工作目录有未提交的更改："
    $gitStatus | ForEach-Object { Write-Host "  $_" -ForegroundColor Yellow }
    $continue = Read-Host "是否继续? (y/N)"
    if ($continue -ne 'y') {
        Write-Error "已取消发布"
        exit 1
    }
}
Write-Success "Git 状态检查完成"

# 4. 更新项目版本
Write-Info "更新项目版本到 $Version..."
$projectFile = "src/PixPin.Core/PixPin.Core.csproj"
$content = Get-Content $projectFile -Raw
$content = $content -replace '<PackageVersion>.*</PackageVersion>', "<PackageVersion>$Version</PackageVersion>"
Set-Content $projectFile $content -NoNewline
Write-Success "项目版本已更新"

# 5. 构建和测试
Write-Info "执行构建和测试..."
try {
    dotnet restore
    if ($LASTEXITCODE -ne 0) { throw "Restore failed" }
    
    dotnet build --configuration Release --no-restore
    if ($LASTEXITCODE -ne 0) { throw "Build failed" }
    
    dotnet test --configuration Release --no-build
    if ($LASTEXITCODE -ne 0) { throw "Tests failed" }
    
    Write-Success "构建和测试成功"
} catch {
    Write-Error "构建或测试失败: $_"
    exit 1
}

# 6. 创建包并验证
Write-Info "创建 NuGet 包..."
try {
    dotnet pack src/PixPin.Core/PixPin.Core.csproj --configuration Release --no-build --output "./release-packages"
    if ($LASTEXITCODE -ne 0) { throw "Pack failed" }
    
    $packageFile = Get-ChildItem "./release-packages" -Filter "PixPin.Core.$Version.nupkg"
    if (-not $packageFile) {
        throw "Package file not found"
    }
    
    Write-Success "NuGet 包创建成功: $($packageFile.Name)"
    Write-Info "包大小: $([math]::Round($packageFile.Length / 1KB, 2)) KB"
} catch {
    Write-Error "包创建失败: $_"
    exit 1
}

# 7. 显示发布指导
Write-Host ""
Write-Host "🎉 发布准备完成！" -ForegroundColor Green
Write-Host "==================" -ForegroundColor Green
Write-Host ""

if ($Prerelease) {
    Write-Host "📋 预发布版本发布步骤：" -ForegroundColor Yellow
} else {
    Write-Host "📋 正式版本发布步骤：" -ForegroundColor Cyan
}

Write-Host ""
Write-Host "1. 提交更改：" -ForegroundColor White
Write-Host "   git add ." -ForegroundColor Gray
Write-Host "   git commit -m `"Release v$Version`"" -ForegroundColor Gray
Write-Host ""

Write-Host "2. 创建标签：" -ForegroundColor White
Write-Host "   git tag v$Version" -ForegroundColor Gray
Write-Host "   git push origin v$Version" -ForegroundColor Gray
Write-Host ""

Write-Host "3. 发布方式（选择一种）：" -ForegroundColor White
Write-Host "   a) GitHub Release (推荐)：" -ForegroundColor Cyan
Write-Host "      - 访问: https://github.com/QuickerHub/pixpin-sdk/releases/new" -ForegroundColor Gray
Write-Host "      - 选择标签: v$Version" -ForegroundColor Gray
Write-Host "      - 填写发布说明并发布" -ForegroundColor Gray
Write-Host ""
Write-Host "   b) 手动触发工作流：" -ForegroundColor Cyan
Write-Host "      - 访问: https://github.com/QuickerHub/pixpin-sdk/actions/workflows/publish.yml" -ForegroundColor Gray
Write-Host "      - 点击 'Run workflow'" -ForegroundColor Gray
Write-Host "      - 输入版本: $Version" -ForegroundColor Gray
if ($Prerelease) {
    Write-Host "      - 选择 'Is this a prerelease?' 为 true" -ForegroundColor Gray
}
Write-Host ""

Write-Host "4. 验证发布：" -ForegroundColor White
Write-Host "   - 检查 NuGet.org: https://www.nuget.org/packages/PixPin.Core" -ForegroundColor Gray
Write-Host "   - 测试安装: dotnet add package PixPin.Core --version $Version" -ForegroundColor Gray
Write-Host ""

Write-Host "📁 生成的包文件：" -ForegroundColor White
Get-ChildItem "./release-packages" | ForEach-Object {
    Write-Host "   $($_.Name)" -ForegroundColor Gray
}

Write-Host ""
Write-Warning "记得在发布后删除 ./release-packages 目录"
Write-Host ""
Write-Success "准备完成！按照上述步骤进行发布。"
