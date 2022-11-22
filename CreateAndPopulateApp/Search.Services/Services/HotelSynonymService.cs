﻿using Azure.Search.Documents.Indexes.Models;
using Search.CogServices;
using Search.Model;

namespace Search.Services;

public class HotelSynonymService : IHotelSynonymService
{
    private readonly IAcmeSearchIndexService _indexService;

    public HotelSynonymService(IAcmeSearchIndexService indexService)
    {
        _indexService = indexService;
    }


    public async Task<bool> AssociateSynonymMapToHotelFieldsAsync(string hotelIndexName, string synonymMapName)
    {
        const int maxNumTries = 3;

        for (int i = 0; i < maxNumTries; ++i)
        {
            try
            {
                // Get the index
                SearchIndex index = _indexService.Client.GetIndex(hotelIndexName);
                
                index.Fields.First(f => f.Name == nameof(HotelDocument.Category).ConvertToCamelCase()).SynonymMapNames.Add(synonymMapName);
                index.Fields.First(f => f.Name == nameof(HotelDocument.Tags).ConvertToCamelCase()).SynonymMapNames.Add(synonymMapName);

                // The IfNotChanged condition ensures that the index is updated only if the ETags match.
               await _indexService.Client.CreateOrUpdateIndexAsync(index);

                return true;
            }
            catch
            {
                Console.WriteLine($"Index update failed : . Attempt({i}/{maxNumTries}).\n");
            }
        }

        return false;
    }

}