using Newtonsoft.Json.Linq;

namespace CustomSqlServerIndexer.Repositories;

public static class JObjectExtensions
{
    /// <summary>Get a boolean property off the provided object.</summary>
    /// <param name="jObj">The JObject to work with</param>
    /// <param name="propertyName">The name of the property</param>
    /// <param name="assertIfNotFound">Indicates if we should throw an <see cref="ArgumentException"/> if the property is not found</param>
    /// <param name="assertIfCannotBeParsed">The value to return if we cannot parse the property.</param>
    /// <param name="defaultIfNoAssertion">The value to use if we don't assert when the property is not found or cannot be parsed.</param>
    public static bool? GetBoolean(this JObject jObj, string propertyName,
        bool assertIfNotFound = false, bool assertIfCannotBeParsed = false, bool? defaultIfNoAssertion = null)
    {
        var prop = GetProperty(jObj, propertyName, assertIfNotFound);

        if (prop == null)
        {
            return defaultIfNoAssertion;
        }

        if (bool.TryParse(prop.ToString(), out var value))
        {
            return value;
        }

        if (assertIfCannotBeParsed)
        {
            throw new ArgumentException($"Unable to PARSE the '{propertyName}' property as a boolean on the provided JObject instance!");
        }

        return defaultIfNoAssertion;
    }

    /// <summary>Get a decimal property off the provided object.</summary>
    /// <param name="jObj">The JObject to work with</param>
    /// <param name="propertyName">The name of the property</param>
    /// <param name="assertIfNotFound">Indicates if we should throw an <see cref="ArgumentException"/> if the property is not found</param>
    /// <param name="assertIfCannotBeParsed">The value to return if we cannot parse the property.</param>
    /// <param name="defaultIfNoAssertion">The value to use if we don't assert when the property is not found or cannot be parsed.</param>
    public static decimal? GetDecimal(this JObject jObj, string propertyName,
        bool assertIfNotFound = false, bool assertIfCannotBeParsed = false, decimal? defaultIfNoAssertion = null)
    {
        var prop = GetProperty(jObj, propertyName, assertIfNotFound);

        if (prop == null)
        {
            return defaultIfNoAssertion;
        }

        if (decimal.TryParse(prop.ToString(), out var value))
        {
            return value;
        }

        if (assertIfCannotBeParsed)
        {
            throw new ArgumentException($"Unable to PARSE the '{propertyName}' property as a decimal on the provided JObject instance!");
        }

        return defaultIfNoAssertion;
    }

    /// <summary>Get a double property off the provided object.</summary>
    /// <param name="jObj">The JObject to work with</param>
    /// <param name="propertyName">The name of the property</param>
    /// <param name="assertIfNotFound">Indicates if we should throw an <see cref="ArgumentException"/> if the property is not found</param>
    /// <param name="assertIfCannotBeParsed">The value to return if we cannot parse the property.</param>
    /// <param name="defaultIfNoAssertion">The value to use if we don't assert when the property is not found or cannot be parsed.</param>
    public static double? GetDouble(this JObject jObj, string propertyName,
        bool assertIfNotFound = false, bool assertIfCannotBeParsed = false, double? defaultIfNoAssertion = null)
    {
        var prop = GetProperty(jObj, propertyName, assertIfNotFound);

        if (prop == null)
        {
            return defaultIfNoAssertion;
        }

        if (double.TryParse(prop.ToString(), out var value))
        {
            return value;
        }

        if (assertIfCannotBeParsed)
        {
            throw new ArgumentException($"Unable to PARSE the '{propertyName}' property as a double on the provided JObject instance!");
        }

        return defaultIfNoAssertion;
    }

    /// <summary>Get a string property off the provided object.</summary>
    /// <param name="jObj">The JObject to work with</param>
    /// <param name="propertyName">The name of the property</param>
    /// <param name="assertIfNotFound">Indicates if we should throw an <see cref="ArgumentException"/> if the property is not found</param>
    /// <param name="defaultIfNoAssertion">The value to use if we don't assert when the property is not found or cannot be parsed.</param>
    public static string? GetString(this JObject jObj, string propertyName,
        bool assertIfNotFound = false, string? defaultIfNoAssertion = null)
    {
        var prop = GetProperty(jObj, propertyName, assertIfNotFound);

        if (prop == null)
        {
            return defaultIfNoAssertion;
        }

        var result = prop.Value<string>();

        return result;
    }

