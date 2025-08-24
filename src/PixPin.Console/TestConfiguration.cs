namespace PixPin.Console;

/// <summary>
/// Test configuration for PixPin Console application
/// </summary>
public class TestConfiguration
{
    /// <summary>
    /// Fallback PixPin executable path (used when process detection fails)
    /// </summary>
    public string? FallbackPixPinPath { get; set; } = "pixpin.exe";

    /// <summary>
    /// Delay between test operations (milliseconds)
    /// </summary>
    public int TestDelayMs { get; set; } = 1000;

    /// <summary>
    /// Timeout for PixPin startup (milliseconds)
    /// </summary>
    public int StartupTimeoutMs { get; set; } = 5000;

    /// <summary>
    /// Temporary directory for test outputs
    /// </summary>
    public string TempDirectory { get; set; } = Path.Combine(Path.GetTempPath(), "PixPinTest");

    /// <summary>
    /// Whether to automatically clean up test files
    /// </summary>
    public bool AutoCleanup { get; set; } = true;

    /// <summary>
    /// Test screenshot coordinates and sizes
    /// </summary>
    public TestCoordinates Coordinates { get; set; } = new();

    /// <summary>
    /// Whether to show detailed output
    /// </summary>
    public bool VerboseOutput { get; set; } = true;
}

/// <summary>
/// Test coordinates for screenshots
/// </summary>
public class TestCoordinates
{
    public int SmallScreenshotX { get; set; } = 100;
    public int SmallScreenshotY { get; set; } = 100;
    public int SmallScreenshotWidth { get; set; } = 400;
    public int SmallScreenshotHeight { get; set; } = 300;

    public int LargeScreenshotX { get; set; } = 200;
    public int LargeScreenshotY { get; set; } = 200;
    public int LargeScreenshotWidth { get; set; } = 800;
    public int LargeScreenshotHeight { get; set; } = 600;

    public int MouseAreaWidth { get; set; } = 300;
    public int MouseAreaHeight { get; set; } = 200;
}
