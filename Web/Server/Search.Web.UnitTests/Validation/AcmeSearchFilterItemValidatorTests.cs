using Search.CogServices;

namespace Search.Web.UnitTests
{
    [TestClass]
    public class AcmeSearchFilterItemValidatorTests
    {
        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        public void Validate_WhenOperatorOutOfRange_ValidationFails(int theOperator)
        {
            // Arrange
            var cut = new AcmeSearchFilterItemValidator();

            // var testData = new
            var testData = new AcmeSearchFilterItem()
            {
                Operator = (AcmeSearchFilterOperatorEnum)theOperator,
                Values = new List<string?> { "34" }
            };

            // Act
            var actualResult = cut.Validate(testData);

            // Assert
            Assert.IsTrue(actualResult.Errors.Any(w => w.PropertyName == "Operator"));
        }
    }
}