namespace EventPlatform.Models.Events;

/// <summary>
/// Represents a workshop event. Adds RequiredMaterials and MaxParticipants.
/// </summary>
public class Workshop : Event
{
    public string RequiredMaterials { get; set; } = string.Empty;
    public int MaxParticipants { get; set; }
}
