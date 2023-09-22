using Gremlin.Net.Driver;
using Gremlin.Net.Process.Traversal;

namespace OutOfTheBoxGremlinIndexer.Repositories;

public interface IGremlinClientWrapper : IDisposable
{
    GremlinClient GetClient();
    GraphTraversalSource GeTraversalSource();
}