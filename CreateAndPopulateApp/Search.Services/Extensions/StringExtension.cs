using System.Text.Json;

namespace Search.Services;

public static class StringExtension
{
    public static string ConvertToCamelCase(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return value;

        return JsonNamingPolicy.CamelCase.ConvertName(value);
    }
}