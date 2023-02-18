namespace Search.CogServices.UnitTests;

[TestClass]
public class AcmeSearchODataHandlerBooleanTests
{
    private const string IndexFieldName = "myField";

    [DataTestMethod]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, "true", $"{IndexFieldName} eq true")]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, "TRUE", $"{IndexFieldName} eq true")]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, " true", $"{IndexFieldName} eq true")]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, "false", $"{IndexFieldName} eq false")]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, "FALSE", $"{IndexFieldName} eq false")]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, " false", $"{IndexFieldName} eq false")]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, null, $"{IndexFieldName} eq null")]
    [DataRow(AcmeSearchFilterOperatorEnum.NotEqual, "true", $"{IndexFieldName} ne true")]
    [DataRow(AcmeSearchFilterOperatorEnum.NotEqual, "false", $"{IndexFieldName} ne false")]
    [DataRow(AcmeSearchFilterOperatorEnum.NotEqual, null, $"{IndexFieldName} ne null")]
    public void CreateFilter_EqualAndNotEqualFiltersWork_FilterCreated(AcmeSearchFilterOperatorEnum theOperator, string theValue, string expectedFilter)
    {
        // Arrange
        var cut = new AcmeSearchODataHandlerBoolean();

        var values = new List<string?> { theValue };

        // Act
        var actualResult = cut.CreateFilter(IndexFieldName, theOperator, values);

        // Assert
        Assert.AreEqual(expectedFilter, actualResult);
    }

    [DataTestMethod]
    [DataRow(AcmeSearchFilterOperatorEnum.LessOrEqual)]
    [DataRow(AcmeSearchFilterOperatorEnum.LessThan)]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterOrEqual)]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterThan)]
    [DataRow(AcmeSearchFilterOperatorEnum.WithinRange)]
    public void CreateFilter_OtherOperatorsDoNotBuildFilters_AnExceptionIsRaised(AcmeSearchFilterOperatorEnum theOperator)
    {
        // Arrange
        var cut = new AcmeSearchODataHandlerBoolean();

        var values = new List<string?> { "true" };

        // Act
        Exception ex = Assert.ThrowsException<ArgumentException>(() => cut.CreateFilter(IndexFieldName, theOperator, values));

        // Assert
        if (ex == null)
            Assert.Fail($"An exception should have been generated for the '{theOperator}'!");

        Assert.IsInstanceOfType(ex, typeof(ArgumentException));
    }
}