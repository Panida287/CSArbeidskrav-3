namespace EventPlatform.UI;

/// <summary>
/// Shared console display utilities used by all menu classes.
/// Never use raw Console.WriteLine/ReadLine outside this class and the Menu classes.
/// </summary>
public static class ConsoleHelper
{
    public static void PrintHeader(string title)
        => Console.WriteLine($"\n=== {title} ===");

    public static void PrintDivider()
        => Console.WriteLine(new string('-', 60));

    public static void PrintSuccess(string message)
        => Console.WriteLine($"[✓] {message}");

    public static void PrintError(string message)
        => Console.WriteLine($"[✗] {message}");

    public static void PressAnyKeyToContinue()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey(intercept: true);
    }

    public static void ClearAndPrintHeader(string title)
    {
        Console.Clear();
        PrintHeader(title);
    }
}
