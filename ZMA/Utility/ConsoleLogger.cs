namespace ZMA.Utility;

public class ConsoleLogger : LoggerBase
{
    public List<string> LoggedMessages { get; } = new ();
    protected override void LogMessage(string message, string type)
    {
        var entry = CreateLogEntry(message, type);
        Console.WriteLine(entry);
        LoggedMessages.Add(entry);
    }
}