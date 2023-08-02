using System.Text.Json;
using System.Text.RegularExpressions;

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

    /// <summary>
    /// Capitalizes the first character of a sentences.
    /// </summary>
    /// <param name="value">The string to manipulate</param>
    /// <remarks>
    /// See https://stackoverflow.com/a/4135491/97803
    /// </remarks>
    public static string? FirstLetterToUpper(this string? value)
    {
        if (value == null)
            return null;

        if (value.Length > 1)
            return char.ToUpper(value[0]) + value.Substring(1);

        return value.ToUpper();
    }

    /// <summary>
    /// Adds delimiter between each character based on camel casing.
    /// </summary>
    /// <param name="value">The string to use as input that should be camel cased.</param>
    /// <param name="delimiter">The text to place between each camel casing</param>
    /// <remarks>
    /// See https://stackoverflow.com/a/4489046/97803
    /// </remarks>
    public static string? SplitOnCamelCasing(this string? value, string delimiter = " ")
    {
        if (value == null) return value;

        var r = new Regex(@"
            (?<=[A-Z])(?=[A-Z][a-z]) |
            (?<=[^A-Z])(?=[A-Z]) |
            (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

        string result = r.Replace(value, delimiter);

        return result;
    }
}