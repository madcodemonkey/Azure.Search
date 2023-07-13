using System.Xml.Linq;
using System;

namespace CustomBlobIndexer.Services;

public class ServiceSettings
{
    /// <summary>
    /// The name of the storage account.  This is the name of the storage resource in the Azure Portal.
    /// </summary>
    public string BlobAccountName { get; set; }

    /// <summary>
    /// The name of the container within the storage account (<see cref="BlobAccountName"/>) that contains the files we are watching.
    /// </summary>
    public string BlobContainerName { get; set; }

    /// <summary>
    /// An Access key to gain access to the storage account.  This could be key1 or key2 from the Access Keys section in the storage account 
    /// </summary>
    public string BlobAccessKey { get; set; }
}