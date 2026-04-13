namespace EventPlatform.Models.Events;

/// <summary>
/// Represents a concert event. Adds Performer and Genre.
/// </summary>
public class Concert : Event
{
    public string Performer { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
}
