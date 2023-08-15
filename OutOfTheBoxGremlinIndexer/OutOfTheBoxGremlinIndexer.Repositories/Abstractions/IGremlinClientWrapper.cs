using Gremlin.Net.Driver;
using Gremlin.Net.Process.Traversal;

namespace CustomSqlServerIndexer.Repositories;

public interface IGremlinClientWrapper : IDisposable
{
    GremlinClient GetClient();
    GraphTraversalSource GeTraversalSource();
}