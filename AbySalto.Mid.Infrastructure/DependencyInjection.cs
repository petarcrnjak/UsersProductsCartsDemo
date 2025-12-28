using AbySalto.Mid.Application.Favorites;
using AbySalto.Mid.Application.Services;
using AbySalto.Mid.Application.Services.External;
using AbySalto.Mid.Infrastructure.Configuration;
using AbySalto.Mid.Infrastructure.Persistence;
using AbySalto.Mid.Infrastructure.Repositories;
using AbySalto.Mid.Infrastructure.Services;
using AbySalto.Mid.Infrastructure.Services.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AbySalto.Mid.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices(configuration);
            services.AddImplementations();
            services.AddDatabase(configuration);
            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind config
            services.Configure<ExternalApisSettings>(configuration.GetSection(ExternalApisSettings.SectionName));

            services.AddSingleton(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            });

            services.AddHttpClient<IDummyJsonApiClient, DummyJsonApiClient>((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<ExternalApisSettings>>().Value;

                client.BaseAddress = new Uri(options.BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            return services;
        }

        private static IServiceCollection AddImplementations(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();

            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind Database settings (optional)
            services.Configure<DatabaseSettings>(configuration.GetSection(DatabaseSettings.SectionName));

            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                var dbSettings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                var connectionString = dbSettings.DefaultConnection ?? configuration.GetConnectionString("DefaultConnection");

                options.UseSqlServer(connectionString);
            });

            return services;
        }
    }
}
