using VectorExample;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;


// Logging notes
// - For isolated Azure Functions, add either APPINSIGHTS_INSTRUMENTATIONKEY or APPLICATIONINSIGHTS_CONNECTION_STRING,
//   which will be the preferred way of doing this in the future, to your local.settings.json file.
// - It is recommended that you update your host.json files if applicationInsights settings is missing
// - The Microsoft documentation, https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide#start-up-and-configuration, 
//   list an example that doesn't work even if you include  Microsoft.Azure.Functions.Worker.ApplicationInsights, which is in pre-release.
//   perhaps it will work in the future and also be the solution to doing Telemetry calls to Application Insights, which doesn't work with
//   Microsoft.ApplicationInsights.AspNetCore either. 

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults((hostContext, builder) =>
    {
        // https://docs.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide#middleware
        // Register our custom middleware with the worker
        builder.UseMiddleware<ExceptionMiddleware>();
    })
    .ConfigureAppConfiguration((hostContext, builder) =>
    {
        // Reference: https://stackoverflow.com/q/74269765/97803
        if (hostContext.HostingEnvironment.IsDevelopment())
        {
            builder.AddJsonFile("local.settings.json");
            builder.AddUserSecrets<Program>();
        }
    })
    .ConfigureServices((hostContext, s) =>
    {
        var configuration = hostContext.Configuration;
        
        s.AddCustomBlobIndexerDependencies(configuration);
    })
    .Build();

host.Run();
 