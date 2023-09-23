using System.Text.Json;
using Azure.Core.Serialization;
using Azure.Search.Documents;
using Search.CogServices;

namespace Search.Services;

/// <summary>This class overrides some of our basic index search items.</summary>
public class AppOptionsService : AcmeOptionsService
{
    /// <summary>Create search options</summary>
    public override SearchClientOptions CreateSearchClientOptions()
    {
        // This is needed to avoid an error when uploading data that has a GeographyPoint property.  
        // Here is the error: The request is invalid. Details: parameters : Cannot find nested property 'location' on the resource type 'search.documentFields'.
        JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            Converters =
            {
                // Requires Microsoft.Azure.Core.Spatial NuGet package.
                new MicrosoftSpatialGeoJsonConverter()
            },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return new SearchClientOptions
        {
            Serializer = new JsonObjectSerializer(serializerOptions)
        };
    }
}