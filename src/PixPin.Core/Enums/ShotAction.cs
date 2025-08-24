namespace PixPin.Core.Enums;

/// <summary>
/// Defines actions to take after taking a screenshot
/// </summary>
public enum ShotAction
{
    /// <summary>
    /// Copy screenshot to clipboard
    /// </summary>
    Copy,
    
    /// <summary>
    /// Pin the screenshot
    /// </summary>
    Pin,
    
    /// <summary>
    /// Take a long screenshot
    /// </summary>
    LongShot,
    
    /// <summary>
    /// Take a GIF screenshot
    /// </summary>
    GifShot,
    
    /// <summary>
    /// Copy OCR text from screenshot to clipboard
    /// </summary>
    CopyOcrText,
    
    /// <summary>
    /// Save screenshot
    /// </summary>
    Save,
    
    /// <summary>
    /// Quick save screenshot
    /// </summary>
    QuickSave,
    
    /// <summary>
    /// Translate text in screenshot (requires API Key or membership)
    /// </summary>
    Translate,
    
    /// <summary>
    /// Close screenshot
    /// </summary>
    Close,
    
    /// <summary>
    /// Convert text in screenshot to table (membership required)
    /// </summary>
    OcrTable
}
