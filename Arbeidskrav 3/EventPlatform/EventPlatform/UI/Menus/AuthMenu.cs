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
            Console.WriteLine("Choose an option: ");

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
    
    
    public void ShowRegister() => throw new NotImplementedException();
    public void ShowLogin() => throw new NotImplementedException();
}
