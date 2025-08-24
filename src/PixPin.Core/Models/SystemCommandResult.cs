namespace PixPin.Core.Models;

/// <summary>
/// Result of executing a system command
/// </summary>
public class SystemCommandResult
{
    /// <summary>
    /// Command execution return code
    /// </summary>
    public int Code { get; set; }
    
    /// <summary>
    /// Standard output from command
    /// </summary>
    public string Output { get; set; } = string.Empty;
    
    /// <summary>
    /// Error output from command
    /// </summary>
    public string Error { get; set; } = string.Empty;
}
