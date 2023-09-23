using System.Text;

namespace Search.CogServices.UnitTests;

[TestClass]
public class StringBuilderExtensionsTests
{
    [DataTestMethod]
    [DataRow("test", "", "test")]
    [DataRow("test", null, "test")]
    [DataRow(null, "2", "2")]
    [DataRow("test", "2", "2test")]
    [DataRow("", "2", "2")]
    public void Prepend_GivenDifferentStrings_CanAppendToTheFrontOfTheString(
        string initialValue, string addThisString, string expectedValue)
    {
        // Arrange
        var cut = new StringBuilder(initialValue);

        // Act
        cut.Prepend(addThisString);

        // Assert
        Assert.AreEqual(expectedValue, cut.ToString());
    }

    [DataTestMethod]
    [DataRow("test", "(test)")]
    [DataRow(null, "()")]
    [DataRow("", "()")]
    public void SurroundWithParenthesis_GivenDifferentStrings_CanAppendToTheFrontOfTheString(
        string initialValue, string expectedValue)
    {
        // Arrange
        var cut = new StringBuilder(initialValue);

        // Act
        cut.SurroundWithParenthesis();

        // Assert
        Assert.AreEqual(expectedValue, cut.ToString());
    }
}