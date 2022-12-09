using Azure.Search.Documents.Models;
using Search.Model;

namespace Search.Services.UnitTests;

[TestClass]
public class HotelSearchHighlightServiceTests : AzureCognitiveSearchTestBase
{
    private readonly IHotelFieldService _fieldService = new HotelFieldService();

    [DataTestMethod]
    [DataRow(50, "string one. string two. string three.")]
    [DataRow(50, "string one. string two. string three.")]
    public async Task MapHighlightsOnToDocumentGoogleStyle_OutputsTheSpecifiedNumberOfCharacters_DoesNotExceedTheCount(
        int maxNumberOfCharacters, string expectedString)
    {
        // Arrange
        var cut = CreateClassUnderTest();

        var documentAndHighlights = new List<TestSearchDocument<HotelDocument>>();
        var oneItem = new TestSearchDocument<HotelDocument>()
        {
            Document = new HotelDocument
            {
                Description = "original content"
            },
            Highlights = new Dictionary<string, IList<string>>()
        };
        oneItem.Highlights.Add("description", new List<string>
        {
            "string one",
            "string two.",
            "string three."
        });

        documentAndHighlights.Add(oneItem);

        SearchResults<HotelDocument> azureSearchResults = CreateAzureSearchResults(documentAndHighlights);
        var docList = await ConvertDocumentsAsync(azureSearchResults);

        // Act
        cut.MapHighlightsOnToDocumentGoogleStyle(docList, maxNumberOfCharacters);

        // Assert
        Assert.AreEqual(1, docList.Count);
        Assert.AreEqual(expectedString, docList[0].Document.Description);
    }

    private HotelSearchHighlightService CreateClassUnderTest()
    {
        return new HotelSearchHighlightService(_fieldService);
    }
}