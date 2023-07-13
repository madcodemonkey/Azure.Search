using Azure.Storage;
using Azure.Storage.Sas;

namespace CustomBlobIndexer.Services;

/// <summary>
/// Used for creating SAS tokens
/// </summary>
/// <remarks>
/// Requires the Microsoft.Azure.WebJobs.Extensions.Storage NuGet package!
/// </remarks>
public class BlobSasBuilderService : IBlobSasBuilderService
{
    private readonly ServiceSettings _settings;

    /// <summary>
    /// Constructor
    /// </summary>
    public BlobSasBuilderService(ServiceSettings settings)
    {
        _settings = settings;
    }

    /// <summary>
    /// Generates a SAS Url by first calling <see cref="GenerateSaSToken"/> and then tagging it onto the <see cref="blobUri"/>
    /// </summary>
    /// <param name="blobName">The name of the blob (partial path if it is a "sub-directory" (e.g., example.pdf or someFiles/example.pdf</param>
    /// <param name="blobUri">The URI to the blob without a SAS token</param>
    /// <param name="permissions">The permissions that the SAS token should allow (defaults to Read).</param>
    /// <param name="expiration">The expiration of the SAS token</param>
    /// <returns>Returns a URL with a SAS token set as the query string.</returns>
    public string GenerateSaSUrl(string blobName, Uri blobUri, BlobSasPermissions permissions = BlobSasPermissions.Read, DateTime? expiration = null)
    {
        var sasToken = GenerateSaSToken(blobName, permissions, expiration);
        var sasUrl = blobUri.AbsoluteUri + "?" + sasToken;
        
        return sasUrl;
    }

    /// <summary>
    /// Generates a SAS token
    /// </summary>
    /// <param name="blobName">The name of the blob (partial path if it is a "sub-directory" (e.g., example.pdf or someFiles/example.pdf</param>
    /// <param name="permissions">The permissions that the SAS token should allow (defaults to Read).</param>
    /// <param name="expiration">The expiration of the SAS token</param>
    /// <returns>Returns a SAS token.</returns>
    public string GenerateSaSToken(string blobName, BlobSasPermissions permissions = BlobSasPermissions.Read, DateTime? expiration = null)
    {
        expiration ??= DateTime.UtcNow.AddHours(1);

        // https://stackoverflow.com/questions/71638718/unable-to-get-blob-sas-url-using-azure-function-blob-trigger
        BlobSasBuilder blobSasBuilder = new BlobSasBuilder
        {
            BlobContainerName = _settings.BlobContainerName,
            BlobName = blobName,
            ExpiresOn = expiration.Value
        };

        blobSasBuilder.SetPermissions(permissions);

        var sasToken = blobSasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_settings.BlobAccountName, _settings.BlobAccessKey)).ToString();
        
        return sasToken;
    }
}