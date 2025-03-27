namespace VitoBox.Utils;

public static class Logger
{
    public static void LogInfo(string message) =>
        Console.WriteLine($"[INFO] {DateTime.Now:HH:mm:ss} — {message}");

    public static void LogError(string message) =>
        Console.WriteLine($"[ERR ] {DateTime.Now:HH:mm:ss} — {message}");
    
    public static void LogWarning(string message) =>
        Console.WriteLine($"[WARN ] {DateTime.Now:HH:mm:ss} — {message}");
}
