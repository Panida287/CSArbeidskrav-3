namespace EventPlatform.Interfaces;

/// <summary>
/// Contract for entities that can be searched by keyword.
/// </summary>
public interface ISearchable
{
    /// <summary>
    /// Returns true if this entity matches the given keyword.
    /// Search is case-insensitive and checks title, description, and venue.
    /// </summary>
    /// <param name="keyword">Case-insensitive search term.</param>
    bool MatchesKeyword(string keyword);
}
