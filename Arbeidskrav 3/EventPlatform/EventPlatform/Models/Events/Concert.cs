namespace EventPlatform.Models.Events;

/// <summary>
/// Represents a concert event. Extends Event with performer and genre details.
/// </summary>
public class Concert : Event
{
    /// <summary>Name of the performing artist or band.</summary>
    public string Performer { get; set; } = string.Empty;

    /// <summary>Musical genre of the concert, e.g. Jazz, Classical, Rock.</summary>
    public string Genre { get; set; } = string.Empty;
}
