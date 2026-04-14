namespace EventPlatform.Enums;

/// <summary>
/// Lifecycle status of an event.
/// </summary>
public enum EventStatus
{
    /// <summary>The event is scheduled and has not yet taken place.</summary>
    Upcoming,

    /// <summary>The event has taken place.</summary>
    Completed,

    /// <summary>The event was cancelled by the organiser.</summary>
    Cancelled
}
