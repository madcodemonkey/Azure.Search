using System.Text;

namespace Search.CogServices;

public static class StringBuilderExtensions
{
    /// <summary>Adds a string to the beginning of the string.</summary>
    /// <param name="sb">The string builder to modify.</param>
    /// <param name="value">The value to add to the beginning of the string</param>
    public static void Prepend(this StringBuilder sb, string value)
    {
        sb.Insert(0, value);
    }

    /// <summary>Surrounds the string with a beginning parenthesis '(' and puts an ending parenthesis ')' onto the string.</summary>
    /// <param name="sb">The string builder to modify.</param>
    public static void SurroundWithParenthesis(this StringBuilder sb)
    {
        sb.Prepend("(");
        sb.Append(")");
    }
}