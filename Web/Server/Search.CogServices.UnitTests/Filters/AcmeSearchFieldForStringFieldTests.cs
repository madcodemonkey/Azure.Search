namespace Search.CogServices.UnitTests;

[TestClass]
public class AcmeSearchFieldForStringFieldTests
{
    private const string FieldName = "myField";

    [DataTestMethod]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, "bob", $"{FieldName} eq 'bob'")]
    [DataRow(AcmeSearchFilterOperatorEnum.NotEqual, "john", $"{FieldName} ne 'john'")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessOrEqual, "55", $"{FieldName} le '55'")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessThan, "Sam", $"{FieldName} lt 'Sam'")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterOrEqual, " Pop", $"{FieldName} ge ' Pop'")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterThan, "Oranges", $"{FieldName} gt 'Oranges'")]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, null, $"{FieldName} eq null")]
    [DataRow(AcmeSearchFilterOperatorEnum.NotEqual, null, $"{FieldName} ne null")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessOrEqual, null, $"{FieldName} le null")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessThan, null, $"{FieldName} lt null")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterOrEqual, null, $"{FieldName} ge null")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterThan, null, $"{FieldName} gt null")]
    public void CreateFilter_AllOperatorsWorkWithOneValue_FilterCreated(AcmeSearchFilterOperatorEnum theOperator, string theValue, string expectedFilter)
    {
        // Arrange
        var cut = new AcmeSearchFieldForStringField(1, FieldName, "Display Name", false, false, false, false);

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
        var cut = new AcmeSearchFieldForNumberField(1, FieldName, "Display Name", false, false, false, false);

        var values = new List<string?> { "James" };

        // Act
        cut.CreateFilter(AcmeSearchFilterOperatorEnum.WithinRange, values);

        // Assert
        Assert.Fail($"Passing in one value to a {AcmeSearchFilterOperatorEnum.WithinRange} operator should result in an exception!");
    }
}