    /// <summary>Get a int property off the provided object.</summary>
    /// <param name="jObj">The JObject to work with</param>
    /// <param name="propertyName">The name of the property</param>
    /// <param name="assertIfNotFound">Indicates if we should throw an <see cref="ArgumentException"/> if the property is not found</param>
    /// <param name="assertIfCannotBeParsed">The value to return if we cannot parse the property.</param>
    /// <param name="defaultIfNoAssertion">The value to use if we don't assert when the property is not found or cannot be parsed.</param>
    public static int? GetInt(this JObject jObj, string propertyName,
        bool assertIfNotFound = false, bool assertIfCannotBeParsed = false, int? defaultIfNoAssertion = null)
    {
        var prop = GetProperty(jObj, propertyName, assertIfNotFound);

        if (prop == null)
        {
            return defaultIfNoAssertion;
        }

        if (int.TryParse(prop.ToString(), out var value))
        {
            return value;
        }

        if (assertIfCannotBeParsed)
        {
            throw new ArgumentException($"Unable to PARSE the '{propertyName}' property as an integer on the provided JObject instance!");
        }

        return defaultIfNoAssertion;
    }

    /// <summary>Get a JArray property off the provided object.</summary>
    /// <param name="jObj">The JObject to work with</param>
    /// <param name="propertyName">The name of the property</param>
    /// <param name="assertIfNotFound">Indicates if we should throw an <see cref="ArgumentException"/> if the property is not found</param>
    /// <param name="assertIfCannotBeParsed">The value to return if we cannot parse the property.</param>
    /// <param name="defaultIfNoAssertion">The value to use if we don't assert when the property is not found or cannot be parsed.</param>
    public static JArray? GetJArray(this JObject jObj, string propertyName,
        bool assertIfNotFound = false, bool assertIfCannotBeParsed = false, JArray? defaultIfNoAssertion = null)
    {
        var prop = GetProperty(jObj, propertyName, assertIfNotFound);

        if (prop == null)
        {
            return defaultIfNoAssertion;
        }

        var result = prop as JArray;

        if (result == null && assertIfCannotBeParsed)
        {
            throw new ArgumentException($"Unable to PARSE the '{propertyName}' property as a JArray on the provided JObject instance!");
        }

        return result;
    }

    /// <summary>Get a JObject property off the provided object.</summary>
    /// <param name="jObj">The JObject to work with</param>
    /// <param name="propertyName">The name of the property</param>
    /// <param name="assertIfNotFound">Indicates if we should throw an <see cref="ArgumentException"/> if the property is not found</param>
    /// <param name="assertIfCannotBeParsed">The value to return if we cannot parse the property.</param>
    /// <param name="defaultIfNoAssertion">The value to use if we don't assert when the property is not found or cannot be parsed.</param>
    public static JObject? GetJObject(this JObject jObj, string propertyName,
        bool assertIfNotFound = false, bool assertIfCannotBeParsed = false, JObject? defaultIfNoAssertion = null)
    {
        var prop = GetProperty(jObj, propertyName, assertIfNotFound);

        if (prop == null)
        {
            return defaultIfNoAssertion;
        }

        var result = prop as JObject;

        if (result == null && assertIfCannotBeParsed)
        {
            throw new ArgumentException($"Unable to PARSE the '{propertyName}' property as a JObject on the provided JObject instance!");
        }

        return result;
    }

    /// <summary>Get a DateTime property off the provided object.</summary>
    /// <param name="jObj">The JObject to work with</param>
    /// <param name="propertyName">The name of the property</param>
    /// <param name="assertIfNotFound">Indicates if we should throw an <see cref="ArgumentException"/> if the property is not found</param>
    /// <param name="assertIfCannotBeParsed">The value to return if we cannot parse the property.</param>
    /// <param name="defaultIfNoAssertion">The value to use if we don't assert when the property is not found or cannot be parsed.</param>
    public static DateTime? GetDate(this JObject jObj, string propertyName,
        bool assertIfNotFound = false, bool assertIfCannotBeParsed = false, DateTime? defaultIfNoAssertion = null)
    {
        var prop = GetProperty(jObj, propertyName, assertIfNotFound);

        if (prop == null)
        {
            return defaultIfNoAssertion;
        }

        if (DateTime.TryParse(prop.ToString(), out var value))
        {
            return value;
        }

        if (assertIfCannotBeParsed)
        {
            throw new ArgumentException($"Unable to PARSE the '{propertyName}' property as a DateTime on the provided JObject instance!");
        }

        return defaultIfNoAssertion;
    }

    /// <summary>
    /// Gets the requested property or asserts if not found.
    /// </summary>
    /// <param name="jObj">The JObject to work with</param>
    /// <param name="propertyName">The name of the property</param>
    /// <param name="assertIfNotFound">Indicates if we should throw an <see cref="ArgumentException"/> if the property is not found</param>
    /// <returns>Null or a JToken</returns>
    /// <exception cref="ArgumentException"></exception>
    private static JToken? GetProperty(JObject jObj, string propertyName, bool assertIfNotFound)
    {
        var prop = jObj.TryGetValue(propertyName, out var value) ? value : null;

        if (prop == null)
        {
            if (assertIfNotFound)
            {
                throw new ArgumentException($"Unable to FIND the '{propertyName}' property on the provided JObject instance!");
            }

            return null;
        }

        return prop;
    }
}