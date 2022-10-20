using System.Text.Json;
using Azure;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Search.Model;

namespace Search.Services;

public class HotelIndexService : AcmeSearchIndexService, IHotelIndexService
{
    /// <summary>Constructor</summary>
    public HotelIndexService(SearchServiceSettings settings) : base(settings)
    {
    }
    
    /// <summary>Creates or updates an index.</summary>
    public async Task<bool> CreateOrUpdateAsync()
    {
        FieldBuilder fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(Hotel));
        var searchIndex = new SearchIndex(_settings.HotelIndexName, searchFields);

        // This is needed for autocomplete.
        string hotelNameFieldName = JsonNamingPolicy.CamelCase.ConvertName(nameof(Hotel.HotelName));
        string categoryFieldName = JsonNamingPolicy.CamelCase.ConvertName(nameof(Hotel.Category));
        
        var suggester = new SearchSuggester(_settings.HotelSuggestorName, new[] { hotelNameFieldName, categoryFieldName });
        searchIndex.Suggesters.Add(suggester);

        // This is a scoring profile to boost results if used.  
        // We can mark one as default if desired.
        var scoringProfile1 = new ScoringProfile("sp-hotel-name")
        {
            FunctionAggregation = ScoringFunctionAggregation.Sum,
            TextWeights = new TextWeights(new Dictionary<string, double> { { hotelNameFieldName, 5.0 } })
        };

        searchIndex.ScoringProfiles.Add(scoringProfile1);
        
        Response<SearchIndex>? result = await Client.CreateOrUpdateIndexAsync(searchIndex);

        return result != null && result.Value != null;
    }
    
    /// <summary>Deletes the hotel index.</summary>
    public async Task<bool> DeleteAsync()
    {
        return await DeleteAsync(_settings.HotelIndexName);
    }
}