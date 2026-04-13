using EventPlatform.Models.Events;
using EventPlatform.Enums;

namespace EventPlatform.Services;

/// <summary>
/// Handles event creation, editing, cancellation, and retrieval.
/// No Console I/O — pure business logic only.
/// </summary>
public class EventService
{
    /// <summary>Creates a new event owned by the given organiser.</summary>
    public Event Create(Event newEvent) => throw new NotImplementedException();

    /// <summary>Updates an existing event. Only the organiser may call this.</summary>
    public bool Edit(Event updatedEvent, int requestingUserId) => throw new NotImplementedException();

    /// <summary>Cancels an event. Only the organiser may call this.</summary>
    public bool Cancel(int eventId, int requestingUserId) => throw new NotImplementedException();

    /// <summary>Returns all events, optionally filtered by status.</summary>
    public List<Event> GetAll(EventStatus? statusFilter = null) => throw new NotImplementedException();

    /// <summary>Returns a single event by ID, or null if not found.</summary>
    public Event? GetById(int eventId) => throw new NotImplementedException();

    /// <summary>
    /// Returns all upcoming events matching the given keyword.
    /// </summary>
    /// <param name="keyword">Case-insensitive search term.</param>
    /// <returns>Filtered list of upcoming events.</returns>
    public List<Event> SearchEvents(string keyword) => throw new NotImplementedException();
}
