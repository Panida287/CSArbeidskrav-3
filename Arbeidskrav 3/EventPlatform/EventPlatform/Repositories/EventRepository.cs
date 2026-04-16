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
        command.Parameters.AddWithValue("date", ev.Date);
        command.Parameters.AddWithValue("@venue", ev.Venue);
        command.Parameters.AddWithValue("organiserId", ev.OrganiserId);
        
        var result = command.ExecuteScalar();
        
        return Convert.Toint32(result);
        
    }

    /// <summary>Updates an existing event record.</summary>
    public bool Update(Event ev) => throw new NotImplementedException();

    /// <summary>Updates the status field only.</summary>
    public bool UpdateStatus(int eventId, string status) => throw new NotImplementedException();

    /// <summary>Returns all events, including related TicketTypes (JOIN).</summary>
    public List<Event> GetAll()
    {
        var events = new List<Event>();

        using var connection = new SQLiteConnection("Data Source=events.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Events;
";
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            events.Add(new Event
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                Type = reader.GetString(3),
                Category = reader.Getstring(4),
                Date = reader.Getstring(5),
                Venue = reader.Getstring(6),
                OrganiserId = reader.GetInt32(7);
            }
        }
        
        return events;
    }

    /// <summary>Returns a single event by ID, including TicketTypes, or null.</summary>
    public Event? GetById(int eventId)
    {
        using var connection = new SQLiteConnection("Data Source=events.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Events WHERE Id = @id;
";
        command.Parameters.AddWithValue("@id", eventId);

        using var reader = command.ExecuteReader();
        
        if (!reader.Read()) return null;

        return new Event
        {
            Id = reader.GetInt32(0),
            Title = reader.GetString(1),
            Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
            Type = reader.GetString(3),
            Category = reader.Getstring(4),
            Date = reader.Getstring(5),
            Venue = reader.Getstring(6),
            OrganiserId = reader.GetInt32(7);
        };
    }
}
