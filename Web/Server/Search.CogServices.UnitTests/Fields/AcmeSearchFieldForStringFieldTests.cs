namespace Search.CogServices.UnitTests;

[TestClass]
public class AcmeSearchFieldForStringFieldTests
{
    private const string IndexFieldName = "myField";
    private const string PropFieldName = "MyField";

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
        var cut = new AcmeSearchFieldForStringField(1, PropFieldName, IndexFieldName, "Display Name", false, false, false, false, false);

        var values = new List<string?> { theValue };

        // Act
        var actualResult = cut.CreateFilter(theOperator, values);

        // Assert
        Assert.AreEqual(expectedFilter, actualResult);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreateFilter_TheWithinOperatorWillThrowAnExceptionIfOnlyOneValueIsPassedIn_AnExceptionIsThrown()
    {
        // Arrange
        var cut = new AcmeSearchFieldForNumberField(1, PropFieldName, IndexFieldName, "Display Name", false, false, false, false, false);

        var values = new List<string?> { "James" };

        // Act
        cut.CreateFilter(AcmeSearchFilterOperatorEnum.WithinRange, values);

        // Assert
        Assert.Fail($"Passing in one value to a {AcmeSearchFilterOperatorEnum.WithinRange} operator should result in an exception!");
    }
}