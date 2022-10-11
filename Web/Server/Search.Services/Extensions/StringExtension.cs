using System.Text.Json;

namespace Search.Services;

public static class StringExtension
{
    /// <summary>Converts a string to camel case.</summary>
    /// <param name="value">The string to convert</param>
    public static string ConvertToCamelCase(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return value;

        return JsonNamingPolicy.CamelCase.ConvertName(value);
    }
}