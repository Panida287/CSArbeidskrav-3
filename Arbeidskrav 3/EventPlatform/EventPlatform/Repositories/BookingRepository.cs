using Microsoft.Data.Sqlite;
using EventPlatform.Data;
using EventPlatform.Models;

namespace EventPlatform.Repositories;

/// <summary>
/// All SQLite queries for the Bookings and TicketTypes tables.
/// </summary>
public class BookingRepository
{
    private readonly AppDatabase _db;

    public BookingRepository(AppDatabase db)
    {
        _db = db;
    }

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

    /// <summary>Returns all ticket types for a given event.</summary>
    public List<TicketType> GetTicketTypesByEvent(int eventId)
    {
        var tickets = new List<TicketType>();

        using var connection = _db.GetConnection();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT TicketTypeId, EventId, Name, Price, TotalQuantity, Remaining
            FROM TicketTypes
            WHERE EventId = @eventId";
        command.Parameters.AddWithValue("@eventId", eventId);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            tickets.Add(new TicketType
            {
                TicketTypeId  = reader.GetInt32(0),
                EventId       = reader.GetInt32(1),
                Name          = reader.GetString(2),
                Price         = (decimal)reader.GetDouble(3),
                TotalQuantity = reader.GetInt32(4),
                Remaining     = reader.GetInt32(5)
            });
        }

        return tickets;
    }
}