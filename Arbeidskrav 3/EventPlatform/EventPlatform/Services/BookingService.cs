using EventPlatform.Models;

namespace EventPlatform.Services;

/// <summary>
/// Handles ticket booking, cancellation, and retrieval.
/// No Console I/O — pure business logic only.
/// </summary>
public class BookingService
{
    /// <summary>Books a ticket for the user. Returns null on failure (sold out, own event, etc.).</summary>
    public Booking? BookTicket(int userId, int eventId, int ticketTypeId) => throw new NotImplementedException();

    /// <summary>Cancels an existing booking. Returns false if not allowed.</summary>
    public bool CancelBooking(int bookingId, int requestingUserId) => throw new NotImplementedException();

    /// <summary>Returns all bookings for a given user.</summary>
    public List<Booking> GetUserBookings(int userId) => throw new NotImplementedException();
}
