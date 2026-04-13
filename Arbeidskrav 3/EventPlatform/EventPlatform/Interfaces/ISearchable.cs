namespace EventPlatform.Interfaces;

/// <summary>
/// Contract for entities that can be searched by keyword.
/// </summary>
public interface ISearchable
{
    /// <summary>
    /// Returns true if this entity matches the given keyword.
    /// </summary>
    /// <param name="keyword">Case-insensitive search term.</param>
    bool MatchesKeyword(string keyword);
}
