using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using CogSearchServices.Services;
using IndexHelper.Models;
 

namespace IndexHelper.Services;

/// <summary>
/// Used to create the index (see method below) and search (see the base class) the index.
/// </summary>
public class PersonIndexService : CogSearchIndexService, IPersonIndexService
{
    private readonly CogSearchServiceSettings _cogSettings;
    private readonly IndexAppSettings _indexSettings;

    /// <summary>
    /// Constructor
    /// </summary>
    public PersonIndexService(CogSearchServiceSettings cogSettings,
        IndexAppSettings indexSettings,
        ICogSearchClientService clientService) : base(clientService)
    {
        _cogSettings = cogSettings;
        _indexSettings = indexSettings;
    }

    /// <summary>
    /// Create index or update the index.
    /// </summary>
    public void CreateOrUpdateIndex()
    {
        FieldBuilder fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(PersonIndexDocument));

        var definition = new SearchIndex(_cogSettings.CognitiveSearchIndexName, searchFields);

        // setup the suggestor
        var suggester = new SearchSuggester("sg", new[]
        {
            nameof(PersonIndexDocument.FirstName),
            nameof(PersonIndexDocument.LastName)
        });
        definition.Suggesters.Add(suggester);

        // Setup Semantic Configuration
        var prioritizedFields = new PrioritizedFields()
        {
            TitleField = new SemanticField()
            {
                FieldName = nameof(PersonIndexDocument.Description)
            }
        };

        prioritizedFields.ContentFields.Add(new SemanticField() { FieldName = nameof(PersonIndexDocument.Description) });

        SemanticConfiguration semanticConfig = new SemanticConfiguration(_indexSettings.CognitiveSearchSemanticConfigurationName, prioritizedFields);
        definition.SemanticSettings = new SemanticSettings();
        definition.SemanticSettings.Configurations.Add(semanticConfig);

        // Create it using the index client
        var indexClient = this.ClientService.GetIndexClient();
        indexClient.CreateOrUpdateIndex(definition);
    }
}