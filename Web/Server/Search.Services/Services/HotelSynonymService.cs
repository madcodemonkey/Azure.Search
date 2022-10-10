using System.Text.Json;
using Azure.Search.Documents.Indexes.Models;
using Search.Model;

namespace Search.Services;

public class HotelSynonymService : SearchSynonymService, IHotelSynonymService
{
    private readonly SearchServiceSettings _searchServiceSettings;

    /// <summary>Constructor</summary>
    public HotelSynonymService(ISearchIndexService indexService, SearchServiceSettings searchServiceSettings) : base(indexService)
    {
        _searchServiceSettings = searchServiceSettings;
    }

    /// <summary>Creates all the synonym lists used by the hotel index.</summary>
    public async Task<string> CreateAsync()
    {
        if (await ExistsAsync(_searchServiceSettings.SearchSynonymMapName))
            return $"{_searchServiceSettings.SearchSynonymMapName} has already been created.";

        // Note that each synonym group is new line delimited!
        // Docs to understand equivalency: USA, United States, United States of America
        // https://learn.microsoft.com/en-us/azure/search/search-synonyms#equivalency-rules
        // Docs to understand explicit mapping (substitute all the words on the left with one on right):  Washington, Wash., WA => WA
        // https://learn.microsoft.com/en-us/azure/search/search-synonyms#explicit-mapping
        await CreateAsync(_searchServiceSettings.SearchSynonymMapName,
            "hotel, motel\ninternet,wifi\nfive star=>luxury\neconomy,inexpensive=>budget");
        
        return $"{_searchServiceSettings.SearchSynonymMapName} created.";
    }


    public async Task<bool> AssociateSynonymMapToHotelFieldsAsync(string hotelIndexName, string synonymMapName)
    {
        int MaxNumTries = 3;

        for (int i = 0; i < MaxNumTries; ++i)
        {
            try
            {
                // Get the index
                SearchIndex index = _indexService.Client.GetIndex(hotelIndexName);

                string categoryFieldName = JsonNamingPolicy.CamelCase.ConvertName(nameof(Hotel.Category));
                index.Fields.First(f => f.Name == categoryFieldName).SynonymMapNames.Add(synonymMapName);

                string tagsFieldName = JsonNamingPolicy.CamelCase.ConvertName(nameof(Hotel.Tags));
                index.Fields.First(f => f.Name == tagsFieldName).SynonymMapNames.Add(synonymMapName);

                // The IfNotChanged condition ensures that the index is updated only if the ETags match.
               await _indexService.Client.CreateOrUpdateIndexAsync(index);

                return true;
            }
            catch
            {
                Console.WriteLine($"Index update failed : . Attempt({i}/{MaxNumTries}).\n");
            }
        }

        return false;
    }

}