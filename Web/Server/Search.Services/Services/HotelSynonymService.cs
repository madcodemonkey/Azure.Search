using Azure.Search.Documents.Indexes.Models;
using Search.Model;

namespace Search.Services;

public class HotelSynonymService : AcmeSearchSynonymService, IHotelSynonymService
{
    private readonly SearchServiceSettings _searchServiceSettings;

    /// <summary>Constructor</summary>
    public HotelSynonymService(IAcmeSearchIndexService indexService, SearchServiceSettings searchServiceSettings) : base(indexService)
    {
        _searchServiceSettings = searchServiceSettings;
    }

    /// <summary>Creates all the synonym lists used by the hotel index.</summary>
    public async Task<string> CreateAsync()
    {
        if (await ExistsAsync(_searchServiceSettings.HotelSynonymMapName))
            return $"{_searchServiceSettings.HotelSynonymMapName} has already been created.";

        // Note that each synonym group is new line delimited!
        // Docs to understand equivalency: USA, United States, United States of America
        // https://learn.microsoft.com/en-us/azure/search/search-synonyms#equivalency-rules
        // Docs to understand explicit mapping (substitute all the words on the left with one on right):  Washington, Wash., WA => WA
        // https://learn.microsoft.com/en-us/azure/search/search-synonyms#explicit-mapping
        await CreateAsync(_searchServiceSettings.HotelSynonymMapName,
            "hotel, motel\ninternet,wifi\nfive star=>luxury\neconomy,inexpensive=>budget");

        return $"{_searchServiceSettings.HotelSynonymMapName} created.";
    }

    /// <summary>Deletes hotel synonym map</summary>
    public async Task<bool> DeleteAsync()
    {
        return await DeleteAsync(_searchServiceSettings.HotelSynonymMapName);
    }

    /// <summary>Associates a synonym map with certain fields on the Hotel Index</summary>
    public async Task AssociateSynonymMapToHotelFieldsAsync()
    {
        const int maxNumTries = 3;

        for (int i = 0; i < maxNumTries; ++i)
        {
            try
            {
                // Get the index
                SearchIndex index = await IndexService.Client.GetIndexAsync(_searchServiceSettings.HotelIndexName);

                AddSynonymToField(index, nameof(SearchHotel.Category).ConvertToCamelCase(), _searchServiceSettings.HotelSynonymMapName);
                AddSynonymToField(index, nameof(SearchHotel.Tags).ConvertToCamelCase(), _searchServiceSettings.HotelSynonymMapName);

                // The IfNotChanged condition ensures that the index is updated only if the ETags match.
                await IndexService.Client.CreateOrUpdateIndexAsync(index);

                break;
            }
            catch
            {
                if (i == (maxNumTries - 1))
                    throw;
            }
        }
    }

    /// <summary>Adds a synonym to a field if it isn't already there.</summary>
    private void AddSynonymToField(SearchIndex index, string fieldName, string synonymMapName)
    {
        var field = index.Fields.First(f => f.Name == fieldName);
        if (field == null) throw new ArgumentException($"Field {fieldName} does not exist!");

        // Only add it if it wasn't already there; otherwise, you will get an exception for adding a synonym twice.
        if (field.SynonymMapNames.Any(w => w == synonymMapName) == false)
        {
            field.SynonymMapNames.Add(synonymMapName);
        }
    }
}