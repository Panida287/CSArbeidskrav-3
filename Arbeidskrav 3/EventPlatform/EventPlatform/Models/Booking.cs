using EventPlatform.Enums;

namespace EventPlatform.Models;

/// <summary>
/// Represents a user's ticket booking for an event.
/// </summary>
public class Booking
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int EventId { get; set; }
    public int TicketTypeId { get; set; }
    public BookingStatus Status { get; set; }
    public DateTime BookedAt { get; set; }
    public string Reference { get; set; } = string.Empty;
}
