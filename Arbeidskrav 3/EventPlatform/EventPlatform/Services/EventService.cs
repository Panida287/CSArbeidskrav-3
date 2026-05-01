using EventPlatform.Models.Events;
using EventPlatform.Enums;
using EventPlatform.Repositories;

namespace EventPlatform.Services;

/// <summary>
/// Handles event creation, editing, cancellation, and retrieval.
/// No Console I/O — pure business logic only.
/// </summary>
public class EventService
{
    private readonly EventRepository _eventRepository;

    public EventService(EventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    /// <summary>Creates a new event owned by the given organiser.</summary>
    public Event Create(Event newEvent)
    {
        if (string.IsNullOrWhiteSpace(newEvent.Title))
            throw new ArgumentException("Title cannot be empty.");

        if (string.IsNullOrWhiteSpace(newEvent.Venue))
            throw new ArgumentException("Venue cannot be empty.");

        if (newEvent.EventDate <= DateTime.UtcNow)
            throw new ArgumentException("Event date must be in the future.");

        if (newEvent.OrganiserId == 0)
            throw new ArgumentException("Event must have an organiser.");

        newEvent.EventId = _eventRepository.Insert(newEvent);
        return newEvent;
    }

    /// <summary>Updates an existing event. Only the organiser may call this.</summary>
    public bool Edit(Event updatedEvent, int requestingUserId)
    {
        var existingEvent = _eventRepository.GetById(updatedEvent.EventId);

        if (existingEvent == null)
            return false;

        if (requestingUserId != existingEvent.OrganiserId)
            return false;

        if (string.IsNullOrWhiteSpace(updatedEvent.Title))
            return false;

        if (string.IsNullOrWhiteSpace(updatedEvent.Venue))
            return false;

        if (updatedEvent.EventDate <= DateTime.UtcNow)
            return false;

        _eventRepository.Update(updatedEvent);

        return true;
    }

    /// <summary>Cancels an event. Only the organiser may call this.</summary>
    public bool Cancel(int eventId, int requestingUserId)
    {
        var existingEvent = _eventRepository.GetById(eventId);

        if (existingEvent == null)
            return false;

        if (requestingUserId != existingEvent.OrganiserId)
            return false;

        _eventRepository.UpdateStatus(eventId, EventStatus.Cancelled);

        return true;
    }

    /// <summary>Returns all events, optionally filtered by status.</summary>
    public List<Event> GetAll(EventStatus? statusFilter = null)
    {
        var events = _eventRepository.GetAll();

        if (statusFilter.HasValue)
            events = events.Where(e => e.Status == statusFilter.Value).ToList();

        return events.OrderBy(e => e.EventDate).ToList();
    }

    /// <summary>Returns a single event by ID, or null if not found.</summary>
    public Event? GetById(int eventId)
    {
        return _eventRepository.GetById(eventId);
    }

    /// <summary>Returns all upcoming events matching the given keyword.</summary>
    public List<Event> SearchEvents(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return new List<Event>();

        keyword = keyword.ToLower();

        return _eventRepository.GetAll()
            .Where(e => e.Title.ToLower().Contains(keyword)
                    || e.Description.ToLower().Contains(keyword)
                    || e.Venue.ToLower().Contains(keyword))
            .OrderBy(e => e.EventDate)
            .ToList();
    }

    /// <summary>Returns all upcoming events in the selected category.</summary>
    public List<Event> FilterByCategory(EventCategory category)
    {
        return GetAll()
            .Where(e => e.Category == category)
            .ToList();
    }

    /// <summary>Returns all upcoming events of the selected type.</summary>
    public List<Event> FilterByType(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            return GetAll();

        return GetAll()
            .Where(e => e.EventType.ToLower() == type.ToLower())
            .ToList();
    }

    /// <summary>Returns all upcoming events matching the selected keyword, category and type.</summary>
    public List<Event> FilterEvents(string keyword, EventCategory? category, string? type)
    {
        var events = SearchEvents(keyword);

        if (category.HasValue)
            events = events.Where(e => e.Category == category.Value).ToList();

        if (!string.IsNullOrWhiteSpace(type))
            events = events.Where(e => e.EventType.ToLower() == type.ToLower()).ToList();

        return events;
    }
}