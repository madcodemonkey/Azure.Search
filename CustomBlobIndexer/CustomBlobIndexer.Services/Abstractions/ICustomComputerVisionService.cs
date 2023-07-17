namespace CustomBlobIndexer.Services;

/// <summary>
/// Azure's Computer Vision service gives you access to advanced algorithms that process images and return information based on the visual features you're interested in.
/// It can OCR, do image analysis, face analysis and spatial analysis. 
/// </summary>
public interface ICustomComputerVisionService
{
    /// <summary>
    /// Use OCR's compute API to extract text from PDF blobs
    /// </summary>
    /// <param name="sasUrl">A complete uri with SAS token to the blob item.</param>
    /// <returns>Extracted text</returns>
    /// <remarks>
    /// Example: https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/textanalytics/Azure.AI.TextAnalytics/samples/Sample4_RecognizeEntities.md
    /// </remarks>
    Task<string> ReadFileAsync(string sasUrl);
}