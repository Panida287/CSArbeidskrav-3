namespace EventPlatform.Models;

/// <summary>
/// Represents a user review left for an event they attended.
/// </summary>
public class Review
{
    /// <summary>Unique identifier for the review.</summary>
    public int ReviewId { get; set; }

    /// <summary>ID of the booking that qualifies the user to leave this review.</summary>
    public int BookingId { get; set; }

    /// <summary>ID of the user who wrote the review.</summary>
    public int UserId { get; set; }

    /// <summary>ID of the event being reviewed.</summary>
    public int EventId { get; set; }

    /// <summary>Star rating from 1 to 5.</summary>
    public int Rating { get; set; }

    /// <summary>Optional written comment. May be empty.</summary>
    public string Comment { get; set; } = string.Empty;
}
