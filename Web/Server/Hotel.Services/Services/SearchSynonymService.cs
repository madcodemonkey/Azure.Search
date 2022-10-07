using Azure;
using Azure.Search.Documents.Indexes.Models;

namespace Hotel.Services;

public class SearchSynonymService : ISearchSynonymService
{
    private readonly ISearchIndexService _indexService;

    /// <summary>Constructor</summary>
    /// <param name="indexService"></param>
    public SearchSynonymService(ISearchIndexService indexService)
    {
        _indexService = indexService;
    }

    /// <summary>Gets a list of synonym maps names</summary>
    public async Task<IReadOnlyList<string>> GetSynonymMapNamesAsync()
    {
        Response<IReadOnlyList<string>>? data =  await _indexService.Client.GetSynonymMapNamesAsync();
        if (data == null) return new List<string>();

        return data.Value;
    }

    /// <summary>Gets the synonym names within a single synonym map.</summary>
    /// <param name="synonymMapName">Name of the synonym map</param>
    public async Task<List<string>> GetSynonymNamesAsync(string synonymMapName)
    {
        Response<SynonymMap> data =  await _indexService.Client.GetSynonymMapAsync(synonymMapName);
        if (data == null) return new List<string>(0);

        string[] items = data.Value.Synonyms.Split('\n');
        var itemList = items.ToList();

        return itemList; //data.Value.Synonyms;
    }

    /// <summary>Checks to see if a synonym map exists.</summary>
    /// <param name="synonymMapName">Name of the synonym map</param>
    public async Task<bool> ExistsAsync(string synonymMapName)
    {
       var existingMaps = await GetSynonymMapNamesAsync();
       return existingMaps.Any(w => String.Compare(synonymMapName, w, StringComparison.OrdinalIgnoreCase) == 0);
    }

    /// <summary>Creates a synonym map.</summary>
    /// <param name="synonymMapName">Name of the new synonym map</param>
    /// <param name="synonyms">A list of new line delimited items ("hotel, motel\ninternet,wifi\nfive star=>luxury\neconomy,inexpensive=>budget")</param>
    public async Task<bool> CreateAsync(string synonymMapName, string synonyms)
    {
        if (await ExistsAsync(synonymMapName))
        {
            return false;
        }
        
        SynonymMap synonymMap = new SynonymMap(synonymMapName, synonyms);
        
        await _indexService.Client.CreateSynonymMapAsync(synonymMap); 

        return true;
    }

    /// <summary>Deletes one synonym map</summary>
    /// <param name="synonymMapName">Name of the existing synonym map</param>
    public async Task<bool> DeleteAsync(string synonymMapName)
    {
        if (await ExistsAsync(synonymMapName) == false)
        {
            return false;
        }

        await _indexService.Client.DeleteSynonymMapAsync(synonymMapName);

        return true;
    }
}