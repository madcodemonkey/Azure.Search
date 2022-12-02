using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using Search.CogServices;
using Search.Model;

namespace Search.Services.UnitTests;

[TestClass]
public class HotelFieldServiceTests
{
    /// <summary>Using the field builder double check that the user has setup the fields correctly.</summary>
    [TestMethod]
    public void FieldsAreSetupCorrectly()
    {
        // Arrange
        FieldBuilder fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(HotelDocument));
            
        var cut = new HotelFieldService();

        // Act & Assert
        foreach (SearchField field in searchFields)
        {
            var acmeSearchField = cut.FindByIndexFieldName(field.Name);
            if (acmeSearchField == null) continue;
                
            Assert.AreEqual(acmeSearchField.IsFacetable, field.IsFacetable, $"The {field.Name} field IsFacetable setting is wrong!  The document used to to create the index says it is {field.IsFacetable}!");
            Assert.AreEqual(acmeSearchField.IsSortable, field.IsSortable, $"The {field.Name} field IsSortable setting is wrong!  The document used to to create the index says it is {field.IsSortable}!");
            Assert.AreEqual(acmeSearchField.IsFilterable, field.IsFilterable, $"The {field.Name} field IsFilterable setting is wrong!  The document used to to create the index says it is {field.IsFilterable}!");
        }
    }

    [TestMethod]
    public void OnlyOneSecurityFieldIsDefined()
    {
        // Arrange
        var cut = new HotelFieldService();
        int numberOfSecurityFields = 0;

        // Act
        foreach (IAcmeSearchField acmeSearchField in cut.FieldList)
        {
            if (acmeSearchField.IsSecurityFilter) 
                numberOfSecurityFields++;
        }

        // Assert
        Assert.IsTrue(numberOfSecurityFields < 2);
    }

    [TestMethod]
    [DynamicData(nameof(HotelFilteringData))]
    public void BuildODataFilter_WhenFilterDataEntered_TheProperODataFilterIsReturned(string testCase,
        List<AcmeSearchFilterField> filters, string expectedFilter,
        IDictionary<string, IList<FacetResult>> facets, List<HotelFieldServiceTestsSelectedFacet> expectedSelectedFacets)
    {
        // Arrange
        var cut = new HotelFieldService();

        // Act
        string actualFilter = cut.BuildODataFilter(filters, new List<string?> { "admin" });

        // Assert
        Assert.AreEqual(expectedFilter, actualFilter, $"{testCase} failed!");
    }

    [TestMethod]
    [DynamicData(nameof(HotelFilteringData))]
    public void ConvertFacets_GivenSeveralFilters_FacetsAreSelectedProperly(string testCase,
        List<AcmeSearchFilterField> filters, string expectedFilter,
        IDictionary<string, IList<FacetResult>> facets, List<HotelFieldServiceTestsSelectedFacet> expectedSelectedFacets)
    {
        // Arrange
        // Arrange
        // Arrange
        var cut = new HotelFieldService();

        // Act
        // Act
        // Act
        var actualFacetList = cut.ConvertFacets(facets, filters);

        // Assert
        // Assert
        // Assert
        var expectedFacetsIds = expectedSelectedFacets.Select(w => w.FacetId).Distinct().ToList();
        int actualFacetIdCount = 0;
        foreach (var facet in actualFacetList)
        {
            if (facet.Items.Any(w => w.Selected)) actualFacetIdCount++;
        }

        Assert.AreEqual(expectedFacetsIds.Count, actualFacetIdCount, $"{testCase} failed!");

        foreach (int facetId in expectedFacetsIds)
        {
            var oneFacet = actualFacetList.Single(w => w.Id == facetId);
            var actualSelectedItems = oneFacet.Items.Where(w => w.Selected).ToList();

            Assert.AreEqual(expectedSelectedFacets.Count(w => w.FacetId == facetId), actualSelectedItems.Count, $"{testCase} failed!");

            foreach (AcmeSearchFacetItem oneItem in actualSelectedItems)
            {
                var item = expectedSelectedFacets.FirstOrDefault(w => w.FacetId == facetId && w.ItemName == oneItem.Text);
                Assert.IsNotNull(item, $"{testCase} failed! Unable to find facetId {facetId} and itemName {oneItem.Text}");
            }
        }
    }


    public static IEnumerable<object[]> HotelFilteringData
    {
        get
        { return new[]
            { 
                new object[]
                {
                    "Test case 1",
                    // Filters
                    new List<AcmeSearchFilterField>()
                    {
                        new AcmeSearchFilterField()
                        {
                            PeerOperator = AcmeSearchGroupOperatorEnum.And,
                            FiltersOperator = AcmeSearchGroupOperatorEnum.And,
                            FieldId = (int) HotelDocumentFieldEnum.Category,
                            Filters = new List<AcmeSearchFilterItem>()
                            {
                                new AcmeSearchFilterItem()
                                {
                                    Operator = AcmeSearchFilterOperatorEnum.Equal,
                                    Values = new List<string?> { "Luxury"}
                                }
                            },
                        }
                    },
                    // Expected OData filter 
                    "category eq 'Luxury' and roles/any(g:search.in(g, 'admin', ','))",
                    // Facets returned from Azure Search
                    CreateFacetResultDictionary(),
                    // Expected selected items in the facet list
                    new List<HotelFieldServiceTestsSelectedFacet>(){ new ((int) HotelDocumentFieldEnum.Category, "Luxury")}
                },
                
                new object[]
                {
                    "Test case 2",
                    // Filters
                    new List<AcmeSearchFilterField>()
                    {
                        new AcmeSearchFilterField()
                        {
                            PeerOperator = AcmeSearchGroupOperatorEnum.And,
                            FiltersOperator = AcmeSearchGroupOperatorEnum.Or,
                            FieldId = (int) HotelDocumentFieldEnum.Rating,
                            Filters = new List<AcmeSearchFilterItem>()
                            {
                                new AcmeSearchFilterItem()
                                {
                                    Operator = AcmeSearchFilterOperatorEnum.Equal,
                                    Values = new List<string?> { "1" }
                                },
                                new AcmeSearchFilterItem()
                                {
                                    Operator = AcmeSearchFilterOperatorEnum.Equal,
                                    Values = new List<string?> { "5"}
                                }
                            },
                        }
                    },
                    // Expected OData filter 
                    "(rating eq 1 or rating eq 5) and roles/any(g:search.in(g, 'admin', ','))",
                    // Facets returned from Azure Search
                    CreateFacetResultDictionary(),
                    // Expected selected items in the facet list
                    new List<HotelFieldServiceTestsSelectedFacet>(){ new ((int) HotelDocumentFieldEnum.Rating, "1"), new ((int) HotelDocumentFieldEnum.Rating, "5")}
                },
                
                new object[]
                {
                    "Test case 3",
                    // Filters
                    new List<AcmeSearchFilterField>()
                    {
                        new AcmeSearchFilterField()
                        {
                            PeerOperator = AcmeSearchGroupOperatorEnum.And,
                            FiltersOperator = AcmeSearchGroupOperatorEnum.And,
                            FieldId =  (int) HotelDocumentFieldEnum.SmokingAllowed,
                            Filters = new List<AcmeSearchFilterItem>()
                            {
                                new AcmeSearchFilterItem()
                                {
                                    Operator = AcmeSearchFilterOperatorEnum.Equal,
                                    Values = new List<string?> { "true"}
                                }
                            },
                        }
                    },
                    // Expected OData filter 
                    "smokingAllowed eq true and roles/any(g:search.in(g, 'admin', ','))",
                    // Facets returned from Azure Search
                    CreateFacetResultDictionary(),
                    // Expected selected items in the facet list
                    new List<HotelFieldServiceTestsSelectedFacet>()
                },
                
                new object[]
                {
                    "Test case 4",
                    // Filters
                    new List<AcmeSearchFilterField>()
                    {
                        new AcmeSearchFilterField()
                        {
                            PeerOperator = AcmeSearchGroupOperatorEnum.And,
                            FiltersOperator = AcmeSearchGroupOperatorEnum.And,
                            FieldId = (int) HotelDocumentFieldEnum.BaseRate,
                            Filters = new List<AcmeSearchFilterItem>()
                            {
                                new AcmeSearchFilterItem()
                                {
                                    Operator = AcmeSearchFilterOperatorEnum.WithinRange,
                                    Values = new List<string?> { "100.01", "456.43"}
                                }
                            },
                        },
                        new AcmeSearchFilterField()
                        {
                            PeerOperator = AcmeSearchGroupOperatorEnum.And,
                            FiltersOperator = AcmeSearchGroupOperatorEnum.And,
                            FieldId =  (int) HotelDocumentFieldEnum.Category,
                            Filters = new List<AcmeSearchFilterItem>()
                            {
                                new AcmeSearchFilterItem()
                                {
                                    Operator = AcmeSearchFilterOperatorEnum.Equal,
                                    Values = new List<string?> { "Luxury"}
                                },
                            },
                        }
                    },
                    // Expected OData filter 
                    "baseRate ge 100.01 and baseRate le 456.43 and category eq 'Luxury' and roles/any(g:search.in(g, 'admin', ','))",
                    // Facets returned from Azure Search
                    CreateFacetResultDictionary(),
                    // Expected selected items in the facet list
                    new List<HotelFieldServiceTestsSelectedFacet> { new ((int) HotelDocumentFieldEnum.Category, "Luxury")}

                },
            };
        }
    }

    private static IDictionary<string, IList<FacetResult>> CreateFacetResultDictionary()
    {
        var result = new Dictionary<string, IList<FacetResult>>();

        result.Add("baseRate", new List<FacetResult>()
        {

        });

        result.Add("category", new List<FacetResult>()
        {
            CreateOneFacetResult(23, "Luxury"),
            CreateOneFacetResult(22, "Budget")

        });

        result.Add("rating", new List<FacetResult>()
        {
            CreateOneFacetResult(123, "1"),
            CreateOneFacetResult(12, "2"),
            CreateOneFacetResult(13, "3"),
            CreateOneFacetResult(21, "4"),
            CreateOneFacetResult(43, "5"),

        });

        // 

        return result;
    }

    /// <summary>Creates one facet result</summary>
    /// <param name="count">Number of times the item was found.</param>
    /// <param name="value">Value of the item.</param>
    /// <returns></returns>
    private static FacetResult CreateOneFacetResult(int count, string value)
    {
        var additionalProperties = new Dictionary<string, object> { { "value", value } };
        return SearchModelFactory.FacetResult(count, additionalProperties);
    }
}


public class HotelFieldServiceTestsSelectedFacet
{

    public HotelFieldServiceTestsSelectedFacet(int facetId, string itemName)
    {
        FacetId = facetId;
        ItemName = itemName;
    }

    public int FacetId { get; set; }
    public string ItemName { get; set; }
}
