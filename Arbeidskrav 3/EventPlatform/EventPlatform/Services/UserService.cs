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

    /// <summary>
    /// Registers a new user with a hashed password.
    /// Throws if username or password is empty, or if username is already taken.
    /// </summary>
    public void Register(string username, string password)
    {
        username = username.Trim();
        
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Username and password cannot be empty.");

        if (_userRepository.Exists(username))
            throw new InvalidOperationException("A user with that username already exists.");

        string hashedPassword = PasswordHelper.Hash(password);
        var user = new User { Username = username, PasswordHash = hashedPassword, CreatedAt = DateTime.UtcNow};
        _userRepository.Insert(user);

    }

    /// <summary>
    /// Logs in a user and stores them as the current session.
    /// Throws if input is empty or credentials are invalid.
    /// </summary>
    public void Login(string username, string password)
    {
        username = username.Trim();
        
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Username and password cannot be empty.");

        var user = _userRepository.FindByUsername(username);

        if (user == null || !PasswordHelper.Verify(password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid username or password.");

        _currentUser = user;
    }

    /// <summary>Logs out the current user.</summary>
    public void Logout()
    {
        _currentUser = null;
    }

    /// <summary>Returns the currently logged-in user, or null if not logged in.</summary>
    public User? GetCurrentUser() => _currentUser;
}
