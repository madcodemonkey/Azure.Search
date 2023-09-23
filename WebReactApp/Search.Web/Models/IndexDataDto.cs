namespace Search.Web.Models;

public class IndexDataDto
{
    public string Name { get; set; } = string.Empty;
    public bool Created { get; set; }
    public int NumberOfDocuments { get; set; }
    public string RouteName { get; set; } = string.Empty;
}