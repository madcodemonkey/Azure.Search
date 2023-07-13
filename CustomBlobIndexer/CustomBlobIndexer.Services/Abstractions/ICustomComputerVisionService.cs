namespace CustomBlobIndexer.Services;

public interface ICustomComputerVisionService
{
    /// <summary>
    /// Use OCR's compute API to extract text from PDF blobs
    /// </summary>
    /// <param name="sasUrl">A complete uri with SAS token to the blob item.</param>
    /// <returns>Extracted text</returns>
    Task<string> ReadFileAsync(string sasUrl);
}