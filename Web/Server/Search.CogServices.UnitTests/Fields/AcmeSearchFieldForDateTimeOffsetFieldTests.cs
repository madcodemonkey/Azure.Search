namespace Search.CogServices.UnitTests;

[TestClass]
public class AcmeSearchFieldForDateTimeOffsetFieldTests
{
    private const string IndexFieldName = "myField";
    private const string PropFieldName = "MyField";

    [DataTestMethod]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, "05/27/2014", $"{IndexFieldName} eq 2014-05-27T00:00:00.000Z")]
    [DataRow(AcmeSearchFilterOperatorEnum.NotEqual, "05/27/2014", $"{IndexFieldName} ne 2014-05-27T00:00:00.000Z")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessOrEqual, "05/27/2014", $"{IndexFieldName} le 2014-05-27T00:00:00.000Z")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessThan, "05/27/2014", $"{IndexFieldName} lt 2014-05-27T00:00:00.000Z")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterOrEqual, "05/27/2014", $"{IndexFieldName} ge 2014-05-27T00:00:00.000Z")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterThan, "05/27/2014", $"{IndexFieldName} gt 2014-05-27T00:00:00.000Z")]
    [DataRow(AcmeSearchFilterOperatorEnum.Equal, null, $"{IndexFieldName} eq null")]
    [DataRow(AcmeSearchFilterOperatorEnum.NotEqual, null, $"{IndexFieldName} ne null")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessOrEqual, null, $"{IndexFieldName} le null")]
    [DataRow(AcmeSearchFilterOperatorEnum.LessThan, null, $"{IndexFieldName} lt null")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterOrEqual, null, $"{IndexFieldName} ge null")]
    [DataRow(AcmeSearchFilterOperatorEnum.GreaterThan, null, $"{IndexFieldName} gt null")]
    public void CreateFilter_AllOperatorsWorkWithOneValue_FilterCreated(AcmeSearchFilterOperatorEnum theOperator, string theValue, string expectedFilter)
    {
        // Arrange
        var cut = new AcmeSearchFieldForDateTimeOffsetField(1, PropFieldName, IndexFieldName, "Display Name", false, false, false, false, false);

        var values = new List<string?> { theValue };

        // Act
        var actualResult = cut.CreateFilter(theOperator, values);

        // Assert
        Assert.AreEqual(expectedFilter, actualResult);
    }

    [DataTestMethod]
    [DataRow("05/27/2014", "12/25/2014", $"{IndexFieldName} ge 2014-05-27T00:00:00.000Z and {IndexFieldName} le 2014-12-25T23:59:59.000Z")]
    [DataRow("05/27/2014", "12/25/2014", $"{IndexFieldName} ge 2014-05-27T00:00:00.000Z and {IndexFieldName} le 2014-12-25T23:59:59.000Z")]
    [DataRow("05/27/2014", "12/25/2014", $"{IndexFieldName} ge 2014-05-27T00:00:00.000Z and {IndexFieldName} le 2014-12-25T23:59:59.000Z")]
    [DataRow("05/27/2014", "12/25/2014", $"{IndexFieldName} ge 2014-05-27T00:00:00.000Z and {IndexFieldName} le 2014-12-25T23:59:59.000Z")]
    [DataRow("05/27/2014", "12/25/2014", $"{IndexFieldName} ge 2014-05-27T00:00:00.000Z and {IndexFieldName} le 2014-12-25T23:59:59.000Z")]
    [DataRow("05/27/2014", "12/25/2014", $"{IndexFieldName} ge 2014-05-27T00:00:00.000Z and {IndexFieldName} le 2014-12-25T23:59:59.000Z")]
    public void CreateFilter_TheWithinOperatorWorksWhenTwoValuesArePassedIn_FilterCreated(string value1, string value2, string expectedFilter)
    {
        // Arrange
        var cut = new AcmeSearchFieldForDateTimeOffsetField(1, PropFieldName, IndexFieldName, "Display Name", false, false, false, false, false);

        var values = new List<string?> { value1, value2 };

        // Act
        var actualResult = cut.CreateFilter(AcmeSearchFilterOperatorEnum.WithinRange, values);

        // Assert
        Assert.AreEqual(expectedFilter, actualResult);
    }

    
    [TestMethod]
    public void CreateFilter_ExactDateAndTimePassedIn_TurnedIntoUtcTime()
    {
        // Arrange
        var cut = new AcmeSearchFieldForDateTimeOffsetField(1, PropFieldName, IndexFieldName, "Display Name", false, false, false, false, false);

        var values = new List<string?> { "05/27/2014 14:23:12" };

        // Act
        var actualResult = cut.CreateFilter(AcmeSearchFilterOperatorEnum.Equal, values);

        // Assert
        Assert.AreEqual($"{IndexFieldName} eq 2014-05-27T14:23:12.000Z", actualResult);

    }



    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CreateFilter_TheWithinOperatorWillThrowAnExceptionIfOnlyOneValueIsPassedIn_AnExceptionIsThrown()
    {
        // Arrange
        var cut = new AcmeSearchFieldForDateTimeOffsetField(1, PropFieldName, IndexFieldName, "Display Name", false, false, false, false, false);

        var values = new List<string?> { "05/27/2014" };

        // Act
        cut.CreateFilter(AcmeSearchFilterOperatorEnum.WithinRange, values);

        // Assert
        Assert.Fail($"Passing in one value to a {AcmeSearchFilterOperatorEnum.WithinRange} operator should result in an exception!");

    }

}