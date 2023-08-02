using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CustomSqlServerIndexer.Functions
{
    public class SqlServerIndexerFunction
    {
        private readonly ILogger _logger;

        public SqlServerIndexerFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SqlServerIndexerFunction>();
        }

        [Function("SqlServerIndexerFunction")]
        public void Run([TimerTrigger("0 */5 * * * *")] MyInfo myTimer)
        {
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
