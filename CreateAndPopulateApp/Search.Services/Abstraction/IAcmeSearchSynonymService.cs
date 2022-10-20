namespace Search.Services;

public interface IAcmeSearchSynonymService
{
    /// <summary>Gets a list of synonym maps names</summary>
    Task<IReadOnlyList<string>> GetSynonymMapNamesAsync();

    /// <summary>Gets the synonym names within a single synonym map.</summary>
    /// <param name="synonymMapName">Name of the synonym map</param>
    Task<List<string>> GetSynonymNamesAsync(string synonymMapName);

    /// <summary>Creates a synonym map.</summary>
    /// <param name="synonymMapName">Name of the new synonym map</param>
    /// <param name="synonyms">A list of new line delimited items ("hotel, motel\ninternet,wifi\nfive star=>luxury\neconomy,inexpensive=>budget")</param>
    Task<bool> CreateAsync(string synonymMapName, string synonyms);

    /// <summary>Deletes one synonym map</summary>
    /// <param name="synonymMapName">Name of the existing synonym map</param>
    Task<bool> DeleteAsync(string synonymMapName);

    /// <summary>Checks to see if a synonym map exists.</summary>
    /// <param name="synonymMapName">Name of the synonym map</param>
    Task<bool> ExistsAsync(string synonymMapName);

}