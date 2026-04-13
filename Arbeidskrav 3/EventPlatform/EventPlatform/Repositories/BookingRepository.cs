using EventPlatform.Models;

namespace EventPlatform.Repositories;

/// <summary>
/// All SQLite queries for the Bookings table.
/// </summary>
public class BookingRepository
{
    /// <summary>Inserts a new booking. Returns the new ID.</summary>
    public int Insert(Booking booking) => throw new NotImplementedException();

    /// <summary>Updates the status of a booking (e.g. Confirmed → Cancelled).</summary>
    public bool UpdateStatus(int bookingId, string status) => throw new NotImplementedException();

    /// <summary>Returns all bookings for the given user.</summary>
    public List<Booking> GetByUser(int userId) => throw new NotImplementedException();

    /// <summary>Decrements remaining ticket count by 1 for the given TicketType.</summary>
    public void DecrementRemaining(int ticketTypeId) => throw new NotImplementedException();

    /// <summary>Increments remaining ticket count by 1 (used on cancellation).</summary>
    public void IncrementRemaining(int ticketTypeId) => throw new NotImplementedException();
}
