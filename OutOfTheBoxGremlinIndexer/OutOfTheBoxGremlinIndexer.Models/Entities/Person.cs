namespace CustomSqlServerIndexer.Models;

public class Person
{
    public string Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? Age { get; set; }
    public bool IsDeleted { get; set; } = false;

}