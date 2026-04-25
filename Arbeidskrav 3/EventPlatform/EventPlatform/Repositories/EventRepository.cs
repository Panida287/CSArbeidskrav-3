using Microsoft.Data.Sqlite;
using EventPlatform.Models.Events;

namespace EventPlatform.Repositories;

/// <summary>
/// All SQLite queries for the Events table (with JOINs to TicketTypes).
/// </summary>
public class EventRepository
{
    /// <summary>Inserts a new event. Returns the new ID.</summary>
    public int Insert(Event ev)
    {
        using var connection = new SQLiteConnection("Data Source=events.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
    INSERT INTO EVENTS 
    (Title, Description, Type, Category, Date, Venue, OrganiserID)
    VALUES 
    (@title, @description, @type, @category, @date, @venue, @organiserID);
    SELECT last_insert_rowid();
    ";
        command.Parameters.AddWithValue("@title", ev.Title);
        command.Parameters.AddWithValue("@description", ev.Description);
        command.Parameters.AddWithValue("@type", ev.Type);
        command.Parameters.AddWithValue("@category", ev.Category);
        command.Parameters.AddWithValue("@date", ev.Date);
        command.Parameters.AddWithValue("@venue", ev.Venue);
        command.Parameters.AddWithValue("@organiserId", ev.OrganiserId);
        
        var result = command.ExecuteScalar();
        
        return Convert.ToInt32(result);
        
    }

    /// <summary>Updates an existing event record.</summary>
    public bool Update(Event ev)
    {
        using var connection = new SQLiteConnection("Data Source=events.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Events
            SET Title = @title,
            Description = @description,
            Type = @type,
            Category = @category,
            Date = @date,
            Venue = @venue
            WHERE Id = @id;
";
        command.Parameters.AddWithValue("@title", ev.Title);
        command.Parameters.AddWithValue("@description", ev.Description);
        command.Parameters.AddWithValue("@type", ev.Type);
        command.Parameters.AddWithValue("@category", ev.Category);
        command.Parameters.AddWithValue("@date", ev.Date);
        command.Parameters.AddWithValue("@venue", ev.Venue);
        command.Parameters.AddWithValue("@id", ev.Id);

        var rowsAffected = command.ExecuteNonQuery();

        return rowsAffected > 0;
        

    }

    /// <summary>Updates the status field only.</summary>
    public bool UpdateStatus(int eventId, string status)
    {
        using var connection = new SQLiteConnection("Data Source=events.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Events
            SET Status = @status
            WHERE Id = @id;
";
        command.Parameters.AddWithValue("@status", status);
        command.Parameters.AddWithValue("@id", eventId);

        var rowsAffected = command.ExecuteNonQuery();

        return rowsAffected > 0;
    }

    /// <summary>Returns all events, including related TicketTypes (JOIN).</summary>
    public List<Event> GetAll()
    {
        var events = new List<Event>();

        using var connection = new SQLiteConnection("Data Source=events.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT e.*,
                   c.ConcertId,
                   conf.ConferanceId,
                   w.WorkshopId
            FROM Events e
            LEFT JOIN Concert c ON e.Id = c.EventId
            LEFT JOIN Conferance conf ON e.Id = conf.EventId
            LEFT JOIN Workshop w ON e.Id = w.EventId;
";
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            if (!reader.IsDBNull(8))
            {
                events.Add(new Concert()
                {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                Type = reader.GetString(3),
                Category = reader.GetString(4),
                Date = reader.GetString(5),
                Venue = reader.GetString(6),
                OrganiserId = reader.GetInt32(7)
            });
        }
            else if (!reader.IsDBNull(9))
            {
                events.Add(new Conference()
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    Type = reader.GetString(3),
                    Category = reader.GetString(4),
                    Date = reader.GetString(5),
                    Venue = reader.GetString(6),
                    OrganiserId = reader.GetInt32(7)
                });
            }
            else if (!reader.IsDBNull(10))
            {
                events.Add(new Workshop()
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    Type = reader.GetString(3),
                    Category = reader.GetString(4),
                    Date = reader.GetString(5),
                    Venue = reader.GetString(6),
                    OrganiserId = reader.GetInt32(7)
                });
            }

            return events;
    }

    /// <summary>Returns a single event by ID, including TicketTypes, or null.</summary>
    public Event? GetById(int eventId)
    {
        using var connection = new SQLiteConnection("Data Source=events.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "@
            SELECT
            e.*,
            c.ConcertId,
            conf.ConferanceId,
            w.WorkshopId 
                FROM Events e
        LEFT JOIN Concert c ON e.Id = c.EventId
        LEFT JOIN Conferance conf ON e.Id = conf.EventId
        LEFT JOIN Workshop w ON e.Id = w.EventId; 
        ";
            
        command.Parameters.AddWithValue("@id", eventId);

        using var reader = command.ExecuteReader();
        
        if (!reader.Read())
            return null;
        
        if (!reader.IsDBNull(8))
        {
        return new Concert
        {
            Id = reader.GetInt32(0),
            Title = reader.GetString(1),
            Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
            Type = reader.GetString(3),
            Category = reader.GetString(4),
            Date = reader.GetString(5),
            Venue = reader.GetString(6),
            OrganiserId = reader.GetInt32(7)
        };
    }
        if (!reader.IsDBNull(9))
        {
            return new Conference
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                Type = reader.GetString(3),
                Category = reader.GetString(4),
                Date = reader.GetString(5),
                Venue = reader.GetString(6),
                OrganiserId = reader.GetInt32(7)
            });
        }
        if (!reader.IsDBNull(10))
        {
            return new Workshop
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                Type = reader.GetString(3),
                Category = reader.GetString(4),
                Date = reader.GetString(5),
                Venue = reader.GetString(6),
                OrganiserId = reader.GetInt32(7)
            });
        }
        
        return new Event
        {
            d = reader.GetInt32(0),
            Title = reader.GetString(1),
            Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
            Type = reader.GetString(3),
            Category = reader.GetString(4),
            Date = reader.GetString(5),
            Venue = reader.GetString(6),
            OrganiserId = reader.GetInt32(7)
        }

    /// <summary>Returns all events for a specific organiser, filtered by OrganiserId.</summary>
    public List<Event> GetByOrganiser(int userId)
    {
        var events = new List<Event>();

        using var connection = new SQLiteConnection("Data Source=events.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Events
            WHERE OrganiserId = @userId;
";
        command.Parameters.AddWithValue("@userId", userId);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            events.Add(new Event
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                Type = reader.GetString(3),
                Category = reader.GetString(4),
                Date = reader.GetString(5),
                Venue = reader.GetString(6),
                OrganiserId = reader.GetInt32(7)
            });
        }
        return events;
    }

    /// <summary>Returns events that match a keyword in title, description, or venue.</summary>
    public List<Event> MatchKeyword(string keyword)
    {
        var events = new List<Event>();

        using var connection = new SQLiteConnection("Data Source=events.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Events
            WHERE Title LIKE @keyword
            OR Description LIKE @keyword
            OR Venue LIKE @keyword;
";
        command.Parameters.AddWithValue("@keyword", $"%{keyword}%");

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            events.Add(new Event
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                Type = reader.GetString(3),
                Category = reader.GetString(4),
                Date = reader.GetString(5),
                Venue = reader.GetString(6),
                OrganiserId = reader.GetInt32(7)
            });
        }
        return events;
    }
}
