namespace Search.CogServices.UnitTests;

[TestClass]
public class AcmeSearchODataHandlerChildObjectStringTests
{
    private const string IndexFieldName = "authors/name";

    [DataTestMethod]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, "James", "authors/any(c: c/name eq 'James')")]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, "Thomas", "authors/any(c: c/name eq 'Thomas')")]
    public void CreateFilter_EqualAndNotEqualFiltersWork_FilterCreated(AcmeSearchFilterOperatorEnum theOperator, string theValue, string expectedFilter)
    {
        // Arrange
        var cut = new AcmeSearchODataHandlerChildObjectString();

        var values = new List<string?> { theValue };

        // Act
        var actualResult = cut.CreateFilter(IndexFieldName, theOperator, values);

        // Assert
        Assert.AreEqual(expectedFilter, actualResult);
    }

    [DataTestMethod]
    [DataRow(AcmeSearchFilterOperatorEnum.NotEqual)]
    [DataRow(AcmeSearchFilterOperatorEnum.LessOrEqual)]
    [DataRow(AcmeSearchFilterOperatorEnum.LessThan)]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterOrEqual)]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterThan)]
    [DataRow(AcmeSearchFilterOperatorEnum.WithinRange)]
    public void CreateFilter_OtherOperatorsDoNotBuildFilters_AnExceptionIsRaised(AcmeSearchFilterOperatorEnum theOperator)
    {
        // Arrange
        var cut = new AcmeSearchODataHandlerChildObjectString();

        var values = new List<string?> { "true" };

        // Act
        Exception ex = Assert.ThrowsException<ArgumentException>(() => cut.CreateFilter(IndexFieldName, theOperator, values));

        // Assert
        if (ex == null)
            Assert.Fail($"An exception should have been generated for the '{theOperator}'!");

        Assert.IsInstanceOfType(ex, typeof(ArgumentException));
    }
}