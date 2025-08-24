namespace PixPin.Core.Models;

/// <summary>
/// Represents a rectangle area for PixPin operations
/// </summary>
public class PixRect
{
    /// <summary>
    /// X coordinate
    /// </summary>
    public int X { get; set; }
    
    /// <summary>
    /// Y coordinate
    /// </summary>
    public int Y { get; set; }
    
    /// <summary>
    /// Width of the rectangle
    /// </summary>
    public int Width { get; set; }
    
    /// <summary>
    /// Height of the rectangle
    /// </summary>
    public int Height { get; set; }
    
    /// <summary>
    /// Creates a new PixRect instance
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    /// <param name="width">Width</param>
    /// <param name="height">Height</param>
    public PixRect(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }
    
    /// <summary>
    /// Creates an empty PixRect
    /// </summary>
    public PixRect() : this(0, 0, 0, 0)
    {
    }
}
