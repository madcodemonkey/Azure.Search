using System.Text;
using CustomSqlServerIndexer.Models;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Exceptions;
using Newtonsoft.Json;

namespace CustomSqlServerIndexer.Repositories;

public class GremlinDataRepository : IGremlinDataRepository
{
    private readonly IGremlinClientWrapper _clientWrapper;

    public GremlinDataRepository(IGremlinClientWrapper clientWrapper)
    {
        _clientWrapper = clientWrapper;
    }

    public async Task<ResultSet<dynamic>> DeleteAllAsync()
    {
        return await SubmitRequestAsync("g.V().drop()");
    }

    public async Task<ResultSet<dynamic>> CreatePersonAsync(string id, string firstname, string lastName, int age, bool isDeleted)
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

    public async Task<List<Person>> GetPeopleAsync()
    {
        var d2d = await PersonExistsAsync("thoma2s");
        var dd = await PersonExistsAsync("thomas");
        var result = new List<Person>();

        ResultSet<dynamic> rawData = await SubmitRequestAsync("g.V()");

        foreach (Dictionary<string, object> item in rawData)
        {
            Dictionary<string, object> properties = item.ContainsKey("properties") ?
                item["properties"] as Dictionary<string, object> :
                new Dictionary<string, object>();

            if (properties == null) continue;

             result.Add(new Person
            {
                Id = item["id"].ToString(),
                FirstName = GetValue<string>(properties, "firstName"),
                LastName = GetValue<string>(properties, "lastName"),
                Age = GetValue<int>(properties, "age"),
                IsDeleted = GetValue<bool>(properties, "isDeleted")
            });
        }

        return result;
    }


    public async Task<ResultSet<dynamic>> CreateKnowsRelationshipAsync(string id1, string id2)
    {
        return await SubmitRequestAsync($"g.V('{id1}').addE('knows').to(g.V('{id2}'))");
    }

    public async Task<bool> PersonExistsAsync(string id)
    {
        // g.V().has('id', 'tho3mas').count()
        //var result = await SubmitRequestAsync($"g.V().fold().has('id','{id}').count()");
        var result = await SubmitRequestAsync($"g.V('{id}').count()");
        return result.Count > 0;
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
            Console.WriteLine("\tRequest Error!");

            // Print the Gremlin status code.
            Console.WriteLine($"\tStatusCode: {e.StatusCode}");

            // On error, ResponseException.StatusAttributes will include the common StatusAttributes for successful requests, as well as
            // additional attributes for retry handling and diagnostics.
            // These include:
            //  x-ms-retry-after-ms         : The number of milliseconds to wait to retry the operation after an initial operation was throttled. This will be populated when
            //                              : attribute 'x-ms-status-code' returns 429.
            //  x-ms-activity-id            : Represents a unique identifier for the operation. Commonly used for troubleshooting purposes.
            Console.WriteLine($"\t[\"x-ms-retry-after-ms\"] : {GetValueAsObject(e.StatusAttributes, "x-ms-retry-after-ms")}");
            Console.WriteLine($"\t[\"x-ms-activity-id\"] : {GetValueAsObject(e.StatusAttributes, "x-ms-activity-id")}");

            throw;
        }
    }

    public static T GetValue<T>(IReadOnlyDictionary<string, object> dictionary, string key)
    {
        var valueOrDefault = GetValueOrDefault(dictionary, key);
        var valueAsString = JsonConvert.SerializeObject(valueOrDefault);
        var theArray = JsonConvert.DeserializeObject<GValue[]>(valueAsString);
        if (theArray != null && theArray.Length > 0)
        {
            return (T)Convert.ChangeType(theArray[0].value, typeof(T));

        }
        return default;
    }

    public static object GetValueAsObject(IReadOnlyDictionary<string, object> dictionary, string key)
    {
        var valueOrDefault = GetValueOrDefault(dictionary, key);
        var valueAsString = JsonConvert.SerializeObject(valueOrDefault);
        var theArray = JsonConvert.DeserializeObject<GValue[]>(valueAsString);
        if (theArray != null)
        {
      
            return theArray[0].value;
        }
        return valueAsString;
    }

    public static object GetValueOrDefault(IReadOnlyDictionary<string, object> dictionary, string key)
    {
        if (dictionary.ContainsKey(key))
        {
            var valueOrDefault = dictionary[key]; 
            return valueOrDefault;
        }

        return null;
    }
}

internal class GValue
{
    public string id { get; set; }

    public object value { get; set; }

}