using EventPlatform.Models.Events;

namespace EventPlatform.Repositories;

/// <summary>
/// All SQLite queries for the Events table (with JOINs to TicketTypes).
/// </summary>
public class EventRepository
{
    /// <summary>Inserts a new event. Returns the new ID.</summary>
    public int Insert(Event ev) => throw new NotImplementedException();

    /// <summary>Updates an existing event record.</summary>
    public bool Update(Event ev) => throw new NotImplementedException();

    /// <summary>Updates the status field only.</summary>
    public bool UpdateStatus(int eventId, string status) => throw new NotImplementedException();

    /// <summary>Returns all events, including related TicketTypes (JOIN).</summary>
    public List<Event> GetAll() => throw new NotImplementedException();

    /// <summary>Returns a single event by ID, including TicketTypes, or null.</summary>
    public Event? GetById(int eventId) => throw new NotImplementedException();
}
