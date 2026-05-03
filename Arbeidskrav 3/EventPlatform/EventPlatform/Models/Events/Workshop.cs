namespace EventPlatform.Models.Events;

/// <summary>
/// Represents a workshop event. Extends Event with materials and capacity details.
/// </summary>
public class Workshop : Event
{
    /// <summary>Materials or equipment participants need to bring.</summary>
    public string RequiredMaterials { get; set; } = string.Empty;

    /// <summary>Maximum number of participants allowed in the workshop.</summary>
    public int MaxParticipants { get; set; }
}
