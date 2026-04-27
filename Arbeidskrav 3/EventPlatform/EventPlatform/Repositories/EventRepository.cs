using Microsoft.Data.Sqlite;
using EventPlatform.Enums;
using EventPlatform.Models.Events;

namespace EventPlatform.Repositories;

/// <summary>
/// All SQLite queries for the Events table (with JOINs to detail tables).
/// </summary>
public class EventRepository
{
    /// <summary>Inserts a new event. Returns the new ID.</summary>
    public int Insert(Event ev)
    {
        using var connection = new SqliteConnection("Data Source=eventplatform.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Events 
            (Title, Description, EventType, Category, EventDate, Venue, OrganiserId, Status)
            VALUES 
            (@title, @description, @eventType, @category, @eventDate, @venue, @organiserId, @status);
            SELECT last_insert_rowid();
        ";
        command.Parameters.AddWithValue("@title", ev.Title);
        command.Parameters.AddWithValue("@description", ev.Description);
        command.Parameters.AddWithValue("@eventType", ev.EventType);
        command.Parameters.AddWithValue("@category", ev.Category.ToString());
        command.Parameters.AddWithValue("@eventDate", ev.EventDate.ToString("o"));
        command.Parameters.AddWithValue("@venue", ev.Venue);
        command.Parameters.AddWithValue("@organiserId", ev.OrganiserId);
        command.Parameters.AddWithValue("@status", ev.Status.ToString());

        var result = command.ExecuteScalar();

        return Convert.ToInt32(result);
    }

    /// <summary>Updates an existing event record.</summary>
    public bool Update(Event ev)
    {
        using var connection = new SqliteConnection("Data Source=eventplatform.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Events
            SET Title = @title,
                Description = @description,
                EventType = @eventType,
                Category = @category,
                EventDate = @eventDate,
                Venue = @venue
            WHERE EventId = @eventId;
        ";
        command.Parameters.AddWithValue("@title", ev.Title);
        command.Parameters.AddWithValue("@description", ev.Description);
        command.Parameters.AddWithValue("@eventType", ev.EventType);
        command.Parameters.AddWithValue("@category", ev.Category.ToString());
        command.Parameters.AddWithValue("@eventDate", ev.EventDate.ToString("o"));
        command.Parameters.AddWithValue("@venue", ev.Venue);
        command.Parameters.AddWithValue("@eventId", ev.EventId);

        var rowsAffected = command.ExecuteNonQuery();

        return rowsAffected > 0;
    }

    /// <summary>Updates the status field only.</summary>
    public bool UpdateStatus(int eventId, EventStatus status)
    {
        using var connection = new SqliteConnection("Data Source=eventplatform.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Events
            SET Status = @status
            WHERE EventId = @eventId;
        ";
        command.Parameters.AddWithValue("@status", status.ToString());
        command.Parameters.AddWithValue("@eventId", eventId);

        var rowsAffected = command.ExecuteNonQuery();

        return rowsAffected > 0;
    }

    /// <summary>Returns all events, returning the correct subtype for each row.</summary>
    public List<Event> GetAll()
    {
        var events = new List<Event>();

        using var connection = new SqliteConnection("Data Source=eventplatform.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT e.EventId, e.OrganiserId, e.Title, e.Description, e.EventType,
                   e.Category, e.EventDate, e.Venue, e.Status,
                   c.Performer, c.Genre,
                   conf.SessionSchedule,
                   w.RequiredMaterials, w.MaxParticipants
            FROM Events e
            LEFT JOIN Concerts c ON e.EventId = c.EventId
            LEFT JOIN Conferences conf ON e.EventId = conf.EventId
            LEFT JOIN Workshops w ON e.EventId = w.EventId;
        ";

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            events.Add(ReadEvent(reader));
        }

        return events;
    }

    /// <summary>Returns a single event by ID, or null if not found.</summary>
    public Event? GetById(int eventId)
    {
        using var connection = new SqliteConnection("Data Source=eventplatform.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT e.EventId, e.OrganiserId, e.Title, e.Description, e.EventType,
                   e.Category, e.EventDate, e.Venue, e.Status,
                   c.Performer, c.Genre,
                   conf.SessionSchedule,
                   w.RequiredMaterials, w.MaxParticipants
            FROM Events e
            LEFT JOIN Concerts c ON e.EventId = c.EventId
            LEFT JOIN Conferences conf ON e.EventId = conf.EventId
            LEFT JOIN Workshops w ON e.EventId = w.EventId
            WHERE e.EventId = @eventId;
        ";
        command.Parameters.AddWithValue("@eventId", eventId);

        using var reader = command.ExecuteReader();

        if (!reader.Read()) return null;

        return ReadEvent(reader);
    }

    /// <summary>Returns all events for a specific organiser.</summary>
    public List<Event> GetByOrganiser(int userId)
    {
        var events = new List<Event>();

        using var connection = new SqliteConnection("Data Source=eventplatform.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT e.EventId, e.OrganiserId, e.Title, e.Description, e.EventType,
                   e.Category, e.EventDate, e.Venue, e.Status,
                   c.Performer, c.Genre,
                   conf.SessionSchedule,
                   w.RequiredMaterials, w.MaxParticipants
            FROM Events e
            LEFT JOIN Concerts c ON e.EventId = c.EventId
            LEFT JOIN Conferences conf ON e.EventId = conf.EventId
            LEFT JOIN Workshops w ON e.EventId = w.EventId
            WHERE e.OrganiserId = @userId;
        ";
        command.Parameters.AddWithValue("@userId", userId);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            events.Add(ReadEvent(reader));
        }

        return events;
    }

    /// <summary>Returns events that match a keyword in title, description, or venue.</summary>
    public List<Event> MatchKeyword(string keyword)
    {
        var events = new List<Event>();

        using var connection = new SqliteConnection("Data Source=eventplatform.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT e.EventId, e.OrganiserId, e.Title, e.Description, e.EventType,
                   e.Category, e.EventDate, e.Venue, e.Status,
                   c.Performer, c.Genre,
                   conf.SessionSchedule,
                   w.RequiredMaterials, w.MaxParticipants
            FROM Events e
            LEFT JOIN Concerts c ON e.EventId = c.EventId
            LEFT JOIN Conferences conf ON e.EventId = conf.EventId
            LEFT JOIN Workshops w ON e.EventId = w.EventId
            WHERE e.Title LIKE @keyword
               OR e.Description LIKE @keyword
               OR e.Venue LIKE @keyword;
        ";
        command.Parameters.AddWithValue("@keyword", $"%{keyword}%");

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            events.Add(ReadEvent(reader));
        }

        return events;
    }

    /// <summary>
    /// Reads the current row and returns the correct Event subtype.
    /// Column order: 0=EventId, 1=OrganiserId, 2=Title, 3=Description, 4=EventType,
    ///               5=Category, 6=EventDate, 7=Venue, 8=Status,
    ///               9=Performer, 10=Genre, 11=SessionSchedule,
    ///               12=RequiredMaterials, 13=MaxParticipants
    /// </summary>
    private Event ReadEvent(SqliteDataReader reader)
    {
        var eventType = reader.GetString(4);
        var category = Enum.Parse<EventCategory>(reader.GetString(5));
        var eventDate = DateTime.Parse(reader.GetString(6));
        var status = Enum.Parse<EventStatus>(reader.GetString(8));

        Event ev = eventType switch
        {
            "Concert" => new Concert
            {
                Performer = reader.IsDBNull(9) ? "" : reader.GetString(9),
                Genre = reader.IsDBNull(10) ? "" : reader.GetString(10)
            },
            "Conference" => new Conference
            {
                SessionSchedule = reader.IsDBNull(11) ? "" : reader.GetString(11)
            },
            "Workshop" => new Workshop
            {
                RequiredMaterials = reader.IsDBNull(12) ? "" : reader.GetString(12),
                MaxParticipants = reader.IsDBNull(13) ? 0 : reader.GetInt32(13)
            },
            _ => throw new InvalidOperationException($"Unknown event type: {eventType}")
        };

        ev.EventId = reader.GetInt32(0);
        ev.OrganiserId = reader.GetInt32(1);
        ev.Title = reader.GetString(2);
        ev.Description = reader.IsDBNull(3) ? "" : reader.GetString(3);
        ev.EventType = eventType;
        ev.Category = category;
        ev.EventDate = eventDate;
        ev.Venue = reader.GetString(7);
        ev.Status = status;

        return ev;
    }
}