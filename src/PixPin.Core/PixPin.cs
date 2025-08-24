#if NETSTANDARD2_0 || NETSTANDARD2_1
using System;
#endif

using PixPin.Core.Enums;
using PixPin.Core.Models;

namespace PixPin.Core;

/// <summary>
/// PixPin .NET SDK - Provides .NET methods that generate JavaScript scripts for PixPin automation
/// </summary>
public class PixPin
{
    #region Screenshot Methods

    /// <summary>
    /// Opens the screenshot interface
    /// </summary>
    public void ScreenShotAndEdit()
    {
        ExecuteScript("pixpin.screenShotAndEdit()");
    }

    /// <summary>
    /// Opens the screenshot interface and sets the action to perform after user selects screenshot area
    /// </summary>
    /// <param name="afterShotAction">Action to perform after screenshot</param>
    public void ScreenShot(ShotAction afterShotAction)
    {
        ExecuteScript($"pixpin.screenShot(ShotAction.{afterShotAction})");
    }

    /// <summary>
    /// Takes a direct screenshot of specified area
    /// </summary>
    /// <param name="shotRect">Area to screenshot</param>
    /// <param name="shotAction">Action to perform after screenshot</param>
    public void DirectScreenShot(PixRect shotRect, ShotAction shotAction)
    {
        ExecuteScript($"pixpin.directScreenShot(pixpin.genRect({shotRect.X}, {shotRect.Y}, {shotRect.Width}, {shotRect.Height}), ShotAction.{shotAction})");
    }

    /// <summary>
    /// Pauses/continues GIF recording
    /// </summary>
    /// <returns>True if successful, false if failed (not currently in GIF recording state)</returns>
    public bool GifShotPause()
    {
        return ExecuteScriptWithReturn<bool>("pixpin.gifShotPause()");
    }

    /// <summary>
    /// Sets the screenshot area
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    /// <param name="width">Width</param>
    /// <param name="height">Height</param>
    /// <returns>True if successful, false if failed (not currently in screenshot interface)</returns>
    public bool SetScreenShotRect(int x, int y, int width, int height)
    {
        return ExecuteScriptWithReturn<bool>($"pixpin.setScreenShotRect({x}, {y}, {width}, {height})");
    }

    /// <summary>
    /// Sets the radius of screenshot area corners
    /// </summary>
    /// <param name="ratio">Corner radius ratio</param>
    /// <returns>True if successful, false if failed (not currently in screenshot interface)</returns>
    public bool SetScreenShotRectRadius(double ratio)
    {
        return ExecuteScriptWithReturn<bool>($"pixpin.setScreenShotRectRadius({ratio})");
    }

    /// <summary>
    /// Opens the custom screenshot interface
    /// </summary>
    public void OpenCustomScreenShot()
    {
        ExecuteScript("pixpin.openCustomScreenShot()");
    }

    /// <summary>
    /// Opens the long screenshot interface
    /// </summary>
    /// <param name="area">Area for long screenshot</param>
    public void LongScreenShot(PixRect area)
    {
        ExecuteScript($"pixpin.longScreenShot(pixpin.genRect({area.X}, {area.Y}, {area.Width}, {area.Height}))");
    }

    /// <summary>
    /// Opens the long screenshot interface
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    /// <param name="width">Width</param>
    /// <param name="height">Height</param>
    public void OpenLongScreenShot(int x, int y, int width, int height)
    {
        ExecuteScript($"pixpin.openLongScreenShot({x}, {y}, {width}, {height})");
    }

    /// <summary>
    /// Opens the GIF screenshot interface
    /// </summary>
    /// <param name="area">Area for GIF screenshot</param>
    public void GifScreenShot(PixRect area)
    {
        ExecuteScript($"pixpin.gifScreenShot(pixpin.genRect({area.X}, {area.Y}, {area.Width}, {area.Height}))");
    }

    /// <summary>
    /// Opens the GIF screenshot interface
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    /// <param name="width">Width</param>
    /// <param name="height">Height</param>
    public void OpenGifScreenShot(int x, int y, int width, int height)
    {
        ExecuteScript($"pixpin.openGifScreenShot({x}, {y}, {width}, {height})");
    }

    /// <summary>
    /// Generates a rectangle area centered on mouse position
    /// </summary>
    /// <param name="width">Width</param>
    /// <param name="height">Height</param>
    /// <returns>Rectangle area centered on mouse position</returns>
    public PixRect GenRectUnderCursor(int width, int height)
    {
        // Note: This would need custom implementation to return actual values
        ExecuteScript($"pixpin.genRectUnderCursor({width}, {height})");
        return new PixRect(0, 0, width, height); // Placeholder return
    }

    /// <summary>
    /// Generates a rectangle area
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    /// <param name="width">Width</param>
    /// <param name="height">Height</param>
    /// <returns>Rectangle area</returns>
    public PixRect GenRect(int x, int y, int width, int height)
    {
        return new PixRect(x, y, width, height);
    }

    /// <summary>
    /// Gets special rectangle area
    /// </summary>
    /// <param name="type">Type of special rectangle</param>
    /// <returns>Rectangle area</returns>
    public PixRect GetSpRect(SpecialRectType type)
    {
        string constName = type switch
        {
            SpecialRectType.ScreenUnderMouse => "PixConst.SpRectScreenUnderMouse",
            SpecialRectType.AllScreen => "PixConst.SpRectAllScreen",
            SpecialRectType.WindowUnderMouse => "PixConst.SpRectWindowUnderMouse",
            SpecialRectType.LastShotRect => "PixConst.SpRectLastShotRect",
            _ => throw new ArgumentException($"Invalid SpecialRectType: {type}")
        };
        
        // Note: This would need custom implementation to return actual values
        ExecuteScript($"pixpin.getSpRect({constName})");
        return new PixRect(); // Placeholder return
    }

