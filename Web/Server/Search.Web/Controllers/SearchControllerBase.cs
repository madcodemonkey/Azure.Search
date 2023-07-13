using Microsoft.AspNetCore.Mvc;

namespace Search.Controllers;

public abstract class SearchControllerBase : ControllerBase
{
    protected List<string> GetRoles()
    {
        return new List<string>() { "admin" };
        //ClaimsPrincipal currentUser = this.User;
        //return currentUser.FindAll(ClaimTypes.Role).Select(s => s.Value.ToLower()).ToArray();
    }
}