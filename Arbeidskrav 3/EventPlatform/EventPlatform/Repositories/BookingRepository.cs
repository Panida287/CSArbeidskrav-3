using Microsoft.Data.Sqlite;
using EventPlatform.Data;
using EventPlatform.Models;
using EventPlatform.Enums;

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
    public int Insert(Booking booking)
    {
        using var connection = _db.GetConnection();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Bookings (UserId, EventId, TicketTypeId, Status, Reference, BookingDate, PriceAtBooking)
            VALUES (@userId, @eventId, @ticketTypeId, @status, @reference, @bookingDate, @priceAtBooking);
            SELECT last_insert_rowid();";

        command.Parameters.AddWithValue("@userId", booking.UserId);
        command.Parameters.AddWithValue("@eventId", booking.EventId);
        command.Parameters.AddWithValue("@ticketTypeId", booking.TicketTypeId);
        command.Parameters.AddWithValue("@status", booking.Status.ToString());
        command.Parameters.AddWithValue("@reference", booking.Reference);
        command.Parameters.AddWithValue("@bookingDate", booking.BookingDate.ToString("o"));
        command.Parameters.AddWithValue("@priceAtBooking", (double)booking.PriceAtBooking);

        var result = command.ExecuteScalar();
        return Convert.ToInt32(result);
    }

    /// <summary>Updates the status of a booking (e.g. Confirmed → Cancelled).</summary>
    public bool UpdateStatus(int bookingId, string status)
    {
        using var connection = _db.GetConnection();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Bookings
            SET Status = @status
            WHERE BookingId = @bookingId;";

        command.Parameters.AddWithValue("@status", status);
        command.Parameters.AddWithValue("@bookingId", bookingId);

        return command.ExecuteNonQuery() > 0;
    }

    /// <summary>Returns all bookings for the given user.</summary>
    public List<Booking> GetByUser(int userId)
    {
        var bookings = new List<Booking>();

        using var connection = _db.GetConnection();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT BookingId, UserId, EventId, TicketTypeId, Status, Reference, BookingDate, PriceAtBooking
            FROM Bookings
            WHERE UserId = @userId;";

        command.Parameters.AddWithValue("@userId", userId);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            bookings.Add(new Booking
            {
                BookingId      = reader.GetInt32(0),
                UserId         = reader.GetInt32(1),
                EventId        = reader.GetInt32(2),
                TicketTypeId   = reader.GetInt32(3),
                Status         = Enum.Parse<BookingStatus>(reader.GetString(4)),
                Reference      = reader.GetString(5),
                BookingDate    = DateTime.Parse(reader.GetString(6)),
                PriceAtBooking = (decimal)reader.GetDouble(7)
            });
        }

        return bookings;
    }

    /// <summary>Decrements remaining ticket count by 1 for the given TicketType.</summary>
    public void DecrementRemaining(int ticketTypeId)
    {
        using var connection = _db.GetConnection();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE TicketTypes
            SET Remaining = Remaining - 1
            WHERE TicketTypeId = @ticketTypeId AND Remaining > 0;";

        command.Parameters.AddWithValue("@ticketTypeId", ticketTypeId);
        command.ExecuteNonQuery();
    }

    /// <summary>Increments remaining ticket count by 1 (used on cancellation).</summary>
    public void IncrementRemaining(int ticketTypeId)
    {
        using var connection = _db.GetConnection();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE TicketTypes
            SET Remaining = Remaining + 1
            WHERE TicketTypeId = @ticketTypeId;";

        command.Parameters.AddWithValue("@ticketTypeId", ticketTypeId);
        command.ExecuteNonQuery();
    }

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
    
    /// <summary>Returns all existing booking references (used to generate the next one).</summary>
    public List<string> GetAllReferences()
    {
        var references = new List<string>();

        using var connection = _db.GetConnection();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Reference FROM Bookings;";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            references.Add(reader.GetString(0));
        }

        return references;
    }
}