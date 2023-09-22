using System.Text;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Exceptions;
using Microsoft.Extensions.Logging;
using OutOfTheBoxGremlinIndexer.Models;

namespace OutOfTheBoxGremlinIndexer.Repositories;

public class GremlinDataRepository : IGremlinDataRepository
{
    private readonly IGremlinClientWrapper _clientWrapper;
    private readonly ILogger<GremlinDataRepository> _logger;
    /// <summary>
    /// Constructor
    /// </summary>
    public GremlinDataRepository(ILogger<GremlinDataRepository> logger, IGremlinClientWrapper clientWrapper)
    {
        _logger = logger;
        _clientWrapper = clientWrapper;
    }

    /// <summary>
    /// Creates a "knows" relationship between two people
    /// </summary>
    /// <param name="id1">The first person's id</param>
    /// <param name="id2">The second person's id</param>
    public async Task<ResultSet<dynamic>> CreateKnowsRelationshipAsync(string id1, string id2)
    {
        return await SubmitRequestAsync($"g.V('{id1}').addE('knows').to(g.V('{id2}'))");
    }


    /// <summary>
    /// Creates a new person
    /// </summary>
    /// <param name="id">The person id</param>
    /// <param name="firstname">first name</param>
    /// <param name="lastName">last name</param>
    /// <param name="age">age</param>
    /// <param name="isDeleted">The soft delete field (by default should be false)</param>
    public async Task<ResultSet<dynamic>> CreatePersonAsync(string id, string firstname, string lastName, int age, bool isDeleted = false)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("g.addV('person')");
        sb.Append($".property('id', '{id}')");
        sb.Append($".property('firstName', '{firstname}')");
        sb.Append($".property('lastName', '{lastName}')");
        sb.Append($".property('age', {age})");
        sb.Append($".property('isDeleted', {isDeleted.ToString().ToLower()})");
        sb.Append(".property('pk', 'pk')");

        return await SubmitRequestAsync(sb.ToString());
    }

    /// <summary>
    /// Deletes all the data
    /// </summary>
    public async Task<ResultSet<dynamic>> DeleteAllAsync()
    {
        return await SubmitRequestAsync("g.V().drop()");
    }

    /// <summary>
    /// Does a soft delete on a person
    /// </summary>
    /// <param name="id">The id of the person</param>
    /// <param name="softDelete">true just sets the isDelete flag to true; otherwise, false really delete the record!</param>
    public async Task<bool> DeletePersonAsync(string id, bool softDelete = true)
    {
        if (softDelete)
        {
            await SubmitRequestAsync($"g.V('{id}').property('isDeleted', true)");
        }
        else
        {
            await SubmitRequestAsync($"g.V('{id}').drop()");
        }

        return true;
    }


    /// <summary>
    /// Gets all the people in the Graph database.
    /// </summary>
    public async Task<List<Person>> GetPeopleAsync(bool showSoftDeleted = false)
    {
        var result = new List<Person>();

        ResultSet<dynamic> rawData = showSoftDeleted ?
            await SubmitRequestAsync("g.V()") :
            await SubmitRequestAsync("g.V().hasLabel('person').has('isDeleted', eq(false))");

        foreach (Dictionary<string, object> item in rawData)
        {
            Dictionary<string, object> properties = item.ContainsKey("properties") ?
                item["properties"] as Dictionary<string, object> :
                new Dictionary<string, object>();

            if (properties == null || properties.Count == 0) continue;

             result.Add(new Person
            {
                Id = item["id"].ToString(),
                FirstName = GetFirstValueFromProperty<string>(properties, "firstName"),
                LastName = GetFirstValueFromProperty<string>(properties, "lastName"),
                Age = GetFirstValueFromProperty<int>(properties, "age"),
                IsDeleted = GetFirstValueFromProperty<bool>(properties, "isDeleted")
            });
        }

        return result;
    }

    /// <summary>
    /// Checks to see if a person exists in the database.
    /// </summary>
    /// <param name="id">The persons id</param>
    public async Task<bool> PersonExistsAsync(string id)
    {
        // Warning! I attempted to use count, but none of these worked!
        // var result = await SubmitRequestAsync($"g.V('{id}').count()");
        // var result = await SubmitRequestAsync($"g.V().has('person','id','{id}').count()");
        
        // This is working!
        var result = await SubmitRequestAsync($"g.V().has('person','id','{id}')");

        return result.Count > 0;
    }

    private T? ConvertObjectInDictionary<T>(IReadOnlyDictionary<string, object>? dictionary, string key)
    {
        if (dictionary != null && dictionary.ContainsKey(key))
        {
            // The object should be an array.  We are only interested in the first element
            var data = dictionary[key];
            return (T)Convert.ChangeType(data, typeof(T));
        }

        return default;
    }

    /// <summary>
    /// Given the properties element, which can be treated like a dictionary, this method will find the first array and get the value from it and convert it to type T.
    ///"properties": {
    ///    "firstName": [
    ///    {
    ///       "id": "0da5c661-6db8-48d3-b17b-89821ad07ad1",
    ///       "value": "Thomas"
    ///    }
    ///    ],
    ///    "lastName": [
    ///     {
    ///        "id": "aca32c6b-a1a8-4fde-a6de-4a50b9fd3e54",
    ///         "value": "Paine"
    ///      }
    ///     ]
    /// }
    /// Here properties is the dictionary parameter!
    /// firstName or lastName would be a key.
    /// </summary>
    /// <typeparam name="T">The type to convert the value to.</typeparam>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    private T? GetFirstValueFromProperty<T>(IReadOnlyDictionary<string, object> dictionary, string key)
    {
        if (dictionary.ContainsKey(key))
        {
            // The object should be an array.  We are only interested in the first element
            if (dictionary[key] is IEnumerable<object> arrayData)
            {
                // Get the first element in the array
                var firstElementInTheArray = arrayData.FirstOrDefault();

                // The first element in the array can be treated as a dictionary
                var firstElementAsDictionary = firstElementInTheArray as IReadOnlyDictionary<string, object>;
                return ConvertObjectInDictionary<T>(firstElementAsDictionary, "value");
            }
        }

        return default;
    }

    private async Task<ResultSet<dynamic>> SubmitRequestAsync(string requestMessage)
    {
        try
        {
            var gremlinClient = _clientWrapper.GetClient();
            return await gremlinClient.SubmitAsync<dynamic>(requestMessage);
        }
        catch (ResponseException e)
        {
            _logger.LogError(e, $"Request Error!  Status code: {e.StatusCode}");

            // On error, ResponseException.StatusAttributes will include the common StatusAttributes for successful requests, as well as
            // additional attributes for retry handling and diagnostics.
            // These include:
            //  x-ms-retry-after-ms         : The number of milliseconds to wait to retry the operation after an initial operation was throttled. This will be populated when
            //                              : attribute 'x-ms-status-code' returns 429.
            //  x-ms-activity-id            : Represents a unique identifier for the operation. Commonly used for troubleshooting purposes.
            _logger.LogError($"[\"x-ms-retry-after-ms\"] : {ConvertObjectInDictionary<string>(e.StatusAttributes, "x-ms-retry-after-ms")}");
            _logger.LogError($"[\"x-ms-activity-id\"] : {ConvertObjectInDictionary<string>(e.StatusAttributes, "x-ms-activity-id")}");

            throw;
        }
    }
}
