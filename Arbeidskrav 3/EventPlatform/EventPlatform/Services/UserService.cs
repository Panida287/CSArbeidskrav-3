using EventPlatform.Models;
using EventPlatform.Repositories;
using EventPlatform.Utilities;

namespace EventPlatform.Services;

/// <summary>
/// Handles user registration, login, logout, and session state.
/// No Console I/O — pure business logic only.
/// </summary>
public class UserService
{
    private User? _currentUser;
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>Registers a new user. Returns false if username is taken.</summary>
    public bool Register(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Username and password cannot be empty.");

        if (_userRepository.Exists(username))
            throw new InvalidOperationException("A user with that username already exists.");

        string hashedPassword = PasswordHelper.Hash(password);
        var user = new User { Username = username, PasswordHash = hashedPassword};
        _userRepository.Insert(user);
        return true;

    }

    /// <summary>Logs in a user. Returns false if credentials are invalid.</summary>
    public bool Login(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Username and password cannot be empty.");

        var user = _userRepository.FindByUsername(username);

        if (user == null || !PasswordHelper.Verify(password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid username or password.");

        _currentUser = user;
        return true;
    }

    /// <summary>Logs out the current user.</summary>
    public void Logout()
    {
        _currentUser = null;
    }

    /// <summary>Returns the currently logged-in user, or null if not logged in.</summary>
    public User? GetCurrentUser() => _currentUser;
}
