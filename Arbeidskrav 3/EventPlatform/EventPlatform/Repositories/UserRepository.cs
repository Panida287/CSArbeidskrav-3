using Microsoft.Data.Sqlite;
using EventPlatform.Models;

namespace EventPlatform.Repositories;

/// <summary>
/// All SQLite queries for the Users table.
/// </summary>
public class UserRepository
{
    /// <summary>Inserts a new user. Returns the new ID.</summary>
    public int Insert(User user)
    {
        using var connection = new SQLiteConnection("Data Source=events.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Users (Username, PasswordHash)
            VALUES (@username, @passwordHash);

            SELECT last_insert_rowid();
";
        command.Parameters.AddWithValue("@username", user.Username);
        command.Parameters.AddWithValue("@passwordHash", user.PasswordHash);

        var result = command.ExecuteScalar();

        return Convert.ToInt32(result);
    }


    /// <summary>Finds a user by username, or null if not found.</summary>
    public User? FindByUsername(string username)
    {
        using var connection = new SQLiteConnection("Data Source=events.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT Id, Username, PasswordHash
            FROM Users
            WHERE Username = @username;
";
        command.Parameters.AddWithValue("@username", username);

        using var reader = command.ExecuteReader();
        
        if (!reader.Read())
            return null;

        return new User
        {
            Id = reader.GetInt32(0),
            Username = reader.GetString(1),
            PasswordHash = reader.GetString(2)
        };
    }

    /// <summary>Returns true if a username already exists.</summary>
    public bool Exists(string username)
    {
        using var connection = new SQLiteConnection("Data Source=events.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT COUNT(*)
            FROM Users
            WHERE Username = @username;
";
        command.Parameters.AddWithValue("@username", username);

        var result = command.ExecuteScalar();

        return Convert.ToInt32(result) > 0;
    }
