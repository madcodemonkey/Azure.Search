namespace CustomBlobIndexer.Services.UnitTests;

[TestClass]
public class UriExtensionsTests
{
    [DataTestMethod]
    [DataRow("https://myStuff.blob.core.windows.net/my-files/OutputTest7-PII-Test.pdf", "my-files", false, "/OutputTest7-PII-Test.pdf")]
    [DataRow("https://myStuff.blob.core.windows.net/my-files/OutputTest7-PII-Test.pdf", "my-files", true, "OutputTest7-PII-Test.pdf")]
    [DataRow("https://myStuff.blob.core.windows.net/my-files/SubFolder1/OutputTest7-PII-Test.pdf", "my-files", false, "/SubFolder1/OutputTest7-PII-Test.pdf")]
    [DataRow("https://myStuff.blob.core.windows.net/my-files/SubFolder1/OutputTest7-PII-Test.pdf", "my-files", true, "SubFolder1/OutputTest7-PII-Test.pdf")]
    [DataRow("https://myStuff.blob.core.windows.net/my-boxes/BoxCart/OutputTest.pdf", "my-boxes", false, "/BoxCart/OutputTest.pdf")] 
    [DataRow("https://myStuff.blob.core.windows.net/my-boxes/BoxCart/OutputTest.pdf", "my-boxes", true, "BoxCart/OutputTest.pdf")]
    [DataRow("https://myStuff.blob.core.windows.net/my-files/BoxCart/OutputTest.pdf", "my-boxes", false, "")]  // specified afterThisText not found!
    [DataRow("https://myStuff.blob.core.windows.net/my-files/BoxCart/OutputTest.pdf", "my-boxes", true, "")]  // specified afterThisText not found!
    public void GetPathAfterText_CanExtractTheProperText(string uriText, string afterThisText, bool trimOffLeadingSlash, string expectedResponse)
    {
        // Arrange
        Uri myUri = new Uri(uriText);

        // Act
        string actualResponse = myUri.GetPathAfterText(afterThisText, trimOffLeadingSlash);

        // Assert
        Assert.AreEqual(expectedResponse, actualResponse);
    }
}