namespace Search.Model;

public class SearchFacetResponse
{
    public string Name { get; set; }

    public List<string> Values { get; set; } = new List<string>();
}