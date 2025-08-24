namespace PixPin.Core.Configuration;

/// <summary>
/// Configuration options for PixPin client
/// </summary>
public class PixPinConfiguration
{
    /// <summary>
    /// Path to the PixPin executable
    /// </summary>
    public string ExecutablePath { get; set; } = "pixpin.exe";

    /// <summary>
    /// Timeout for script execution in milliseconds
    /// </summary>
    public int ExecutionTimeoutMs { get; set; } = 30000;

    /// <summary>
    /// Whether to automatically start PixPin if it's not running
    /// </summary>
    public bool AutoStartPixPin { get; set; } = true;

    /// <summary>
    /// Delay after starting PixPin before executing scripts (milliseconds)
    /// </summary>
    public int StartupDelayMs { get; set; } = 2000;

    /// <summary>
    /// Whether to throw exceptions on script execution errors
    /// </summary>
    public bool ThrowOnExecutionError { get; set; } = true;

    /// <summary>
    /// Default screenshot action when not specified
    /// </summary>
    public Enums.ShotAction DefaultShotAction { get; set; } = Enums.ShotAction.Copy;

    /// <summary>
    /// Default path for saving images
    /// </summary>
    public string? DefaultSavePath { get; set; }

    /// <summary>
    /// Whether to log executed scripts for debugging
    /// </summary>
    public bool LogScripts { get; set; } = false;

    /// <summary>
    /// Creates a default configuration
    /// </summary>
    /// <returns>Default PixPin configuration</returns>
    public static PixPinConfiguration Default()
    {
        return new PixPinConfiguration();
    }

    /// <summary>
    /// Creates a configuration for development/debugging
    /// </summary>
    /// <returns>Development PixPin configuration</returns>
    public static PixPinConfiguration Development()
    {
        return new PixPinConfiguration
        {
            LogScripts = true,
            ThrowOnExecutionError = true,
            ExecutionTimeoutMs = 60000
        };
    }

    /// <summary>
    /// Creates a configuration for production use
    /// </summary>
    /// <returns>Production PixPin configuration</returns>
    public static PixPinConfiguration Production()
    {
        return new PixPinConfiguration
        {
            LogScripts = false,
            ThrowOnExecutionError = false,
            ExecutionTimeoutMs = 10000
        };
    }
}
