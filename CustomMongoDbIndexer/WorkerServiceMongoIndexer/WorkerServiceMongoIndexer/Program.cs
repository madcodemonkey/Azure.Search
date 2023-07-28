using WorkerServiceMongoIndexer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .ConfigureServices((hostContext, sc) =>
    {
        var configuration = hostContext.Configuration;
        sc.AddCustomDependencies(configuration);
    })
    .Build();

await host.RunAsync();
