namespace Search.CogServices.UnitTests;

[TestClass]
public class StringExtensionsTests
{
    [DataTestMethod]
    [DataRow("TestThis", "testThis")]
    [DataRow("TEstThis", "tEstThis")]
    [DataRow("testThis", "testThis")]
    public void ConvertToCamelCase_GivenDifferentStrings_CanConvertToCamelCase(string input, string expectedValue)
    {
        // Act
        var actualValue = input.ConvertToCamelCase();

        // Assert
        Assert.AreEqual(expectedValue, actualValue);
    }
}