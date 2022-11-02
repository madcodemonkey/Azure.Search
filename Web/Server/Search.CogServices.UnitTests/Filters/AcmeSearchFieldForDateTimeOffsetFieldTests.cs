namespace Search.CogServices.UnitTests;

[TestClass]
public class AcmeSearchFieldForDateTimeOffsetFieldTests
{
    private const string FieldName = "myField";

    [DataTestMethod]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, "05/27/2014", $"{FieldName} eq 2014-05-27T00:00:00.000Z")]
    [DataRow(AcmeSearchFilterOperatorEnum.NotEqual, "05/27/2014", $"{FieldName} ne 2014-05-27T00:00:00.000Z")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessOrEqual, "05/27/2014", $"{FieldName} le 2014-05-27T00:00:00.000Z")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessThan, "05/27/2014", $"{FieldName} lt 2014-05-27T00:00:00.000Z")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterOrEqual, "05/27/2014", $"{FieldName} ge 2014-05-27T00:00:00.000Z")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterThan, "05/27/2014", $"{FieldName} gt 2014-05-27T00:00:00.000Z")]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, null, $"{FieldName} eq null")]
    [DataRow(AcmeSearchFilterOperatorEnum.NotEqual, null, $"{FieldName} ne null")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessOrEqual, null, $"{FieldName} le null")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessThan, null, $"{FieldName} lt null")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterOrEqual, null, $"{FieldName} ge null")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterThan, null, $"{FieldName} gt null")]
    public void CreateFilter_AllOperatorsWorkWithOneValue_FilterCreated(AcmeSearchFilterOperatorEnum theOperator, string theValue, string expectedFilter)
    {
        // Arrange
        var cut = new AcmeSearchFieldForDateTimeOffsetField(1, FieldName, "Display Name", false, false, false, false);

        var values = new List<string?> { theValue };

        // Act
        var actualResult = cut.CreateFilter(theOperator, values);

        // Assert
        Assert.AreEqual(expectedFilter, actualResult);
    }

    [DataTestMethod]
    [DataRow("05/27/2014", "12/25/2014", $"{FieldName} ge 2014-05-27T00:00:00.000Z and {FieldName} le 2014-12-25T00:00:00.000Z")]
    [DataRow("05/27/2014", "12/25/2014", $"{FieldName} ge 2014-05-27T00:00:00.000Z and {FieldName} le 2014-12-25T00:00:00.000Z")]
    [DataRow("05/27/2014", "12/25/2014", $"{FieldName} ge 2014-05-27T00:00:00.000Z and {FieldName} le 2014-12-25T00:00:00.000Z")]
    [DataRow("05/27/2014", "12/25/2014", $"{FieldName} ge 2014-05-27T00:00:00.000Z and {FieldName} le 2014-12-25T00:00:00.000Z")]
    [DataRow("05/27/2014", "12/25/2014", $"{FieldName} ge 2014-05-27T00:00:00.000Z and {FieldName} le 2014-12-25T00:00:00.000Z")]
    [DataRow("05/27/2014", "12/25/2014", $"{FieldName} ge 2014-05-27T00:00:00.000Z and {FieldName} le 2014-12-25T00:00:00.000Z")]
    public void CreateFilter_TheWithinOperatorWorksWhenTwoValuesArePassedIn_FilterCreated(string value1, string value2, string expectedFilter)
    {
        // Arrange
        var cut = new AcmeSearchFieldForDateTimeOffsetField(1, FieldName, "Display Name", false, false, false, false);

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
        var cut = new AcmeSearchFieldForDateTimeOffsetField(1, FieldName, "Display Name", false, false, false, false);

        var values = new List<string?> { "05/27/2014" };

        // Act
        cut.CreateFilter(AcmeSearchFilterOperatorEnum.WithinRange, values);

        // Assert
        Assert.Fail($"Passing in one value to a {AcmeSearchFilterOperatorEnum.WithinRange} operator should result in an exception!");

    }

}