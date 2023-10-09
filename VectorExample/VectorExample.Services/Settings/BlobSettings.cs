namespace VectorExample.Services;

public class BlobSettings
{
    public const string SectionName = "Blob";

    /// <summary>
    /// An Access key to gain access to the storage account.  This could be key1 or key2 from the Access Keys section in the storage account
    /// </summary>
    public string AccessKey { get; set; } = string.Empty;

    /// <summary>
    /// The name of the storage account.  This is the name of the storage resource in the Azure Portal.
    /// </summary>
    public string AccountName { get; set; } = string.Empty;

    /// <summary>
    /// The name of the container within the storage account (<see cref="AccountName"/>) that contains the files we are watching.
    /// </summary>
    public string ContainerName { get; set; } = string.Empty;
}

 