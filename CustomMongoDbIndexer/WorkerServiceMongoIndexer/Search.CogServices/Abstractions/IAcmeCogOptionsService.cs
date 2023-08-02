using Azure.Search.Documents;

namespace Search.CogServices;

/// <summary>All options needs by my service so that I can avoid calling virtual methods in any of the classes
/// that I'm using as base classes.  If someone needs to override an option, they can inherit from this class and
/// override the method and then in the dependency injection they can register their override in place of  <see cref="IAcmeCogOptionsService"/>  </summary>
public interface IAcmeCogOptionsService
{
    /// <summary>Create search options</summary>
    SearchClientOptions CreateSearchClientOptions();
}