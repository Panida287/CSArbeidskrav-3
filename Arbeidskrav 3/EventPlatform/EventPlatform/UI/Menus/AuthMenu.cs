using EventPlatform.Services;

namespace EventPlatform.UI.Menus;

/// <summary>
/// Handles the Welcome screen, Register, and Log In flows.
/// </summary>
public class AuthMenu
{
    private readonly UserService _userService;

    public AuthMenu(UserService userService)
    {
        _userService = userService;
    }

    public void Show()
    {
        while (true)
        {
            ConsoleHelper.ClearAndPrintHeader("Welcome to EventPlatform!");
            ConsoleHelper.PrintDivider();
            Console.WriteLine(" 1. Register");
            Console.WriteLine(" 2. Log In");
            Console.WriteLine(" 3. Exit");
            ConsoleHelper.PrintDivider();
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1": ShowRegister(); break;
                case "2": ShowLogin(); break;
                case "3": return;
                default:
                    ConsoleHelper.PrintError("Invalid option. Please try again.");
                    ConsoleHelper.PressAnyKeyToContinue();
                    break;
            }
        }
    }

    public void ShowRegister()
    {
        ConsoleHelper.ClearAndPrintHeader("Register");
        ConsoleHelper.PrintDivider();

        Console.Write("Choose a username: ");
        string username = Console.ReadLine() ?? "";

        string password = ReadMaskedPassword("Choose a password: ");
        string confirm = ReadMaskedPassword("Confirm password: ");

        if (password != confirm)
        {
            ConsoleHelper.PrintError("Passwords do not match.");
            ConsoleHelper.PressAnyKeyToContinue();
            return;
        }

        try
        {
            _userService.Register(username, password);
            ConsoleHelper.PrintSuccess($"Account created! Welcome, {username}!");

        }
        catch (Exception e)
        {
            ConsoleHelper.PrintError(e.Message);
        }
        
        ConsoleHelper.PressAnyKeyToContinue();
    }


    public void ShowLogin()
    {
        ConsoleHelper.ClearAndPrintHeader("Log In");
        ConsoleHelper.PrintDivider();
        
        Console.Write("Username: ");
        string username = Console.ReadLine() ?? "";

        string password = ReadMaskedPassword("Password: ");

        try
        {
            _userService.Login(username, password);
            var user = _userService.GetCurrentUser();
            ConsoleHelper.PrintSuccess($"Welcome back, {user!.Username}!");
            ConsoleHelper.PressAnyKeyToContinue();
            new MainMenu(_userService).Show();
        }
        catch (Exception e)
        {
            ConsoleHelper.PrintError(e.Message);
            ConsoleHelper.PressAnyKeyToContinue();
        }
    }

    private string ReadMaskedPassword(string prompt)
    {
        Console.Write(prompt);
        string password = "";

        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }

            if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[..^1];
                Console.Write("\b \b");
            }
            else if (keyInfo.Key != ConsoleKey.Backspace)
            {
                password += keyInfo.KeyChar;
                Console.Write("*");
            }
        }

        return password;
    }
}
