using AbySalto.Mid.Application.Auth;
using AbySalto.Mid.Application.Auth.Authentification;
using AbySalto.Mid.Application.Auth.Interfaces;
using AbySalto.Mid.Application.Carts;
using AbySalto.Mid.Application.Favorites;
using AbySalto.Mid.Application.Services;
using AbySalto.Mid.Application.Services.External;
using AbySalto.Mid.Infrastructure.Authentication;
using AbySalto.Mid.Infrastructure.Configuration;
using AbySalto.Mid.Infrastructure.Persistence;
using AbySalto.Mid.Infrastructure.Repositories;
using AbySalto.Mid.Infrastructure.Services;
using AbySalto.Mid.Infrastructure.Services.External;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AbySalto.Mid.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabase(configuration);
            services.AddJwtAuthentication(configuration);
            services.AddServices(configuration);
            services.AddImplementations();
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
            // Make HttpContext accessible from infrastructure services
            services.AddHttpContextAccessor();
            // Current user abstraction
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            // Application services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddScoped<ICartService, CartService>();

            // Infrastructure services
            services.AddScoped<ITokenProvider, TokenProvider>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            services.AddScoped<ICartRepository, CartRepository>();

            return services;
        }

        private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

            var jwtSecret = configuration["Jwt:Secret"]
                ?? throw new InvalidOperationException("JWT Secret not configured.");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind Database settings (optional)
            services.Configure<DatabaseSettings>(configuration.GetSection(DatabaseSettings.SectionName));

            // Read connection string directly from configuration to avoid overload ambiguity
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            return services;
        }
    }
}
