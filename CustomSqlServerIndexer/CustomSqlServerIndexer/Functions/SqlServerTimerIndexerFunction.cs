using System;
using CustomSqlServerIndexer.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CustomSqlServerIndexer.Functions
{
    public class SqlServerTimerIndexerFunction
    {
        private readonly ICustomSqlServerIndexerService _indexerService;
        private readonly ILogger _logger;

        public SqlServerTimerIndexerFunction(ILoggerFactory loggerFactory, ICustomSqlServerIndexerService indexerService)
        {
            _indexerService = indexerService;
            _logger = loggerFactory.CreateLogger<SqlServerTimerIndexerFunction>();
        }

        [Function("SqlServerTimerIndexerFunction")]
        public async Task Run([TimerTrigger("0 */15 * * * *")] MyInfo myTimer)
        {
            await _indexerService.DoWorkAsync();
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
