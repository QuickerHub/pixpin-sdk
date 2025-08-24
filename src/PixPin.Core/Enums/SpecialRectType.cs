namespace PixPin.Core.Enums;

/// <summary>
/// Special rectangle types for getting specific screen areas
/// </summary>
public enum SpecialRectType
{
    /// <summary>
    /// Screen area where mouse is located
    /// </summary>
    ScreenUnderMouse,
    
    /// <summary>
    /// All screen areas
    /// </summary>
    AllScreen,
    
    /// <summary>
    /// Window area where mouse is located
    /// </summary>
    WindowUnderMouse,
    
    /// <summary>
    /// Last screenshot area
    /// </summary>
    LastShotRect
}
