#if NETSTANDARD2_0 || NETSTANDARD2_1
using System.Threading;
#endif

using PixPin.Core.Enums;
using PixPin.Core.Models;

namespace PixPin.Core.Extensions;

/// <summary>
/// Extension methods for PixPin to provide convenient operations
/// </summary>
public static class PixPinExtensions
{
    /// <summary>
    /// Takes a screenshot of the entire screen and copies to clipboard
    /// </summary>
    /// <param name="pixpin">PixPin instance</param>
    public static void CaptureFullScreen(this PixPin pixpin)
    {
        var fullScreenRect = pixpin.GetSpRect(SpecialRectType.AllScreen);
        pixpin.DirectScreenShot(fullScreenRect, ShotAction.Copy);
    }

    /// <summary>
    /// Takes a screenshot of the window under mouse and pins it
    /// </summary>
    /// <param name="pixpin">PixPin instance</param>
    public static void CaptureAndPinWindowUnderMouse(this PixPin pixpin)
    {
        var windowRect = pixpin.GetSpRect(SpecialRectType.WindowUnderMouse);
        pixpin.DirectScreenShot(windowRect, ShotAction.Pin);
    }

    /// <summary>
    /// Takes a screenshot of a centered area around the mouse
    /// </summary>
    /// <param name="pixpin">PixPin instance</param>
    /// <param name="width">Width of the capture area</param>
    /// <param name="height">Height of the capture area</param>
    /// <param name="action">Action to perform after screenshot</param>
    public static void CaptureAroundMouse(this PixPin pixpin, int width, int height, ShotAction action = ShotAction.Copy)
    {
        var rect = pixpin.GenRectUnderCursor(width, height);
        pixpin.DirectScreenShot(rect, action);
    }

    /// <summary>
    /// Creates a GIF recording of the last screenshot area
    /// </summary>
    /// <param name="pixpin">PixPin instance</param>
    public static void RecordLastArea(this PixPin pixpin)
    {
        var lastRect = pixpin.GetSpRect(SpecialRectType.LastShotRect);
        pixpin.GifScreenShot(lastRect);
    }

    /// <summary>
    /// Performs OCR on a screenshot and copies the text to clipboard
    /// </summary>
    /// <param name="pixpin">PixPin instance</param>
    public static void CaptureAndOcr(this PixPin pixpin)
    {
        pixpin.ScreenShot(ShotAction.CopyOcrText);
    }

    /// <summary>
    /// Takes a long screenshot of the specified area
    /// </summary>
    /// <param name="pixpin">PixPin instance</param>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    /// <param name="width">Width</param>
    /// <param name="height">Height</param>
    public static void CaptureLongScreenshot(this PixPin pixpin, int x, int y, int width, int height)
    {
        pixpin.OpenLongScreenShot(x, y, width, height);
    }

    /// <summary>
    /// Saves all current pins and then closes them
    /// </summary>
    /// <param name="pixpin">PixPin instance</param>
    /// <param name="savePath">Path to save the pins (optional)</param>
    public static void SaveAndCloseAllPins(this PixPin pixpin, string? savePath = null)
    {
        if (!string.IsNullOrEmpty(savePath))
        {
            pixpin.SaveAllPinImageTo(savePath!);
        }
        else
        {
            pixpin.SaveAllPinImageWithDialog();
        }
        
        pixpin.CloseAllPin();
    }

    /// <summary>
    /// Toggles the visibility of all pins
    /// </summary>
    /// <param name="pixpin">PixPin instance</param>
    /// <returns>True if pins are now hidden, false if they are now visible</returns>
    public static bool TogglePinVisibility(this PixPin pixpin)
    {
        pixpin.HideOrShowAllPin();
        return pixpin.IsAllPinHide();
    }

    /// <summary>
    /// Executes multiple screenshots in sequence
    /// </summary>
    /// <param name="pixpin">PixPin instance</param>
    /// <param name="screenshots">Array of screenshot configurations</param>
    public static void CaptureMultiple(this PixPin pixpin, params (PixRect area, ShotAction action)[] screenshots)
    {
        foreach (var (area, action) in screenshots)
        {
            pixpin.DirectScreenShot(area, action);
            // Small delay between screenshots
            Thread.Sleep(500);
        }
    }

    /// <summary>
    /// Creates a batch of GIF recordings
    /// </summary>
    /// <param name="pixpin">PixPin instance</param>
    /// <param name="areas">Areas to record</param>
    public static void StartMultipleGifRecordings(this PixPin pixpin, params PixRect[] areas)
    {
        foreach (var area in areas)
        {
            pixpin.GifScreenShot(area);
            Thread.Sleep(1000); // Delay between starting recordings
        }
    }
}
