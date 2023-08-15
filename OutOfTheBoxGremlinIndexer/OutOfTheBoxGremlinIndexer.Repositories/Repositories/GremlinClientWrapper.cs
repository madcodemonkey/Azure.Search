using Gremlin.Net.Driver;
using System.Net.WebSockets;
using Gremlin.Net.Driver.Remote;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Structure.IO.GraphSON;

namespace CustomSqlServerIndexer.Repositories;

public class GremlinClientWrapper : IDisposable, IGremlinClientWrapper
{
    private readonly RepositorySettings _repositorySettings;
    private GremlinClient? _client;
    private GremlinServer? _server;
    /// <summary>
    /// Constructor
    /// </summary>
    public GremlinClientWrapper(RepositorySettings repositorySettings)
    {
        _repositorySettings = repositorySettings;
    }

    public GremlinClient GetClient()
    {
        if (_client == null)
        {
            ConnectionPoolSettings connectionPoolSettings = new ConnectionPoolSettings()
            {
                MaxInProcessPerConnection = 10,
                PoolSize = 30,
                ReconnectionAttempts = 3,
                ReconnectionBaseDelay = TimeSpan.FromMilliseconds(500)
            };

            var webSocketConfiguration =
                new Action<ClientWebSocketOptions>(options =>
                {
                    options.KeepAliveInterval = TimeSpan.FromSeconds(10);
                });

            _client = new GremlinClient(
                GetServer(),
                new GraphSON2Reader(),
                new GraphSON2Writer(),
                GremlinClient.GraphSON2MimeType,
                webSocketConfiguration: webSocketConfiguration,
                connectionPoolSettings: connectionPoolSettings
            );
        }

        return _client;
    }

    public GraphTraversalSource GeTraversalSource()
    {
        GremlinClient gremlinClient = GetClient();
        var driverRemoteConnection = new DriverRemoteConnection(gremlinClient, "g");
        return AnonymousTraversalSource.Traversal().WithRemote(driverRemoteConnection);
    }

    private GremlinServer GetServer()
    {
        if (_server == null)
        {
            string containerLink = "/dbs/" + _repositorySettings.GremlinDatabaseName + "/colls/" + _repositorySettings.GremlinContainerName;


            _server = new GremlinServer(_repositorySettings.GremlinHostName,
                _repositorySettings.GremlinHostPort,
                enableSsl: _repositorySettings.GremlinEnableSSL,
                username: containerLink,
                password: _repositorySettings.GremlinKey);
        }

        return _server;
    }


    public void Dispose()
    {
        _client?.Dispose();
        _client = null;
    }
}