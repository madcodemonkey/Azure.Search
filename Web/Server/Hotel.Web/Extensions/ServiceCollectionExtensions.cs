using Hotel.Services;

namespace Hotel.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddHotelServices(this IServiceCollection services, IConfiguration config)
        {
            var settings = new SearchServiceSettings();
            config.GetSection("SearchService").Bind(settings);

            services.AddSearchServices(settings);


            return services;

        }
    }
}
