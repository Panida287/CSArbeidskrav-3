namespace EventPlatform.Models;

/// <summary>
/// Represents a registered user of the platform.
/// </summary>
public class User
{
    /// <summary>Unique identifier for the user.</summary>
    public int UserId { get; set; }

    /// <summary>The user's chosen display name. Must be unique.</summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>BCrypt hash of the user's password. Never store plain text.</summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>Date and time the account was created.</summary>
    public DateTime CreatedAt { get; set; }
}
