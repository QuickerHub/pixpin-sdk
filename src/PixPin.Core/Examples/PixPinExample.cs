#if NETSTANDARD2_0 || NETSTANDARD2_1
using System;
using System.IO;
using System.Threading;
#endif

using PixPin.Core.Enums;
using PixPin.Core.Models;
using PixPin.Core.Extensions;
using PixPin.Core.Configuration;

namespace PixPin.Core.Examples;

/// <summary>
/// Example implementation of PixPin SDK showing how to implement the ExecuteScript methods
/// </summary>
public class PixPinExample : PixPin
{
    /// <summary>
    /// Example implementation of ExecuteScript method
    /// In a real implementation, this would send the script to PixPin via command line or IPC
    /// </summary>
    /// <param name="script">JavaScript script to execute</param>
    protected override void ExecuteScript(string script)
    {
        // Example: Execute via command line
        // System.Diagnostics.Process.Start("pixpin.exe", $"-r \"{script}\"");
        
        // For demonstration purposes, we'll just log the script
        Console.WriteLine($"Executing PixPin script: {script}");
    }

    /// <summary>
    /// Example implementation of ExecuteScriptWithReturn method
    /// In a real implementation, this would send the script to PixPin and parse the return value
    /// </summary>
    /// <typeparam name="T">Return type</typeparam>
    /// <param name="script">JavaScript script to execute</param>
    /// <returns>Result from script execution</returns>
    protected override T ExecuteScriptWithReturn<T>(string script)
    {
        // Example: Execute via command line and capture output
        // var process = System.Diagnostics.Process.Start(new ProcessStartInfo
        // {
        //     FileName = "pixpin.exe",
        //     Arguments = $"-r \"{script}\"",
        //     RedirectStandardOutput = true,
        //     UseShellExecute = false
        // });
        // process.WaitForExit();
        // string output = process.StandardOutput.ReadToEnd();
        // return JsonSerializer.Deserialize<T>(output);
        
        // For demonstration purposes, we'll return default values
        Console.WriteLine($"Executing PixPin script with return: {script}");
        return default(T)!;
    }
}

/// <summary>
/// Comprehensive usage examples for the PixPin SDK
/// </summary>
public static class PixPinUsageExamples
{
    public static void RunBasicExamples()
    {
        var pixpin = new PixPinExample();
        
        // Screenshot examples
        Console.WriteLine("=== Basic Screenshot Examples ===");
        
        // Take a screenshot and copy to clipboard
        pixpin.ScreenShot(ShotAction.Copy);
        
        // Take a direct screenshot of a specific area
        var rect = pixpin.GenRect(100, 100, 500, 300);
        pixpin.DirectScreenShot(rect, ShotAction.Pin);
        
        // Open long screenshot
        pixpin.OpenLongScreenShot(0, 0, 1920, 1080);
        
        // Pin examples
        Console.WriteLine("\n=== Pin Management Examples ===");
        
        // Pin from clipboard
        pixpin.PinFromClipBoard();
        
        // Save all pins to desktop
        pixpin.SaveAllPinImageTo("C:/Users/Desktop");
        
        // Toggle all pins visibility
        pixpin.HideOrShowAllPin();
        
        // System examples
        Console.WriteLine("\n=== System Integration Examples ===");
        
        // Open calculator
        pixpin.RunSystem("calc");
        
        // Disable shortcuts temporarily
        pixpin.DisableShortcuts(true);
        
        // Re-enable shortcuts
        pixpin.DisableShortcuts(false);
    }

    public static void RunAdvancedExamples()
    {
        // Using the full PixPinClient with automatic process detection
        var pixpin = new PixPinClient();

        Console.WriteLine("=== Advanced Examples with PixPinClient ===");

        try
        {
            // Ensure PixPin is running
            pixpin.EnsurePixPinRunning();

            // Extension method examples
            Console.WriteLine("\n=== Extension Method Examples ===");
            
            // Capture full screen
            pixpin.CaptureFullScreen();
            
            // Capture window under mouse and pin it
            pixpin.CaptureAndPinWindowUnderMouse();
            
            // Capture area around mouse
            pixpin.CaptureAroundMouse(400, 300, ShotAction.Save);
            
            // OCR example
            pixpin.CaptureAndOcr();

            // Batch operations
            Console.WriteLine("\n=== Batch Operations ===");
            
            // Multiple screenshots
            pixpin.CaptureMultiple(
                (pixpin.GenRect(100, 100, 200, 150), ShotAction.Pin),
                (pixpin.GenRect(400, 200, 300, 200), ShotAction.Copy),
                (pixpin.GenRect(800, 300, 250, 180), ShotAction.Save)
            );

            // Multiple GIF recordings
            pixpin.StartMultipleGifRecordings(
                pixpin.GenRect(100, 100, 400, 300),
                pixpin.GenRect(600, 200, 500, 400)
            );

            // Pin management
            Console.WriteLine("\n=== Advanced Pin Management ===");
            
            // Save and close all pins
            pixpin.SaveAndCloseAllPins("C:/Screenshots");
            
            // Toggle pin visibility and check state
            bool pinsHidden = pixpin.TogglePinVisibility();
            Console.WriteLine($"Pins are now {(pinsHidden ? "hidden" : "visible")}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing PixPin operations: {ex.Message}");
        }
    }

    public static void RunWorkflowExamples()
    {
        var pixpin = new PixPinClient();

        Console.WriteLine("=== Workflow Examples ===");

        // Workflow 1: Screenshot documentation workflow
        DocumentationWorkflow(pixpin);

        // Workflow 2: Bug reporting workflow
        BugReportingWorkflow(pixpin);

        // Workflow 3: Design review workflow
        DesignReviewWorkflow(pixpin);
    }

    private static void DocumentationWorkflow(PixPinClient pixpin)
    {
        Console.WriteLine("\n--- Documentation Workflow ---");
        
        // 1. Take screenshot of specific UI element
        pixpin.ScreenShot(ShotAction.Pin);
        
        // 2. Add annotations (this would be manual)
        // User would manually annotate the pinned screenshot
        
        // 3. Save annotated screenshots
        pixpin.SaveAllPinImageTo("C:/Documentation/Screenshots");
        
        // 4. Clean up
        pixpin.CloseAllPin();
    }

    private static void BugReportingWorkflow(PixPinClient pixpin)
    {
        Console.WriteLine("\n--- Bug Reporting Workflow ---");
        
        // 1. Capture the problematic area
        pixpin.CaptureAndPinWindowUnderMouse();
        
        // 2. Create a GIF of the issue reproduction
        var windowRect = pixpin.GetSpRect(SpecialRectType.WindowUnderMouse);
        pixpin.GifScreenShot(windowRect);
        
        // 3. Save everything for the bug report
        pixpin.SaveAllPinImageWithDialog();
    }

    private static void DesignReviewWorkflow(PixPinClient pixpin)
    {
        Console.WriteLine("\n--- Design Review Workflow ---");
        
        // 1. Capture multiple design variations
        pixpin.CaptureMultiple(
            (pixpin.GenRect(100, 100, 400, 600), ShotAction.Pin),  // Design A
            (pixpin.GenRect(600, 100, 400, 600), ShotAction.Pin),  // Design B
            (pixpin.GenRect(1100, 100, 400, 600), ShotAction.Pin)  // Design C
        );
        
        // 2. Pin them for comparison
        // Screenshots are already pinned from the previous step
        
        // 3. After review, save the selected designs
        pixpin.SaveAllPinImageTo("C:/DesignReview/Selected");
        
        // 4. Clean up
        pixpin.CloseAllPin();
    }
}
