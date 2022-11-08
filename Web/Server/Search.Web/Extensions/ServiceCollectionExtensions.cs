using Search.CogServices;
using Search.Repositories;
using Search.Services;
using Search.Web.Configuration;
using FluentValidation;

namespace Search.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddSearchWebServices(this IServiceCollection services, IConfiguration config)
        {
            // Automapper docs on DI: https://docs.automapper.org/en/stable/Dependency-injection.html
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);

            // Add Fluent validation
            services.AddValidatorsFromAssemblyContaining<AcmeSearchFilterItemValidator>();

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
