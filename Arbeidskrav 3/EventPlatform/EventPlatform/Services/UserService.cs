using EventPlatform.Models;

namespace EventPlatform.Services;

/// <summary>
/// Handles user registration, login, logout, and session state.
/// No Console I/O — pure business logic only.
/// </summary>
public class UserService
{
    private User? _currentUser;

    /// <summary>Registers a new user. Returns false if username is taken.</summary>
    public bool Register(string username, string password) => throw new NotImplementedException();

    /// <summary>Logs in a user. Returns false if credentials are invalid.</summary>
    public bool Login(string username, string password) => throw new NotImplementedException();

    /// <summary>Logs out the current user.</summary>
    public void Logout() => throw new NotImplementedException();

    /// <summary>Returns the currently logged-in user, or null if not logged in.</summary>
    public User? GetCurrentUser() => _currentUser;
}
