namespace Search.CogServices.UnitTests;

[TestClass]
public class AcmeSearchODataHandlerStringTests
{
    private const string IndexFieldName = "myField";

    [DataTestMethod]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, "bob", $"{IndexFieldName} eq 'bob'")]
    [DataRow(AcmeSearchFilterOperatorEnum.NotEqual, "john", $"{IndexFieldName} ne 'john'")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessOrEqual, "55", $"{IndexFieldName} le '55'")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessThan, "Sam", $"{IndexFieldName} lt 'Sam'")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterOrEqual, " Pop", $"{IndexFieldName} ge ' Pop'")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterThan, "Oranges", $"{IndexFieldName} gt 'Oranges'")]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, null, $"{IndexFieldName} eq null")]
    [DataRow(AcmeSearchFilterOperatorEnum.NotEqual, null, $"{IndexFieldName} ne null")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessOrEqual, null, $"{IndexFieldName} le null")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessThan, null, $"{IndexFieldName} lt null")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterOrEqual, null, $"{IndexFieldName} ge null")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterThan, null, $"{IndexFieldName} gt null")]
    public void CreateFilter_AllOperatorsWorkWithOneValue_FilterCreated(AcmeSearchFilterOperatorEnum theOperator, string theValue, string expectedFilter)
    {
        // Arrange
        var cut = new AcmeSearchODataHandlerString();

        var values = new List<string?> { theValue };

        // Act
        var actualResult = cut.CreateFilter(IndexFieldName, theOperator, values);

        // Assert
        Assert.AreEqual(expectedFilter, actualResult);
    }

    [TestMethod]
    public void CanCreateSearchInFilterWhenSearchFilterOperatorIsEqualAndGivenMoreThanOneValue()
    {
        // Arrange
        var cut = new AcmeSearchODataHandlerString();
        var values = new List<string?> { "bob", "john" };

        // Act
        string actualFilter = cut.CreateFilter(IndexFieldName, AcmeSearchFilterOperatorEnum.Equal, values);

        // Assert
        Assert.AreEqual($"search.in({IndexFieldName}, 'bob,john', ',')", actualFilter);
    }


    [TestMethod]
    public void CanRemoveParenthesisFromInputValues()
    {
        // Arrange
        var cut = new AcmeSearchODataHandlerString();
        var values = new List<string?> { "(Chardonnay)(" };

        // Act
        string actualFilter = cut.CreateFilter(IndexFieldName, AcmeSearchFilterOperatorEnum.Equal, values);

        // Assert
        Assert.AreEqual($"{IndexFieldName} eq 'Chardonnay'", actualFilter);
    }

    [ExpectedException(typeof(ArgumentException))]
    [TestMethod]
    public void CannotCreateFilterUsingTheWithinRangeOperator_ArgumentExceptionIsThrown()
    {
        // Arrange
        var cut = new AcmeSearchODataHandlerString();
        var values = new List<string?> { "true" };
        
        // Act
        cut.CreateFilter(IndexFieldName, AcmeSearchFilterOperatorEnum.WithinRange, values);
    }
}