    #endregion

    #region Pin Methods

    /// <summary>
    /// Pins data from clipboard
    /// </summary>
    public void PinFromClipBoard()
    {
        ExecuteScript("pixpin.pinFromCilpBoard()");
    }

    /// <summary>
    /// Destroys all pins
    /// </summary>
    public void DestroyAllPin()
    {
        ExecuteScript("pixpin.destoryAllPin()");
    }

    /// <summary>
    /// Saves all pin images to specified path
    /// </summary>
    /// <param name="path">Path to save images to</param>
    public void SaveAllPinImageTo(string path)
    {
        // Escape path for JavaScript
        string escapedPath = path.Replace("\\", "/");
        ExecuteScript($"pixpin.saveAllPinImageTo(\"{escapedPath}\")");
    }

    /// <summary>
    /// Saves all pin images with save dialog
    /// </summary>
    public void SaveAllPinImageWithDialog()
    {
        ExecuteScript("pixpin.saveAllPinImageWithDialog()");
    }

    /// <summary>
    /// Restores the last closed pin
    /// </summary>
    /// <returns>True if successful, false if failed</returns>
    public bool RestoreLastClosedPin()
    {
        return ExecuteScriptWithReturn<bool>("pixpin.restoreLastClosedPin()");
    }

    /// <summary>
    /// Toggles mouse penetration for pin under mouse
    /// </summary>
    public void TriggerMousePenetration()
    {
        ExecuteScript("pixpin.trigMousePenetration()");
    }

    /// <summary>
    /// Closes all pins
    /// </summary>
    public void CloseAllPin()
    {
        ExecuteScript("pixpin.closeAllPin()");
    }

    /// <summary>
    /// Sets all pins to ROI (thumbnail) mode
    /// </summary>
    public void SetAllPinToRoiMode()
    {
        ExecuteScript("pixpin.setAllPinToRoiMode()");
    }

    /// <summary>
    /// Unsets all pins from ROI (thumbnail) mode
    /// </summary>
    public void UnsetAllPinToRoiMode()
    {
        ExecuteScript("pixpin.unsetAllPinToRoiMode()");
    }

    /// <summary>
    /// Hides or shows all pins
    /// </summary>
    public void HideOrShowAllPin()
    {
        ExecuteScript("pixpin.hideOrShowAllPin()");
    }

    /// <summary>
    /// Checks if all pins are hidden
    /// </summary>
    /// <returns>True if all pins are hidden, false if any pin is visible</returns>
    public bool IsAllPinHide()
    {
        return ExecuteScriptWithReturn<bool>("pixpin.isAllPinHide()");
    }

    /// <summary>
    /// Opens the configuration window
    /// </summary>
    public void OpenConfigurationWindow()
    {
        ExecuteScript("pixpin.openConfigurationWindow()");
    }

    #endregion

    #region Other Methods

    /// <summary>
    /// Opens the pin group switching window
    /// </summary>
    public void SwitchPinGroup()
    {
        ExecuteScript("pixpin.switchPinGroup()");
    }

    /// <summary>
    /// Disables or enables shortcuts
    /// </summary>
    /// <param name="disable">True to disable, false to enable</param>
    public void DisableShortcuts(bool disable)
    {
        ExecuteScript($"pixpin.disableShortcuts({disable.ToString().ToLower()})");
    }

    /// <summary>
    /// Checks if shortcuts are disabled
    /// </summary>
    /// <returns>True if shortcuts are disabled, false if enabled</returns>
    public bool IsDisableShortcuts()
    {
        return ExecuteScriptWithReturn<bool>("pixpin.isDisableShortcuts()");
    }

    /// <summary>
    /// Executes a system command
    /// </summary>
    /// <param name="command">Command to execute</param>
    public void RunSystem(string command)
    {
        ExecuteScript($"pixpin.runSystem(\"{command}\")");
    }

    /// <summary>
    /// Executes a system command synchronously and returns result
    /// </summary>
    /// <param name="command">Command to execute</param>
    /// <returns>Command execution result</returns>
    public SystemCommandResult RunSystemSync(string command)
    {
        // Note: This would need custom implementation to return actual values
        ExecuteScript($"pixpin.runSystemSync(\"{command}\")");
        return new SystemCommandResult(); // Placeholder return
    }

    #endregion

    #region Script Execution

    /// <summary>
    /// Executes a JavaScript script in PixPin
    /// This method should be implemented by the user to handle script execution
    /// </summary>
    /// <param name="script">JavaScript script to execute</param>
    protected virtual void ExecuteScript(string script)
    {
        // This method should be implemented by the user
        // It should send the script to PixPin for execution
        throw new NotImplementedException("ExecuteScript method must be implemented by the user");
    }

    /// <summary>
    /// Executes a JavaScript script in PixPin and returns a value
    /// This method should be implemented by the user to handle script execution with return values
    /// </summary>
    /// <typeparam name="T">Return type</typeparam>
    /// <param name="script">JavaScript script to execute</param>
    /// <returns>Result from script execution</returns>
    protected virtual T ExecuteScriptWithReturn<T>(string script)
    {
        // This method should be implemented by the user
        // It should send the script to PixPin for execution and return the result
        throw new NotImplementedException("ExecuteScriptWithReturn method must be implemented by the user");
    }

    #endregion
}
