using System.Text.Json;

namespace Search.CogServices;

public static class StringExtensions
{
    /// <summary>Converts a string to camel case.</summary>
    /// <param name="value">The string to convert</param>
    public static string ConvertToCamelCase(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return value;

        return JsonNamingPolicy.CamelCase.ConvertName(value);
    }
}