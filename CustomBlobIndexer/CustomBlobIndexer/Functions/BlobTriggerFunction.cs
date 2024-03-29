using CustomBlobIndexer.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CustomBlobIndexer.Functions;

public class BlobTriggerFunction
{
    private readonly IFileProcessService _fileProcessService;
    private readonly ILogger _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    public BlobTriggerFunction(ILoggerFactory loggerFactory, IFileProcessService fileProcessService)
    {
        _fileProcessService = fileProcessService;
        _logger = loggerFactory.CreateLogger<BlobTriggerFunction>();
    }


    [Function("BlobTriggerFunction")]
    public async Task Run([BlobTrigger("my-files/{name}", Connection = "Blob:StorageConnectionString")] 
        byte[] myBlob, string name, Uri uri, CancellationToken cancellationToken)
    {
        await _fileProcessService.ProcessFileAsync(name, uri, cancellationToken);

    }
}