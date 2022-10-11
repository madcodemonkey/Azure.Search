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

            services.AddSearchServices(settings);
            services.AddAcmeRepositories(options => config.GetSection("AcmeDatabaseOptions").Bind(options));

            return services;

        }
    }
}
