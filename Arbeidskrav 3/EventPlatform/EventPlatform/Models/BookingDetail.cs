using EventPlatform.Enums;

namespace EventPlatform.Models;

/// <summary>
/// A booking with joined event and ticket details for display purposes.
/// </summary>
public class BookingDetail
{
    public int BookingId { get; set; }
    public string Reference { get; set; } = string.Empty;
    public BookingStatus Status { get; set; }
    public decimal PriceAtBooking { get; set; }
    public DateTime BookingDate { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string TicketName { get; set; } = string.Empty;
}