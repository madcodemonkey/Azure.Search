namespace Search.CogServices.UnitTests;

[TestClass]
public class AcmeSearchODataHandlerNumberTests
{
    private const string IndexFieldName = "myField";
    private const string PropFieldName = "MyField";

    [DataTestMethod]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, "55", $"{IndexFieldName} eq 55")]
    [DataRow(AcmeSearchFilterOperatorEnum.NotEqual, "55", $"{IndexFieldName} ne 55")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessOrEqual, "55", $"{IndexFieldName} le 55")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessThan, "55", $"{IndexFieldName} lt 55")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterOrEqual, "55", $"{IndexFieldName} ge 55")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterThan, "55", $"{IndexFieldName} gt 55")]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, null, $"{IndexFieldName} eq null")]
    [DataRow(AcmeSearchFilterOperatorEnum.NotEqual, null, $"{IndexFieldName} ne null")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessOrEqual, null, $"{IndexFieldName} le null")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessThan, null, $"{IndexFieldName} lt null")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterOrEqual, null, $"{IndexFieldName} ge null")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterThan, null, $"{IndexFieldName} gt null")]
    public void CreateFilter_AllOperatorsWorkWithOneValue_FilterCreated(AcmeSearchFilterOperatorEnum theOperator, string theValue, string expectedFilter)
    {
        // Arrange
        var cut = new AcmeSearchODataHandlerNumber();

        var values = new List<string?> { theValue };

        // Act
        var actualResult = cut.CreateFilter(IndexFieldName, theOperator, values);

        // Assert
        Assert.AreEqual(expectedFilter, actualResult);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreateFilter_TheWithinOperatorWillThrowAnExceptionIfOnlyOneValueIsPassedIn_AnExceptionIsThrown()
    {
        // Arrange
        var cut = new AcmeSearchODataHandlerNumber();

        var values = new List<string?> { "34" };

        // Act
        cut.CreateFilter(IndexFieldName, AcmeSearchFilterOperatorEnum.WithinRange, values);

        // Assert
        Assert.Fail($"Passing in one value to a {AcmeSearchFilterOperatorEnum.WithinRange} operator should result in an exception!");
    }

    [DataTestMethod]
    [DataRow("55", "85", $"{IndexFieldName} ge 55 and {IndexFieldName} le 85")]
    [DataRow("55", "85", $"{IndexFieldName} ge 55 and {IndexFieldName} le 85")]
    [DataRow("55", "85", $"{IndexFieldName} ge 55 and {IndexFieldName} le 85")]
    [DataRow("55", "85", $"{IndexFieldName} ge 55 and {IndexFieldName} le 85")]
    [DataRow("55", "85", $"{IndexFieldName} ge 55 and {IndexFieldName} le 85")]
    [DataRow("55", "85", $"{IndexFieldName} ge 55 and {IndexFieldName} le 85")]
    public void CreateFilter_TheWithinOperatorWorksWhenTwoValuesArePassedIn_FilterCreated(string value1, string value2, string expectedFilter)
    {
        // Arrange
        var cut = new AcmeSearchODataHandlerNumber();

        var values = new List<string?> { value1, value2 };

        // Act
        var actualResult = cut.CreateFilter(IndexFieldName, AcmeSearchFilterOperatorEnum.WithinRange, values);

        // Assert
        Assert.AreEqual(expectedFilter, actualResult);
    }
}