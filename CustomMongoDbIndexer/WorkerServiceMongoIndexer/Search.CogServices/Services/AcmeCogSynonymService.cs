using Azure;
using Azure.Search.Documents.Indexes.Models;

namespace Search.CogServices;

public class AcmeCogSynonymService : IAcmeCogSynonymService
{
    private readonly IAcmeCogClientService _clientService;

    /// <summary>Constructor</summary>
    public AcmeCogSynonymService(IAcmeCogClientService clientService)
    {
        _clientService = clientService;
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
        
        var client = _clientService.GetIndexClient();
        await client.CreateSynonymMapAsync(synonymMap);

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

        var client = _clientService.GetIndexClient();
        await client.DeleteSynonymMapAsync(synonymMapName);

        return true;
    }

    /// <summary>Checks to see if a synonym map exists.</summary>
    /// <param name="synonymMapName">Name of the synonym map</param>
    public async Task<bool> ExistsAsync(string synonymMapName)
    {
        var existingMaps = await GetSynonymMapNamesAsync();
        return existingMaps.Any(w => String.Compare(synonymMapName, w, StringComparison.OrdinalIgnoreCase) == 0);
    }

    /// <summary>Gets a list of synonym maps names</summary>
    public async Task<IReadOnlyList<string>> GetSynonymMapNamesAsync()
    {
        var client = _clientService.GetIndexClient();
        Response<IReadOnlyList<string>>? data = await client.GetSynonymMapNamesAsync();
        if (data == null) return new List<string>();

        return data.Value;
    }

    /// <summary>Gets the synonym names within a single synonym map.</summary>
    /// <param name="synonymMapName">Name of the synonym map</param>
    public async Task<List<string>> GetSynonymNamesAsync(string synonymMapName)
    {
        var client = _clientService.GetIndexClient();
        Response<SynonymMap> data = await client.GetSynonymMapAsync(synonymMapName);
        if (data == null) return new List<string>(0);

        string[] items = data.Value.Synonyms.Split('\n');
        var itemList = items.ToList();

        return itemList; //data.Value.Synonyms;
    }
}