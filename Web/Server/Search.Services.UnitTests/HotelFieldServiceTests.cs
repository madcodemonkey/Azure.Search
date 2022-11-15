using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Search.CogServices;
using Search.Model;

namespace Search.Services.UnitTests
{
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
    }
}