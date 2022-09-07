using Azure.Search.Documents.Indexes.Models;

namespace Search.Services;

public class HotelSynonymService : IHotelSynonymService
{
    private readonly ISearchIndexService _indexService;

    public HotelSynonymService(ISearchIndexService indexService)
    {
        _indexService = indexService;
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

                index.Fields.First(f => f.Name == "category").SynonymMapNames.Add(synonymMapName);
                index.Fields.First(f => f.Name == "tags").SynonymMapNames.Add(synonymMapName);

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