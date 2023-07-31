using MongoDBServices;

namespace IndexHelper.Models;

public class PersonModel : MongoEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; } = 18;
    public string Description { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"Id: {Id}  FirstName: {FirstName} LastName: {LastName}  Age: {Age}  Description: {Age}";
    }
}