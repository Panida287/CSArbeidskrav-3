using EventPlatform.Services;

namespace EventPlatform.UI.Menus;

/// <summary>
/// Post-login navigation hub. Routes to all other menus.
/// </summary>
public class MainMenu
{
    private readonly UserService _userService;

    public MainMenu(UserService userService)
    {
        _userService = userService;
    }

    public void Show() => throw new NotImplementedException();
}
