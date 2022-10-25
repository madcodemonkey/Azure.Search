using Search.Repositories;
using Search.Services;

namespace Search.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddHotelServices(this IServiceCollection services, IConfiguration config)
        {
            var settings = new SearchServiceSettings();
            config.GetSection("SearchService").Bind(settings);

            var databaseOptions = new AcmeDatabaseOptions();
            config.GetSection("AcmeDatabaseOptions").Bind(databaseOptions);

            services.AddSearchServices(settings, databaseOptions);
            services.AddAcmeRepositories(databaseOptions);

            return services;

        }
    }
}
