@echo off
echo ========================================
echo          PixPin .NET SDK 测试工具
echo ========================================
echo.

echo 🔧 构建项目...
dotnet build src/PixPin.Console/PixPin.Console.csproj --configuration Release

if %ERRORLEVEL% neq 0 (
    echo ❌ 构建失败！
    pause
    exit /b 1
)

echo ✅ 构建成功！
echo.

echo 🚀 启动测试程序...
echo.

dotnet run --project src/PixPin.Console/PixPin.Console.csproj --configuration Release

echo.
echo 测试完成。
pause
