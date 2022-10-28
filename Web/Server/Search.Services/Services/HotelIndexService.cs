using System.Text.Json;
using Azure;
using Azure.Core.Serialization;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Search.CogServices;
using Search.Model;

namespace Search.Services;

public class HotelIndexService : AcmeSearchIndexService, IHotelIndexService
{
    private SearchServiceSettings _searchSettings;

    /// <summary>Constructor</summary>
    public HotelIndexService(SearchServiceSettings settings) : base(settings)
    {
        _searchSettings = settings;
    }

    
    /// <summary>Creates or updates an index.</summary>
    public async Task<bool> CreateOrUpdateAsync()
    {
        FieldBuilder fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(HotelDocument));
        var searchIndex = new SearchIndex(_searchSettings.Hotel.IndexName, searchFields);

        // This is needed for autocomplete.
        string hotelNameFieldName = JsonNamingPolicy.CamelCase.ConvertName(nameof(HotelDocument.HotelName));
        string categoryFieldName = JsonNamingPolicy.CamelCase.ConvertName(nameof(HotelDocument.Category));
        
        var suggester = new SearchSuggester(_searchSettings.Hotel.SuggestorName, new[] { hotelNameFieldName, categoryFieldName });
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
        return await DeleteAsync(_searchSettings.Hotel.IndexName);
    }

    /// <summary>Create search options</summary>
    protected override SearchClientOptions CreateSearchClientOptions()
    {
        // This is needed to avoid an error when uploading data that has a GeographyPoint property.  
        // Here is the error: The request is invalid. Details: parameters : Cannot find nested property 'location' on the resource type 'search.documentFields'.
        JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            Converters =
            {
                // Requires Microsoft.Azure.Core.Spatial NuGet package.
                new MicrosoftSpatialGeoJsonConverter()
            },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return new SearchClientOptions
        {
            Serializer = new JsonObjectSerializer(serializerOptions)
        };
    }
}