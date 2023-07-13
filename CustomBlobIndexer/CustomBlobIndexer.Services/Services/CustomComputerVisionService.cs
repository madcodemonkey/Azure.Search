using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace CustomBlobIndexer.Services;

// TODO: Remove all Console.WriteLines

/// <summary>
/// 
/// </summary>
/// <remarks>
/// Computer Vision Main documentation: https://learn.microsoft.com/en-us/azure/cognitive-services/computer-vision/
/// </remarks>
public class CustomComputerVisionService : ICustomComputerVisionService
{
    private readonly ServiceSettings _settings;
    private IComputerVisionClient? _client;

    /// <summary>
    /// Constructor
    /// </summary>
    public CustomComputerVisionService(ServiceSettings settings)
    {
        _settings = settings;
    }

    /// <summary>
    /// Use OCR's compute API to extract text from PDF blobs
    /// </summary>
    /// <param name="sasUrl">A complete uri with SAS token to the blob item.</param>
    /// <returns>Extracted text</returns>
    public async Task<string> ReadFileAsync(string sasUrl)
    {
        var client = GetClient();

        string returnText = "";

        // Read text from URL
        var textHeaders = await client.ReadAsync(sasUrl);
        // After the request, get the operation location (operation ID)
        string operationLocation = textHeaders.OperationLocation;
        Thread.Sleep(2000);

        // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
        // We only need the ID and not the full URL
        const int numberOfCharsInOperationId = 36;
        string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

        // Extract the text
        ReadOperationResult results;
        Console.WriteLine($"Extracting text from URL file {Path.GetFileName(sasUrl)}...");
        Console.WriteLine();
        do
        {
            results = await client.GetReadResultAsync(Guid.Parse(operationId));
        }
        while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));

        // Display the found text.
        Console.WriteLine();
        var textUrlFileResults = results.AnalyzeResult.ReadResults;
        foreach (ReadResult page in textUrlFileResults)
        {
            foreach (Line line in page.Lines)
            {
                returnText += line.Text + "\n";
                Console.WriteLine(line.Text);
            }
        }
        Console.WriteLine();
        return returnText;
    }

 
    private IComputerVisionClient GetClient()
    {
        return _client ??= new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(_settings.CognitiveServiceKey))
        {
            Endpoint = _settings.CognitiveServiceEndpoint
        };
    }
}