namespace EventPlatform.Models.Events;

/// <summary>
/// Represents a conference event. Adds SpeakerList and SessionSchedule.
/// </summary>
public class Conference : Event
{
    public List<string> SpeakerList { get; set; } = new();
    public string SessionSchedule { get; set; } = string.Empty;
}
