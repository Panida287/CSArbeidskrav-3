using EventPlatform.Models;

namespace EventPlatform.Repositories;

/// <summary>
/// All SQLite queries for the Users table.
/// </summary>
public class UserRepository
{
    /// <summary>Inserts a new user. Returns the new ID.</summary>
    public int Insert(User user) => throw new NotImplementedException();

    /// <summary>Finds a user by username, or null if not found.</summary>
    public User? FindByUsername(string username) => throw new NotImplementedException();

    /// <summary>Returns true if a username already exists.</summary>
    public bool Exists(string username) => throw new NotImplementedException();
}
