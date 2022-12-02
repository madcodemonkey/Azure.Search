using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
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
    public void BuildODataFilter_WhenFilterDataEntered_TheProperODataFilterIsReturned(
        List<AcmeSearchFilterItem> filters, string expectedFilter)
    {
        // Arrange
        var cut = new HotelFieldService();

        // Act
        string actualFilter = cut.BuildODataFilter(filters, new List<string> { "admin" });

        // Assert
        Assert.AreEqual(expectedFilter, actualFilter);

    }

    public static IEnumerable<object[]> HotelFilteringData
    {
        get
        {
            return new[]
            { 
                // 1st Test
                new object[]
                {
                    // Filters
                    new List<AcmeSearchFilterItem>()
                    {
                        new AcmeSearchFilterItem()
                        {
                            Id = (int) HotelDocumentFieldEnum.Category,
                            Operator = AcmeSearchFilterOperatorEnum.Equal,
                            Values = new List<string> { "Luxury"}
                        }
                    },
                    // Expected OData filter 
                    "category eq 'Luxury' and roles/any(g:search.in(g, 'admin', ','))"
                },

                // 2nd Test
                new object[]
                {
                    // Filters
                    new List<AcmeSearchFilterItem>()
                    {
                        new AcmeSearchFilterItem()
                        {
                            Id = (int) HotelDocumentFieldEnum.Rating,
                            Operator = AcmeSearchFilterOperatorEnum.WithinRange,
                            Values = new List<string> { "1", "5"}
                        }
                    },
                    // Expected OData filter 
                    "rating ge 1 and rating le 5 and roles/any(g:search.in(g, 'admin', ','))"
                },

                // 3rd Test
                new object[]
                {
                    // Filters
                    new List<AcmeSearchFilterItem>()
                    {
                        new AcmeSearchFilterItem()
                        {
                            Id = (int) HotelDocumentFieldEnum.SmokingAllowed,
                            Operator = AcmeSearchFilterOperatorEnum.Equal,
                            Values = new List<string> { "true"}
                        }
                    },
                    // Expected OData filter 
                    "smokingAllowed eq true and roles/any(g:search.in(g, 'admin', ','))"
                },

                // 4th Test
                new object[]
                {
                    // Filters
                    new List<AcmeSearchFilterItem>()
                    {
                        new AcmeSearchFilterItem()
                        {
                            Id = (int) HotelDocumentFieldEnum.BaseRate,
                            Operator = AcmeSearchFilterOperatorEnum.WithinRange,
                            Values = new List<string> { "100.01", "456.43"}
                        },
                        new AcmeSearchFilterItem()
                        {
                            Id = (int) HotelDocumentFieldEnum.Category,
                            Operator = AcmeSearchFilterOperatorEnum.Equal,
                            Values = new List<string> { "Luxury"}
                        },
                    },
                    // Expected OData filter 
                    "baseRate ge 100.01 and baseRate le 456.43 and category eq 'Luxury' and roles/any(g:search.in(g, 'admin', ','))"
                },
            };
        }
    }

}