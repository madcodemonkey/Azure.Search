using CustomBlobIndexer.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CustomBlobIndexer;

public class BlobTriggerFunction
{
    private readonly IBlobSasBuilderService _sasBuilderService;
    private readonly ILogger _logger;

    public BlobTriggerFunction(ILoggerFactory loggerFactory, IBlobSasBuilderService sasBuilderService)
    {
        _sasBuilderService = sasBuilderService;
        _logger = loggerFactory.CreateLogger<BlobTriggerFunction>();
    }

      
    [Function("BlobTriggerFunction")]
    public void Run([BlobTrigger("my-files/{name}", Connection = "BlobStorageConnectionString")] byte[] myBlob, string name, Uri uri)
    {
        _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} Uri: {uri}  ");

        string newUriWithSaSToken = _sasBuilderService.GenerateSaSUrl(name, uri);

        _logger.LogInformation(newUriWithSaSToken);


    }
}