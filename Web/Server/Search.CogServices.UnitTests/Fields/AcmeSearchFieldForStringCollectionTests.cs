namespace Search.CogServices.UnitTests;

[TestClass]
public class AcmeSearchFieldForStringCollectionTests
{
    private const string IndexFieldName = "myField";
    private const string PropFieldName = "MyField";

    [DataTestMethod]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, "1,21,3", $"{IndexFieldName}/any(g:search.in(g, '1,21,3', ','))")]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, "1, 21, 3", $"{IndexFieldName}/any(g:search.in(g, '1, 21, 3', ','))")]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, "7,8,9", $"{IndexFieldName}/any(g:search.in(g, '7,8,9', ','))")]
    public void CreateFilter_EqualAndNotEqualFiltersWork_FilterCreated(AcmeSearchFilterOperatorEnum theOperator, string theValue, string expectedFilter)
    {
        // Arrange
        var cut = new AcmeSearchFieldForStringCollection(1, PropFieldName, IndexFieldName, "Display Name", false, false, false, false, false);

        var values = new List<string?> { theValue };

        // Act
        var actualResult = cut.CreateFilter(theOperator, values);

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
        var cut = new AcmeSearchFieldForStringCollection(1, PropFieldName, IndexFieldName, "Display Name", false, false, false, false, false);

        var values = new List<string?> { "true" };

        // Act
        Exception ex = Assert.ThrowsException<ArgumentException>(() => cut.CreateFilter(theOperator, values));

        // Assert
        if (ex == null)
            Assert.Fail($"An exception should have been generated for the '{theOperator}'!");

        Assert.IsInstanceOfType(ex, typeof(ArgumentException));
    }
}