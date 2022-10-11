using System.Text.Json;
using Azure;
using Azure.Core.Serialization;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using Search.Model;

namespace Search.Services;

public class SearchIndexService : ISearchIndexService
{
    private readonly SearchServiceSettings _settings;
    private SearchIndexClient? _client;
    private readonly SearchClientOptions _clientOptions;

    /// <summary>Constructor</summary>
    public SearchIndexService(SearchServiceSettings settings)
    {
        _settings = settings;
        _clientOptions =  CreateSearchClientOptions();
    }
    
    /// <summary>This is the Microsoft client that does all the work.</summary>
    public SearchIndexClient Client => _client ??= new SearchIndexClient(
        new Uri(_settings.SearchEndPoint), new AzureKeyCredential(_settings.SearchAdminApiKey), _clientOptions);

    /// <summary>Creates or updates an index.</summary>
    /// <param name="typeToCreate">The class type that represents the index.  This POCO will be decorated with Azure Search attributes
    /// indicating how the field can be used.</param>
    /// <param name="indexName">The name you want to give the index.</param>
    public async Task<bool> CreateOrUpdateAsync(Type typeToCreate, string indexName)
    {
        FieldBuilder fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeToCreate);
        var searchIndex = new SearchIndex(indexName, searchFields);

        
        // This is needed for autocomplete.
        var suggester = new SearchSuggester("sg", new[] { nameof(Hotel.HotelName).ConvertToCamelCase(), nameof(Hotel.Category).ConvertToCamelCase() });
        searchIndex.Suggesters.Add(suggester);

        // This is a scoring profile to boost results if used.  
        // We can mark one as default if desired.
        var scoringProfile1 = new ScoringProfile("sp-hotel-name")
        {
            FunctionAggregation = ScoringFunctionAggregation.Sum,
            TextWeights = new TextWeights(new Dictionary<string, double> { {  nameof(Hotel.HotelName).ConvertToCamelCase(), 5.0 } })
        };

        searchIndex.ScoringProfiles.Add(scoringProfile1);
        
        Response<SearchIndex>? result = await Client.CreateOrUpdateIndexAsync(searchIndex);

        return result != null && result.Value != null;
    }

    /// <summary>Deletes an index.</summary>
    public async Task<bool> DeleteAsync(string indexName)
    {
        if (await ExistsAsync(indexName) == false)
            return true; // it does not exists

        await Client.DeleteIndexAsync(indexName);

        return true;
    }

    /// <summary>Retrieves a single document.</summary>
    /// <param name="indexName">The name of the index</param>
    /// <param name="searchText">The partial bit of text to search upon</param>
    /// <param name="suggesterName">The name of the suggester</param>
    public async Task<Response<AutocompleteResults>> AutocompleteAsync(string indexName, string searchText, string suggesterName)
    {
        var searchClient = Client.GetSearchClient(indexName);
        return await searchClient.AutocompleteAsync(searchText, suggesterName);
    }

    /// <summary>Retrieves a single document.</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="indexName">The name of the index</param>
    /// <param name="key">The documents key</param>
    public async Task<Response<T>?> GetDocumentAsync<T>(string indexName, string key)
    {
        try
        {
            var searchClient = Client.GetSearchClient(indexName);
            return await searchClient.GetDocumentAsync<T>(key);
        }
        catch (RequestFailedException ex)
        {
            if (ex.Status == 404)
              return null;
            throw;
        }
    }

    /// <summary>Indicates if an index exists.</summary>
    /// <param name="indexName">The name of the index to find.</param>
    /// <remarks>Unfortunately, we get this response as an exception from the API,
    /// so we have to check for the HTTP status code of 404 to determine if it was really missing or if there was
    /// some type of other error</remarks>
    public async Task<bool> ExistsAsync(string indexName)
    {
        try
        {
            return await Client.GetIndexAsync(indexName) != null;
        }
        catch (RequestFailedException e) when (e.Status == 404)
        {
            // if exception occurred and status is "Not Found", this is working as expected
            // because someone was too lazy to put in an exist query.
            return false;
        }
    }

    /// <summary>Returns a list of index names.</summary>
    public async Task<List<string>> GetIndexNamesAsync()
    {
        var result = new List<string>();
        AsyncPageable<string>? pages = Client.GetIndexNamesAsync();

        await foreach (Page<string> onePage in pages.AsPages())
        {
            foreach (string oneIndexName in onePage.Values)
            {
                result.Add(oneIndexName);
            }
        }

        return result;
    }

    /// <summary>Performs a search against the index.</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="indexName">The name of the index</param>
    /// <param name="searchText">The text to find</param>
    /// <param name="options">The search options to apply</param>
    public async Task<Response<SearchResults<T>>> Search<T>(string indexName, string searchText, SearchOptions? options = null)
    {
        var searchClient = Client.GetSearchClient(indexName);
        return await searchClient.SearchAsync<T>(searchText, options);
    }

    /// <summary>Uploads documents to an index.</summary>
    /// <typeparam name="T">The class type that we are uploading.</typeparam>
    /// <param name="indexName">The name of the index</param>
    /// <param name="uploadList">The list of items of type T to upload.</param>
    public async Task UploadDocuments<T>(string indexName, List<T> uploadList)
    {
        if (uploadList.Count == 0) return;

        // Turn the uploadLIst into an array of Upload Actions
        IndexDocumentsAction<T>[] actions = uploadList.Select(s => IndexDocumentsAction.Upload(s)).ToArray();

        // Create a back of actions
        IndexDocumentsBatch<T> batch = IndexDocumentsBatch.Create(actions);

        SearchClient searchClient = Client.GetSearchClient(indexName);

        IndexDocumentsResult result = await searchClient.IndexDocumentsAsync(batch);
    }

    /// <summary>Create search options</summary>
    private SearchClientOptions CreateSearchClientOptions()
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