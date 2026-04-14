using EventPlatform.Enums;

namespace EventPlatform.Models.Events;

/// <summary>
/// Abstract base class for all event types. Contains shared properties only.
/// No Console I/O or service logic here.
/// </summary>
public abstract class Event
{
    /// <summary>Unique identifier for the event.</summary>
    public int EventId { get; set; }

    /// <summary>ID of the user who created and owns the event.</summary>
    public int OrganiserId { get; set; }

    /// <summary>Display title of the event.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Full description of the event.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>The type of event: Concert, Conference, or Workshop.</summary>
    public string EventType { get; set; } = string.Empty;

    /// <summary>The category this event belongs to, e.g. Music or Technology.</summary>
    public EventCategory Category { get; set; }

    /// <summary>Date and time the event takes place.</summary>
    public DateTime EventDate { get; set; }

    /// <summary>Name or address of the venue.</summary>
    public string Venue { get; set; } = string.Empty;

    /// <summary>Current lifecycle status of the event.</summary>
    public EventStatus Status { get; set; }
}
