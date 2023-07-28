using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using IndexHelper.Models;
using SearchServices.Services;

namespace IndexHelper.Services;

public class PersonIndexService : SearchIndexService, IPersonIndexService
{
    private readonly SearchServiceSettings _settings;

    /// <summary>
    /// Constructor
    /// </summary>
    public PersonIndexService(SearchServiceSettings settings, 
        ISearchClientService clientService) : base(clientService)
    {
        _settings = settings;
    }

    /// <summary>
    /// Create index or update the index.
    /// </summary>
    public void CreateOrUpdateIndex()
    {
        FieldBuilder fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(SearchIndexDocument));

        var definition = new SearchIndex(_settings.CognitiveSearchIndexName, searchFields);

        // setup the suggestor
        var suggester = new SearchSuggester("sg", new[]
        {
            nameof(SearchIndexDocument.Title),
            nameof(SearchIndexDocument.Id),
            nameof(SearchIndexDocument.KeyPhrases)
        });
        definition.Suggesters.Add(suggester);

        // Setup Semantic Configuration
        var prioritizedFields = new PrioritizedFields()
        {
            TitleField = new SemanticField()
            {
                FieldName = nameof(SearchIndexDocument.Title)
            }
        };

        prioritizedFields.ContentFields.Add(new SemanticField() { FieldName = nameof(SearchIndexDocument.Content) });
        prioritizedFields.KeywordFields.Add(new SemanticField() { FieldName = nameof(SearchIndexDocument.KeyPhrases) });

        SemanticConfiguration semanticConfig = new SemanticConfiguration(_settings.CognitiveSearchSemanticConfigurationName, prioritizedFields);
        definition.SemanticSettings = new SemanticSettings();
        definition.SemanticSettings.Configurations.Add(semanticConfig);

        // Create it using the index client
        var indexClient = this.ClientService.GetIndexClient();
        indexClient.CreateOrUpdateIndex(definition);
    }
 

    /// <summary>
    /// Upload documents in a single Upload request.
    /// </summary>
    /// <param name="doc"></param>
    public void UploadDocuments(SearchIndexDocument doc)
    {
        IndexDocumentsBatch<SearchIndexDocument> batch = IndexDocumentsBatch.Create(
            IndexDocumentsAction.Upload(doc));

        var searchClient = this.ClientService.GetSearchClient();
        IndexDocumentsResult result = searchClient.IndexDocuments(batch);
    }
 
}