namespace Search.Services.UnitTests;

/// <summary>Used to generate search objects for unit tests.</summary>
public class TestSearchDocument<T>
{
    /// <summary>The document that will have highlights.</summary>
    public T Document { get; set; } = default!;

    /// <summary>The search score</summary>
    public double? Score { get; set; } = 0.98;

    /// <summary>The highlights for the document.</summary>
    public Dictionary<string, IList<string>>? Highlights { get; set; }

}