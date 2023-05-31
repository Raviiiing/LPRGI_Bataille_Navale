namespace Core;

public enum LogLevel
{
    Info,
    Warning,
    Error
}

public abstract class ConsoleLogger
{
    public static void Log(string message, LogLevel level)
    {
        var logPrefix = GetLogLevelPrefix(level);
        var logMessage = $"{DateTime.Now} [{logPrefix}]: {message}";
        var originalColor = Console.ForegroundColor;

        Console.ForegroundColor = level switch
        {
            LogLevel.Info => ConsoleColor.Green,
            LogLevel.Warning => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Red,
            _ => Console.ForegroundColor
        };

        Console.WriteLine(logMessage);
        Console.ForegroundColor = originalColor;
    }

    private static string GetLogLevelPrefix(LogLevel level)
    {
        return level switch
        {
            LogLevel.Info => "INFO",
            LogLevel.Warning => "WARNING",
            LogLevel.Error => "ERROR",
            _ => "UNKNOWN"
        };
    }
}