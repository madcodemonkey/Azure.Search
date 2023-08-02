using System.Text.Json;
using Azure.Core.Serialization;
using Azure.Search.Documents;

namespace Search.CogServices;

/// <summary>All options needs by my service so that I can avoid calling virtual methods in any of the classes
/// that I'm using as base classes.  If someone needs to override an option, they can inherit from this class and
/// override the method and then in the dependency injection they can register their override in place of  <see cref="IAcmeCogOptionsService"/>  </summary>
public class AcmeCogOptionsService : IAcmeCogOptionsService
{
    /// <summary>Create search options</summary>
    public virtual SearchClientOptions CreateSearchClientOptions()
    {
        // This is needed to avoid an error when uploading data that has a GeographyPoint property.  
        // Here is the error: The request is invalid. Details: parameters : Cannot find nested property 'location' on the resource type 'search.documentFields'.
        JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            Converters =
            {
                // The Super class should override the method and do this if GeoSpatial classes are needed!
                // Requires Microsoft.Azure.Core.Spatial NuGet package.
                // new MicrosoftSpatialGeoJsonConverter()
            },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return new SearchClientOptions
        {
            Serializer = new JsonObjectSerializer(serializerOptions)
        };
    }
}