namespace EventPlatform.Models.Events;

/// <summary>
/// Represents a conference event. Extends Event with speaker and session details.
/// </summary>
public class Conference : Event
{
    /// <summary>List of speaker names appearing at the conference.</summary>
    public List<string> SpeakerList { get; set; } = new();

    /// <summary>Description or schedule of the conference sessions.</summary>
    public string SessionSchedule { get; set; } = string.Empty;
}
