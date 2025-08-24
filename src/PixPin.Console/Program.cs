using PixPin.Core;
using PixPin.Core.Enums;
using PixPin.Core.Extensions;
using PixPin.Core.Configuration;

namespace PixPin.Console;

class Program
{
    private static PixPinClient? _pixPin;
    
    static async Task Main(string[] args)
    {
        System.Console.WriteLine("========================================");
        System.Console.WriteLine("         PixPin .NET SDK 测试程序");
        System.Console.WriteLine("========================================");
        System.Console.WriteLine();

        try
        {
            // Initialize PixPin client
            await InitializePixPin();
            
            if (_pixPin == null)
            {
                System.Console.WriteLine("❌ PixPin 客户端初始化失败，程序退出。");
                return;
            }

            // Show main menu
            await ShowMainMenu();
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"❌ 程序执行出错: {ex.Message}");
            System.Console.WriteLine($"详细信息: {ex}");
        }
        finally
        {
            System.Console.WriteLine("\n按任意键退出...");
            System.Console.ReadKey();
        }
    }

    private static async Task InitializePixPin()
    {
        System.Console.WriteLine("🔧 正在初始化 PixPin 客户端...");
        
        try
        {
            // Create PixPin client - it will automatically detect running process
            _pixPin = new PixPinClient();
            
            // Check if PixPin is currently running
            bool isRunning = _pixPin.IsPixPinRunning();
            System.Console.WriteLine($"🔍 检测 PixPin 进程状态: {(isRunning ? "✅ 正在运行" : "⚠️ 未运行")}");
            
            if (isRunning)
            {
                // Show detected executable path
                string executablePath = _pixPin.ExecutableFileName;
                System.Console.WriteLine($"📍 检测到的可执行文件路径: {executablePath}");
                System.Console.WriteLine($"📊 客户端可用性: {(_pixPin.IsAvailable ? "✅ 可用" : "❌ 不可用")}");
            }
            else
            {
                System.Console.WriteLine("🚀 PixPin 未运行，尝试启动...");
                _pixPin.EnsurePixPinRunning();
                
                await Task.Delay(2000); // Give PixPin time to start
                
                // Re-check status after attempting to start
                bool isNowRunning = _pixPin.IsPixPinRunning();
                System.Console.WriteLine($"   启动后状态: {(isNowRunning ? "✅ 正在运行" : "⚠️ 未能启动")}");
                
                if (isNowRunning)
                {
                    string executablePath = _pixPin.ExecutableFileName;
                    System.Console.WriteLine($"📍 启动后检测到的路径: {executablePath}");
                }
            }

            if (_pixPin.IsAvailable)
            {
                System.Console.WriteLine("✅ PixPin 客户端初始化成功！");
            }
            else
            {
                System.Console.WriteLine("❌ PixPin 客户端不可用");
                System.Console.WriteLine("   请确保 PixPin 已正确安装并且有足够的权限访问");
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"❌ 初始化失败: {ex.Message}");
            _pixPin = null;
        }
        
        System.Console.WriteLine();
    }

    private static async Task ShowMainMenu()
    {
        while (true)
        {
            System.Console.WriteLine("========================================");
            System.Console.WriteLine("              主菜单");
            System.Console.WriteLine("========================================");
            System.Console.WriteLine("1. 基础截图功能测试");
            System.Console.WriteLine("2. 贴图功能测试");
            System.Console.WriteLine("3. 扩展方法测试");
            System.Console.WriteLine("4. 工作流示例测试");
            System.Console.WriteLine("5. 系统功能测试");
            System.Console.WriteLine("6. 批量操作测试");
            System.Console.WriteLine("0. 退出程序");
            System.Console.WriteLine("========================================");
            System.Console.Write("请选择功能 (0-6): ");

            string? input = System.Console.ReadLine();
            System.Console.WriteLine();

            switch (input)
            {
                case "1":
                    await TestBasicScreenshotFeatures();
                    break;
                case "2":
                    await TestPinFeatures();
                    break;
                case "3":
                    await TestExtensionMethods();
                    break;
                case "4":
                    await TestWorkflowExamples();
                    break;
                case "5":
                    await TestSystemFeatures();
                    break;
                case "6":
                    await TestBatchOperations();
                    break;
                case "0":
                    System.Console.WriteLine("👋 感谢使用 PixPin .NET SDK 测试程序！");
                    return;
                default:
                    System.Console.WriteLine("❌ 无效选择，请重试。");
                    break;
            }

            System.Console.WriteLine("\n按任意键继续...");
            System.Console.ReadKey();
            System.Console.Clear();
        }
    }

    private static async Task TestBasicScreenshotFeatures()
    {
        System.Console.WriteLine("📸 测试基础截图功能");
        System.Console.WriteLine("=====================================");

        try
        {
            System.Console.WriteLine("1. 测试打开截图界面...");
            _pixPin!.ScreenShotAndEdit();
            await Task.Delay(1000);
            System.Console.WriteLine("   ✅ 命令已发送");

            System.Console.WriteLine("\n2. 测试截图并复制到剪贴板...");
            _pixPin.ScreenShot(ShotAction.Copy);
            await Task.Delay(1000);
            System.Console.WriteLine("   ✅ 命令已发送");

            System.Console.WriteLine("\n3. 测试直接截图指定区域...");
            var rect = _pixPin.GenRect(100, 100, 400, 300);
            _pixPin.DirectScreenShot(rect, ShotAction.Pin);
            await Task.Delay(1000);
            System.Console.WriteLine($"   ✅ 截图区域: {rect.X}, {rect.Y}, {rect.Width}x{rect.Height}");

            System.Console.WriteLine("\n4. 测试长截图...");
            _pixPin.OpenLongScreenShot(200, 200, 600, 800);
            await Task.Delay(1000);
            System.Console.WriteLine("   ✅ 长截图命令已发送");

            System.Console.WriteLine("\n5. 测试 GIF 录制...");
            _pixPin.OpenGifScreenShot(300, 300, 500, 400);
            await Task.Delay(1000);
            System.Console.WriteLine("   ✅ GIF 录制命令已发送");

            System.Console.WriteLine("\n✅ 基础截图功能测试完成！");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"❌ 测试过程中出错: {ex.Message}");
        }
    }

    private static async Task TestPinFeatures()
    {
        System.Console.WriteLine("📌 测试贴图功能");
        System.Console.WriteLine("=====================================");

        try
        {
            System.Console.WriteLine("1. 测试从剪贴板贴图...");
            _pixPin!.PinFromClipBoard();
            await Task.Delay(1000);
            System.Console.WriteLine("   ✅ 命令已发送");

            System.Console.WriteLine("\n2. 测试检查贴图状态...");
            bool pinsHidden = _pixPin.IsAllPinHide();
            System.Console.WriteLine($"   📊 当前贴图状态: {(pinsHidden ? "隐藏" : "显示")}");

            System.Console.WriteLine("\n3. 测试切换贴图显示/隐藏...");
            _pixPin.HideOrShowAllPin();
            await Task.Delay(1000);
            System.Console.WriteLine("   ✅ 切换命令已发送");

            System.Console.WriteLine("\n4. 测试设置贴图为缩略图模式...");
            _pixPin.SetAllPinToRoiMode();
            await Task.Delay(1000);
            System.Console.WriteLine("   ✅ 缩略图模式命令已发送");

            System.Console.WriteLine("\n5. 测试取消缩略图模式...");
            _pixPin.UnsetAllPinToRoiMode();
            await Task.Delay(1000);
            System.Console.WriteLine("   ✅ 取消缩略图模式命令已发送");

            System.Console.WriteLine("\n6. 测试保存所有贴图...");
            string tempPath = Path.Combine(Path.GetTempPath(), "PixPinTest");
            _pixPin.SaveAllPinImageTo(tempPath);
            await Task.Delay(1000);
            System.Console.WriteLine($"   ✅ 保存路径: {tempPath}");

            System.Console.WriteLine("\n✅ 贴图功能测试完成！");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"❌ 测试过程中出错: {ex.Message}");
        }
    }

    private static async Task TestExtensionMethods()
    {
        System.Console.WriteLine("🚀 测试扩展方法");
        System.Console.WriteLine("=====================================");

        try
        {
            System.Console.WriteLine("1. 测试截图整个屏幕...");
            _pixPin!.CaptureFullScreen();
            await Task.Delay(1000);
            System.Console.WriteLine("   ✅ 全屏截图命令已发送");

            System.Console.WriteLine("\n2. 测试截图鼠标所在窗口并贴图...");
            _pixPin!.CaptureAndPinWindowUnderMouse();
            await Task.Delay(1000);
            System.Console.WriteLine("   ✅ 窗口截图贴图命令已发送");

            System.Console.WriteLine("\n3. 测试截图鼠标周围区域...");
            _pixPin!.CaptureAroundMouse(300, 200, ShotAction.Copy);
            await Task.Delay(1000);
            System.Console.WriteLine("   ✅ 鼠标周围截图命令已发送");

            System.Console.WriteLine("\n4. 测试 OCR 文字识别...");
            _pixPin!.CaptureAndOcr();
            await Task.Delay(1000);
            System.Console.WriteLine("   ✅ OCR 命令已发送");

            System.Console.WriteLine("\n5. 测试切换贴图可见性...");
            bool isHidden = _pixPin!.TogglePinVisibility();
            System.Console.WriteLine($"   📊 贴图现在是: {(isHidden ? "隐藏" : "显示")}");

            System.Console.WriteLine("\n✅ 扩展方法测试完成！");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"❌ 测试过程中出错: {ex.Message}");
        }
    }

    private static async Task TestWorkflowExamples()
    {
        System.Console.WriteLine("📋 测试工作流示例");
        System.Console.WriteLine("=====================================");

        try
        {
            System.Console.WriteLine("1. 测试文档截图工作流...");
            DocumentationWorkflow();
            await Task.Delay(2000);
            System.Console.WriteLine("   ✅ 文档工作流命令已发送");

            System.Console.WriteLine("\n2. 测试 Bug 报告工作流...");
            BugReportingWorkflow();
            await Task.Delay(2000);
            System.Console.WriteLine("   ✅ Bug 报告工作流命令已发送");

            System.Console.WriteLine("\n3. 测试设计评审工作流...");
            DesignReviewWorkflow();
            await Task.Delay(2000);
            System.Console.WriteLine("   ✅ 设计评审工作流命令已发送");

            System.Console.WriteLine("\n✅ 工作流示例测试完成！");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"❌ 测试过程中出错: {ex.Message}");
        }
    }

    private static async Task TestSystemFeatures()
    {
        System.Console.WriteLine("⚙️ 测试系统功能");
        System.Console.WriteLine("=====================================");

        try
        {
            System.Console.WriteLine("1. 测试打开计算器...");
            _pixPin!.RunSystem("calc");
            await Task.Delay(1000);
            System.Console.WriteLine("   ✅ 计算器启动命令已发送");

            System.Console.WriteLine("\n2. 测试禁用快捷键...");
            _pixPin.DisableShortcuts(true);
            await Task.Delay(1000);
            System.Console.WriteLine("   ✅ 禁用快捷键命令已发送");

            System.Console.WriteLine("\n3. 测试检查快捷键状态...");
            bool shortcutsDisabled = _pixPin.IsDisableShortcuts();
            System.Console.WriteLine($"   📊 快捷键状态: {(shortcutsDisabled ? "已禁用" : "已启用")}");

            System.Console.WriteLine("\n4. 测试重新启用快捷键...");
            _pixPin.DisableShortcuts(false);
            await Task.Delay(1000);
            System.Console.WriteLine("   ✅ 启用快捷键命令已发送");

            System.Console.WriteLine("\n5. 测试打开配置窗口...");
            _pixPin.OpenConfigurationWindow();
            await Task.Delay(1000);
            System.Console.WriteLine("   ✅ 配置窗口命令已发送");

            System.Console.WriteLine("\n6. 测试切换贴图组...");
            _pixPin.SwitchPinGroup();
            await Task.Delay(1000);
            System.Console.WriteLine("   ✅ 贴图组切换命令已发送");

            System.Console.WriteLine("\n✅ 系统功能测试完成！");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"❌ 测试过程中出错: {ex.Message}");
        }
    }

    private static async Task TestBatchOperations()
    {
        System.Console.WriteLine("🔄 测试批量操作");
        System.Console.WriteLine("=====================================");

        try
        {
            System.Console.WriteLine("1. 测试批量截图...");
            _pixPin!.CaptureMultiple(
                (_pixPin!.GenRect(100, 100, 200, 150), ShotAction.Pin),
                (_pixPin!.GenRect(400, 200, 300, 200), ShotAction.Copy),
                (_pixPin!.GenRect(800, 300, 250, 180), ShotAction.Save)
            );
            await Task.Delay(3000);
            System.Console.WriteLine("   ✅ 批量截图命令已发送");

            System.Console.WriteLine("\n2. 测试批量 GIF 录制...");
            _pixPin!.StartMultipleGifRecordings(
                _pixPin!.GenRect(100, 100, 400, 300),
                _pixPin!.GenRect(600, 200, 500, 400)
            );
            await Task.Delay(3000);
            System.Console.WriteLine("   ✅ 批量 GIF 录制命令已发送");

            System.Console.WriteLine("\n3. 测试保存并关闭所有贴图...");
            string tempPath = Path.Combine(Path.GetTempPath(), "PixPinBatchTest");
            _pixPin!.SaveAndCloseAllPins(tempPath);
            await Task.Delay(2000);
            System.Console.WriteLine($"   ✅ 保存路径: {tempPath}");

            System.Console.WriteLine("\n✅ 批量操作测试完成！");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"❌ 测试过程中出错: {ex.Message}");
        }
    }

    // Workflow implementations
    private static void DocumentationWorkflow()
    {
        System.Console.WriteLine("   📝 执行文档截图工作流...");
        _pixPin!.ScreenShot(ShotAction.Pin);
        // In real scenario, user would manually annotate
        System.Threading.Thread.Sleep(500);
        string docPath = Path.Combine(Path.GetTempPath(), "Documentation", "Screenshots");
        _pixPin.SaveAllPinImageTo(docPath);
        System.Threading.Thread.Sleep(500);
        _pixPin.CloseAllPin();
    }

    private static void BugReportingWorkflow()
    {
        System.Console.WriteLine("   🐛 执行 Bug 报告工作流...");
        _pixPin!.CaptureAndPinWindowUnderMouse();
        System.Threading.Thread.Sleep(500);
        var windowRect = _pixPin!.GetSpRect(SpecialRectType.WindowUnderMouse);
        _pixPin!.GifScreenShot(windowRect);
        System.Threading.Thread.Sleep(500);
        _pixPin!.SaveAllPinImageWithDialog();
    }

    private static void DesignReviewWorkflow()
    {
        System.Console.WriteLine("   🎨 执行设计评审工作流...");
        _pixPin!.CaptureMultiple(
            (_pixPin!.GenRect(100, 100, 400, 600), ShotAction.Pin),
            (_pixPin!.GenRect(600, 100, 400, 600), ShotAction.Pin),
            (_pixPin!.GenRect(1100, 100, 400, 600), ShotAction.Pin)
        );
        System.Threading.Thread.Sleep(1000);
        string designPath = Path.Combine(Path.GetTempPath(), "DesignReview", "Selected");
        _pixPin!.SaveAllPinImageTo(designPath);
        System.Threading.Thread.Sleep(500);
        _pixPin!.CloseAllPin();
    }
}
