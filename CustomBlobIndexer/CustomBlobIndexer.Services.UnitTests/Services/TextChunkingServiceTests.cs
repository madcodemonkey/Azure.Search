namespace CustomBlobIndexer.Services.UnitTests;

[TestClass]
public class TextChunkingServiceTests
{
    [DataTestMethod]
    [DataRow("Sentence ending in a period.  Sentence ending in an exclamation mark!  Is this a sentence with a question mark?", 45, 3)] // 2 spaces between sentences
    [DataRow("Sentence ending in a period. Sentence ending in an exclamation mark! Is this a sentence with a question mark?", 45, 3)]  // 1 space between sentences
    [DataRow("Sentence ending in a period.Sentence ending in an exclamation mark!Is this a sentence with a question mark?", 45, 3)]  // no spaces between sentences
    [DataRow("Sentence ending in a period. Sentence ending in an exclamation mark! Is this a sentence with a question mark?", 90, 2)]
    [DataRow("Sentence ending in a period. Sentence ending in an exclamation mark! Is this a sentence with a question mark?", 150, 1)]
    [DataRow("fjlkfjslkefjsliejfallisjiasjfliejsfiljalsliefjalsiejflisajeflisaejfliefjlkaisejfilasejflisjefliasjefliiesj", 90, 2)]

    public void CreateChunks_CanChunkDifferentSentenceEndingsTypes(string sentences, int maxChunkSize, int expectedNumberOfSentences)
    {
        // Arrange
        var classUnderTest = new TextChunkingService();

        // Act 
        var actualResult = classUnderTest.CreateChunks(sentences, maxChunkSize);

        // Assert
        Assert.AreEqual(expectedNumberOfSentences, actualResult.Count);
    }

    [TestMethod]
    public void CreateChunks_WhenDealingWithALongStringOfTextThatWillNotFitIntoAChunk_ItIsBrokenApartCorrectly()
    {
        // Arrange
        var classUnderTest = new TextChunkingService();

        // Act 
        var actualResult = classUnderTest.CreateChunks(
            "fjlkfjslkefjsliejfallisjiasjfliejsfiljalsliefjalsiejflisajeflisaejfliefjlkaisejfilasejflisjefliasjefliiesj ", 90);

        // Assert
        Assert.AreEqual(2, actualResult.Count);
        string firstString = actualResult[0];
        Assert.AreEqual("fjlkfjslkefjsliejfallisjiasjfliejsfiljalsliefjalsiejflisajeflisaejfliefjlkaisejfilasejflis", firstString);
        string secondString = actualResult[1];
        Assert.AreEqual("jefliasjefliiesj", secondString);
    }

    [TestMethod]
    public void CreateChunks_WhenDealingWithASentenceAndLongStringOfTextThatWillNotFitIntoAChunk_ItIsBrokenApartCorrectly()
    {
        // Arrange
        var classUnderTest = new TextChunkingService();

        // Act 
        var actualResult = classUnderTest.CreateChunks(
            "This is some text fjlkfjslkefjsliejfallisjiasjfliejsfiljalsliefjalsiejflisajeflisaejfliefjlkaisejfilasejflisjefliasjefliiesj ", 90);

        // Assert
        Assert.AreEqual(2, actualResult.Count);
        string firstString = actualResult[0];
        Assert.AreEqual("This is some text fjlkfjslkefjsliejfallisjiasjfliejsfiljalsliefjalsiejflisajeflisaejfliefj", firstString);
        string secondString = actualResult[1];
        Assert.AreEqual("lkaisejfilasejflisjefliasjefliiesj", secondString);

    }
}