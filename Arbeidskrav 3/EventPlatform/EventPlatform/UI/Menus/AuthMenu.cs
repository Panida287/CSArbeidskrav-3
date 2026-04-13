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

    public void Show() => throw new NotImplementedException();
    public void ShowRegister() => throw new NotImplementedException();
    public void ShowLogin() => throw new NotImplementedException();
}
