using Azure.Search.Documents.Models;

namespace Search.CogServices.UnitTests;

[TestClass]
public class SearchDocumentExtensionsTests
{
    // TODO: Cannot map over an existing field

    // TODO: Specifying source field that does not exist does not generate an error

    // TODO: can remap fields

    // TODO: remove: can remove field

    // TODO: remove: ignores fields that do not exist   (inline data)

    public SearchDocument CreateClassUnderTests()
    {
        return new SearchDocument()
        {
            { "HotelName", "Bobs luxury hotel" },
            { "City", "Seattle" },
            { "State", "WA" }
        };
    }
}