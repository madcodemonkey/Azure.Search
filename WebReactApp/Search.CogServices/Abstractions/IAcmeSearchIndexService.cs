using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;

namespace Search.CogServices;

public interface IAcmeSearchIndexService
{
    SearchIndexClient Client { get; }

    /// <summary>Performs an autocomplete lookup.</summary>
    /// <param name="indexName">The name of the index</param>
    /// <param name="searchText">The partial bit of text to search upon</param>
    /// <param name="suggesterName">The name of the suggester</param>
    /// <param name="options">Options that allow specifying autocomplete behaviors, like fuzzy matching.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to propagate notifications that the operation should be canceled. </param>
    Task<Response<AutocompleteResults>> AutocompleteAsync(string indexName, string searchText, string suggesterName,
        AutocompleteOptions? options = null, CancellationToken cancellationToken = default);
  
    /// <summary>Deletes an index.</summary>
    /// <param name="indexName">The name of the index to delete</param>
    Task<bool> DeleteAsync(string indexName);

    /// <summary>Retrieves a single document.</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="indexName">The name of the index</param>
    /// <param name="key">The documents key</param>
    Task<Response<T>?> GetDocumentAsync<T>(string indexName, string key);
    
    /// <summary>Returns a list of index names.</summary>
    Task<List<string>> GetIndexNamesAsync();

    /// <summary>Performs a search against the index.</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="indexName">The name of the index</param>
    /// <param name="searchText">The text to find</param>
    /// <param name="options">The search options to apply</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to propagate notifications that the operation should be canceled. </param>
    Task<Response<SearchResults<T>>> SearchAsync<T>(string indexName, string searchText, 
           SearchOptions? options = null, CancellationToken cancellationToken = default);
    
    /// <summary>Used for autocomplete to get a suggestion.</summary>
    /// <typeparam name="T">The type of data being returned.</typeparam>
    /// <param name="indexName">The name of the index</param>
    /// <param name="searchText">The text to find</param>
    /// <param name="suggesterName">The name of the suggestor</param>
    /// <param name="options">The search options to apply</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken"/> to propagate notifications that the operation should be canceled. </param>
    Task<SuggestResults<T>> SuggestAsync<T>(string indexName, string searchText, string suggesterName,
        SuggestOptions? options = null, CancellationToken cancellationToken = default);
    
    /// <summary>Uploads documents to an index.</summary>
    /// <typeparam name="T">The class type that we are uploading.</typeparam>
    /// <param name="indexName">The name of the index</param>
    /// <param name="uploadList">The list of items of type T to upload.</param>
    Task UploadDocuments<T>(string indexName, List<T> uploadList);
}