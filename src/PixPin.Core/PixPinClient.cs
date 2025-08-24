#if NETSTANDARD2_0 || NETSTANDARD2_1
using System;
using System.IO;
using System.Linq;
using System.Threading;
#endif

using System.Diagnostics;
using System.Text.Json;
using PixPin.Core.Models;

namespace PixPin.Core;

/// <summary>
/// Complete implementation of PixPin SDK with command line integration.
/// 
/// Performance Optimizations:
/// - Executable path is detected once during initialization and cached
/// - No repeated calls to Process.GetProcessesByName() during script execution
/// - Lightweight process existence checks for IsPixPinRunning()
/// - Manual refresh mechanism available via RefreshExecutablePath() when needed
/// </summary>
public class PixPinClient : PixPin
{
    private const string PixPinProcessName = "pixpin";
    private readonly string? _fallbackExecutablePath;
    private string? _cachedExecutablePath;
    private bool _pathInitialized;

    /// <summary>
    /// Creates a new PixPinClient instance
    /// </summary>
    /// <param name="fallbackExecutablePath">Fallback path to pixpin.exe if process detection fails</param>
    public PixPinClient(string? fallbackExecutablePath = null)
    {
        _fallbackExecutablePath = fallbackExecutablePath ?? "pixpin.exe";
        InitializeExecutablePath();
    }

    /// <summary>
    /// Gets the PixPin executable file path (cached after first detection)
    /// </summary>
    public string ExecutableFileName
    {
        get
        {
            if (!_pathInitialized)
            {
                InitializeExecutablePath();
            }
            
            return _cachedExecutablePath ?? _fallbackExecutablePath ?? "pixpin.exe";
        }
    }

    /// <summary>
    /// Checks if PixPin is available (process is running)
    /// </summary>
    public bool IsAvailable
    {
        get
        {
            // Quick check using cached path
            if (_cachedExecutablePath != null)
            {
                return IsPixPinRunning();
            }
            
            // If no cached path, try to initialize again
            InitializeExecutablePath();
            return _cachedExecutablePath != null && IsPixPinRunning();
        }
    }

    /// <summary>
    /// Initializes the executable path by detecting running PixPin process (called only once or on manual refresh)
    /// </summary>
    private void InitializeExecutablePath()
    {
        try
        {
            var processes = Process.GetProcessesByName(PixPinProcessName);
            var pixPinProcess = processes.FirstOrDefault();
            
            if (pixPinProcess?.MainModule?.FileName is string fileName && File.Exists(fileName))
            {
                _cachedExecutablePath = fileName;
            }
            else
            {
                // No running process found, use fallback
                _cachedExecutablePath = null;
            }
        }
        catch
        {
            // Error during process detection, use fallback
            _cachedExecutablePath = null;
        }
        finally
        {
            _pathInitialized = true;
        }
    }

    /// <summary>
    /// Manually refreshes the PixPin process detection and path cache.
    /// Only call this when you suspect PixPin has been restarted or moved.
    /// </summary>
    public void RefreshExecutablePath()
    {
        _pathInitialized = false;
        InitializeExecutablePath();
    }

    /// <summary>
    /// Executes a JavaScript script in PixPin via command line
    /// </summary>
    /// <param name="script">JavaScript script to execute</param>
    protected override void ExecuteScript(string script)
    {
        try
        {
            var executablePath = ExecutableFileName;
            var arguments = $"-r {JsonString(script)}";

            var startInfo = new ProcessStartInfo
            {
                FileName = executablePath,
                Arguments = arguments,
                UseShellExecute = true, // Use shell execute like in the reference code
                CreateNoWindow = true
            };

            using var process = Process.Start(startInfo);
            // Don't wait for exit as PixPin scripts may be async
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to execute PixPin script: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Executes a JavaScript script in PixPin and returns a value
    /// </summary>
    /// <typeparam name="T">Return type</typeparam>
    /// <param name="script">JavaScript script to execute</param>
    /// <returns>Result from script execution</returns>
    protected override T ExecuteScriptWithReturn<T>(string script)
    {
        try
        {
            var executablePath = ExecutableFileName;
            var arguments = $"-r {JsonString(script)}";

            var startInfo = new ProcessStartInfo
            {
                FileName = executablePath,
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using var process = Process.Start(startInfo);
            if (process == null)
            {
                throw new InvalidOperationException("Failed to start PixPin process");
            }

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"PixPin script execution failed: {error}");
            }

            // Try to parse the output as JSON for complex types
            if (typeof(T) == typeof(bool))
            {
                if (bool.TryParse(output.Trim(), out bool boolResult))
                {
                    return (T)(object)boolResult;
                }
                // Handle JavaScript boolean values
                string lowerOutput = output.Trim().ToLower();
                return (T)(object)(lowerOutput == "true");
            }

            if (typeof(T) == typeof(SystemCommandResult))
            {
                try
                {
                    var result = JsonSerializer.Deserialize<SystemCommandResult>(output);
                    return (T)(object)result!;
                }
                catch
                {
                    // If JSON parsing fails, return a default result
                    return (T)(object)new SystemCommandResult { Output = output };
                }
            }

            // For other types, try direct conversion
            try
            {
                return JsonSerializer.Deserialize<T>(output)!;
            }
            catch
            {
                // If JSON parsing fails, try direct conversion
                return (T)Convert.ChangeType(output.Trim(), typeof(T));
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to execute PixPin script with return: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Executes a script from a file
    /// </summary>
    /// <param name="scriptFilePath">Path to the script file</param>
    public void ExecuteScriptFile(string scriptFilePath)
    {
        try
        {
            var executablePath = ExecutableFileName;
            var startInfo = new ProcessStartInfo
            {
                FileName = executablePath,
                Arguments = $"-f \"{scriptFilePath}\"",
                UseShellExecute = true,
                CreateNoWindow = true
            };

            using var process = Process.Start(startInfo);
            // Don't wait for exit as PixPin scripts may be async
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to execute PixPin script file: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Checks if PixPin is running (lightweight check)
    /// </summary>
    /// <returns>True if PixPin is running, false otherwise</returns>
    public bool IsPixPinRunning()
    {
        try
        {
            // Lightweight check - just see if any pixpin process exists
            return Process.GetProcessesByName(PixPinProcessName).Length > 0;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Starts PixPin if it's not already running
    /// </summary>
    public void EnsurePixPinRunning()
    {
        if (!IsPixPinRunning())
        {
            try
            {
                var executablePath = ExecutableFileName;
                Process.Start(new ProcessStartInfo
                {
                    FileName = executablePath,
                    UseShellExecute = true
                });

                // Wait a bit for PixPin to start
                Thread.Sleep(2000);
                
                // Refresh cached path after starting (in case path changed)
                RefreshExecutablePath();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to start PixPin: {ex.Message}", ex);
            }
        }
    }

    /// <summary>
    /// Escapes a string for use in command line arguments
    /// </summary>
    /// <param name="input">String to escape</param>
    /// <returns>Escaped string</returns>
    private static string JsonString(string input)
    {
        return JsonSerializer.Serialize(input);
    }
}
