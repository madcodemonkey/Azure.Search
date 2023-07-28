using CustomMongoDbIndexer.Services;

namespace CustomMongoDbIndexer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMonitorDatabaseService _monitorDatabaseService;
  
        /// <summary>
        /// Constructor
        /// </summary>
        public Worker(ILogger<Worker> logger, 
            IMonitorDatabaseService monitorDatabaseService)
        {
            _logger = logger;
            _monitorDatabaseService = monitorDatabaseService;
        }

        /// <summary>
        /// Main entry point!
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _monitorDatabaseService.StartAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Some error occurred while monitoring the Mongo database!");
                    Console.WriteLine(ex);
                    throw;
                }
            }

            _logger.LogInformation("Worker stopping at: {time}", DateTimeOffset.Now);
        }
    }
}