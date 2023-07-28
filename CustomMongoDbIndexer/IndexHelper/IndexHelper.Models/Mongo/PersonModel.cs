using MongoDBServices;

namespace IndexHelper.Models;

public class PersonModel : MongoEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; } = 18;
    public string Description { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}  FirstName: {FirstName} LastName: {LastName}  Age: {Age}  Description: {Age}";
    }
}