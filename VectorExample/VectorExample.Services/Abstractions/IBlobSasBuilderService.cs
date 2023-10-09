using Azure.Storage.Sas;

namespace VectorExample.Services;

public interface IBlobSasBuilderService
{
    /// <summary>
    /// Generates a SAS Url by first calling <see cref="GenerateSaSToken"/> and then tagging it onto the <see cref="blobUri"/>
    /// </summary>
    /// <param name="blobName">The name of the blob (partial path if it is a "sub-directory" (e.g., example.pdf or someFiles/example.pdf</param>
    /// <param name="blobUri">The URI to the blob without a SAS token</param>
    /// <param name="permissions">The permissions that the SAS token should allow (defaults to Read).</param>
    /// <param name="expiration">The expiration of the SAS token</param>
    /// <returns>Returns a URL with a SAS token set as the query string.</returns>
    string GenerateSaSUrl(string blobName, Uri blobUri, BlobSasPermissions permissions = BlobSasPermissions.Read, DateTime? expiration = null);

    /// <summary>
    /// Generates a SAS token
    /// </summary>
    /// <param name="blobName">The name of the blob (partial path if it is a "sub-directory" (e.g., example.pdf or someFiles/example.pdf</param>
    /// <param name="permissions">The permissions that the SAS token should allow (defaults to Read).</param>
    /// <param name="expiration">The expiration of the SAS token</param>
    /// <returns>Returns a SAS token.</returns>
    string GenerateSaSToken(string blobName, BlobSasPermissions permissions = BlobSasPermissions.Read, DateTime? expiration = null);
}