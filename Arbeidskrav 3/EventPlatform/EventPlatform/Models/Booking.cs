using EventPlatform.Enums;

namespace EventPlatform.Models;

/// <summary>
/// Represents a user's ticket booking for an event.
/// </summary>
public class Booking
{
    /// <summary>Unique identifier for the booking.</summary>
    public int BookingId { get; set; }

    /// <summary>ID of the user who made the booking.</summary>
    public int UserId { get; set; }

    /// <summary>ID of the event that was booked.</summary>
    public int EventId { get; set; }

    /// <summary>ID of the ticket type selected (e.g. Early Bird, VIP).</summary>
    public int TicketTypeId { get; set; }

    /// <summary>The ticket price at the time of booking, in NOK.</summary>
    public decimal PriceAtBooking { get; set; }

    /// <summary>Date and time the booking was made.</summary>
    public DateTime BookingDate { get; set; }

    /// <summary>Current status of the booking.</summary>
    public BookingStatus Status { get; set; }

    /// <summary>Human-readable booking reference, e.g. BK-00142.</summary>
    public string Reference { get; set; } = string.Empty;
}
