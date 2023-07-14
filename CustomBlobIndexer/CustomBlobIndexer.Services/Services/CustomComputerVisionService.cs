using System.Text;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Logging;

namespace CustomBlobIndexer.Services;

/// <summary>
/// Azure's Computer Vision service gives you access to advanced algorithms that process images and return information based on the visual features you're interested in.
/// It can OCR, do image analysis, face analysis and spatial analysis. 
/// </summary>
/// <remarks>
/// Computer Vision Main documentation: https://learn.microsoft.com/en-us/azure/cognitive-services/computer-vision/
/// </remarks>
public class CustomComputerVisionService : ICustomComputerVisionService
{
    private readonly ILogger<CustomComputerVisionService> _logger;
    private readonly ServiceSettings _settings;
    private IComputerVisionClient? _client;
    private const int NumberOfCharsInAnOperationId = 36;

    /// <summary>
    /// Constructor
    /// </summary>
    public CustomComputerVisionService(ILogger<CustomComputerVisionService> logger, ServiceSettings settings)
    {
        _logger = logger;
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
       
        // Read text from URL
        var textHeaders = await client.ReadAsync(sasUrl);

        // After the request, get the operation location (operation ID)
        string operationLocation = textHeaders.OperationLocation;
        Thread.Sleep(2000); // TODO: Can we delete this?  It was part of the demo code, but it's bad practice!

        // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
        // We only need the ID and not the full URL
        string operationId = operationLocation.Substring(operationLocation.Length - NumberOfCharsInAnOperationId);

        // Extract the text
        ReadOperationResult results;

        _logger.LogInformation($"Extracting text from URL file {Path.GetFileName(sasUrl)}...");

        do
        {
            results = await client.GetReadResultAsync(Guid.Parse(operationId));
        }
        while (results.Status == OperationStatusCodes.Running || results.Status == OperationStatusCodes.NotStarted);

        return ConsolidatePagesToOneString(results);
    }

    /// <summary>
    /// Consolidates all the pages of text to a single line.
    /// </summary>
    private string ConsolidatePagesToOneString(ReadOperationResult results)
    {
        var result = new StringBuilder();

        var textUrlFileResults = results.AnalyzeResult.ReadResults;
        foreach (ReadResult page in textUrlFileResults)
        {
            foreach (Line line in page.Lines)
            {
                result.AppendLine(line.Text);
            }
        }

        return result.ToString();
    }

 
    /// <summary>
    /// Creates or returns the client
    /// </summary>
    private IComputerVisionClient GetClient()
    {
        return _client ??= new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(_settings.CognitiveServiceKey))
        {
            Endpoint = _settings.CognitiveServiceEndpoint
        };
    }
}