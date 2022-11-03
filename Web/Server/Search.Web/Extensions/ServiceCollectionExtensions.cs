using Search.CogServices;
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

            services.AddCogServices(settings); // Do this first in case we override an interface registered here in AddSearchServices!
            services.AddSearchServices(settings, databaseOptions);
            services.AddAcmeRepositories(databaseOptions);

            return services;

        }
    }
}
