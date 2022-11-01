namespace Search.CogServices.UnitTests;

[TestClass]
public class AcmeSearchFilterForNumberFieldTests
{
    private const string FieldName = "myField";

    [DataTestMethod]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, "55", $"{FieldName} eq 55")]
    [DataRow(AcmeSearchFilterOperatorEnum.NotEqual, "55", $"{FieldName} ne 55")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessOrEqual, "55", $"{FieldName} le 55")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessThan, "55", $"{FieldName} lt 55")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterOrEqual, "55", $"{FieldName} ge 55")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterThan, "55", $"{FieldName} gt 55")]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, null, $"{FieldName} eq null")]
    [DataRow(AcmeSearchFilterOperatorEnum.NotEqual, null, $"{FieldName} ne null")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessOrEqual, null, $"{FieldName} le null")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessThan, null, $"{FieldName} lt null")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterOrEqual, null, $"{FieldName} ge null")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterThan, null, $"{FieldName} gt null")]
    public void CreateFilter_AllOperatorsWorkWithOneValue_FilterCreated(AcmeSearchFilterOperatorEnum theOperator, string theValue, string expectedFilter)
    {
        // Arrange
        var cut = new AcmeSearchFilterForNumberField(1, FieldName, "Display Name", false, false);

        var values = new List<string?> { theValue };

        // Act
        var actualResult = cut.CreateFilter(theOperator, values);

        // Assert
        Assert.AreEqual(expectedFilter, actualResult);
    }

    [DataTestMethod]
    [DataRow("55", "85", $"{FieldName} ge 55 and {FieldName} le 85")]
    [DataRow("55", "85", $"{FieldName} ge 55 and {FieldName} le 85")]
    [DataRow("55", "85", $"{FieldName} ge 55 and {FieldName} le 85")]
    [DataRow("55", "85", $"{FieldName} ge 55 and {FieldName} le 85")]
    [DataRow("55", "85", $"{FieldName} ge 55 and {FieldName} le 85")]
    [DataRow("55", "85", $"{FieldName} ge 55 and {FieldName} le 85")]
    public void CreateFilter_TheWithinOperatorWorksWhenTwoValuesArePassedIn_FilterCreated(string value1, string value2, string expectedFilter)
    {
        // Arrange
        var cut = new AcmeSearchFilterForNumberField(1, FieldName, "Display Name", false, false);

        var values = new List<string?> { value1, value2 };

        // Act
        var actualResult = cut.CreateFilter(AcmeSearchFilterOperatorEnum.WithinRange, values);

        // Assert
        Assert.AreEqual(expectedFilter, actualResult);
    }



    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreateFilter_TheWithinOperatorWillThrowAnExceptionIfOnlyOneValueIsPassedIn_AnExceptionIsThrown()
    {
        // Arrange
        var cut = new AcmeSearchFilterForNumberField(1, FieldName, "Display Name", false, false);

        var values = new List<string?> { "34" };

        // Act
        cut.CreateFilter(AcmeSearchFilterOperatorEnum.WithinRange, values);

        // Assert
        Assert.Fail($"Passing in one value to a {AcmeSearchFilterOperatorEnum.WithinRange} operator should result in an exception!");

    }
}