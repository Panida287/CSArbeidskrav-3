namespace EventPlatform.UI.Validators;

/// <summary>
/// Reusable input helpers. All methods loop until valid input is received.
/// </summary>
public static class InputValidator
{
    /// <summary>Prompts until the user enters a valid integer within the given range.</summary>
    public static int GetValidInt(string prompt, int min, int max) => throw new NotImplementedException();

    /// <summary>Prompts until the user enters a non-empty string.</summary>
    public static string GetNonEmptyString(string prompt) => throw new NotImplementedException();

    /// <summary>Prompts until the user enters a date in dd/MM/yyyy format that is in the future.</summary>
    public static DateTime GetFutureDate(string prompt) => throw new NotImplementedException();

    /// <summary>Prompts until the user enters a positive decimal price.</summary>
    public static decimal GetValidPrice(string prompt) => throw new NotImplementedException();
}
