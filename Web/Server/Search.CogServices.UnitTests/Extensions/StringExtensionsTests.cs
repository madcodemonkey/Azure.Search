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

    [DataTestMethod]
    [DataRow("i see you", "I see you")]
    [DataRow(" i see you", " i see you")]
    [DataRow(null, null)]
    public void FirstLetterToUpper_GivenDifferentStrings_CanCapitalizeTheFirsCharacter(string input, string expectedValue)
    {
        // Act
        var actualValue = input.FirstLetterToUpper();

        // Assert
        Assert.AreEqual(expectedValue, actualValue);
    }

    [DataTestMethod]
    [DataRow("todayILiveIn1234%TheUSAWithSimon14Old", "today I Live In 1234% The USA With Simon 14 Old", " ")]
    [DataRow("todayILiveIn1234%TheUSAWithSimon14Old", "today-I-Live-In-1234%-The-USA-With-Simon-14-Old", "-")]
    [DataRow("tEstThis", "t Est This", " ")]
    [DataRow("TEstThis", "T Est This", " ")]
    [DataRow(null, null, " ")]
    public void SplitOnCamelCasing_GivenDifferentStrings_CanSplitOnCamelCase(string input, string expectedValue, string delimiter)
    {
        // Act
        var actualValue = input.SplitOnCamelCasing(delimiter);

        // Assert
        Assert.AreEqual(expectedValue, actualValue);
    }
}