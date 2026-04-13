namespace EventPlatform.Models;

/// <summary>
/// Represents a user review for an event they attended.
/// </summary>
public class Review
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public int UserId { get; set; }
    public int EventId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
