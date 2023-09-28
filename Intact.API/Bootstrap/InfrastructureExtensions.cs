using Microsoft.OpenApi.Models;

namespace Intact.API.Bootstrap;

public static class InfrastructureExtensions
{
    public static ConfigurationManager SetupConfiguration(this ConfigurationManager manager)
    {
        manager.AddJsonFile($"secrets.json", true, true);
        return manager;
    }

    public static IServiceCollection AddConfigOptionsAndBind<TOptions>(this IServiceCollection services,
        IConfiguration configuration, string section, out TOptions instance) where TOptions : class, new()
    {
        var configSection = configuration.GetSection(section);

        var options = new TOptions();
        configSection.Bind(options);
        instance = options;

        services.Configure<TOptions>(configSection);
        return services;
    }

    public static WebApplicationBuilder AddConfigOptions<TOptions>(this WebApplicationBuilder builder) where TOptions : class, new()
    {
        var configSection = builder.Configuration.GetSection(typeof(TOptions).Name);
        
        builder.Services.Configure<TOptions>(configSection);
        return builder;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection serviceCollection)
    {
        const string version = "v1";
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(version,
                new OpenApiInfo
                {
                    Title = "Intact API Documentation",
                    Version = version
                });
            options.IncludeXmlComments(Path.ChangeExtension(typeof(Program).Assembly.Location, "xml"));
        });

        return serviceCollection;
    }
        
    /*
    public static IServiceCollection AddRedis(this IServiceCollection serviceCollection, RedisSettings redisSettings)
    {
        //Redis
        serviceCollection.AddSingleton<IRedisSettings, RedisConfiguration>();
        serviceCollection.RegisterRedisServices(redisSettings.UseInMemoryCache);
        serviceCollection.AddScoped<IRedisCacheRepository, RedisCacheRepository>();

        return serviceCollection;
    }

    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<Program>()
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();
        services.AddScoped<IValidator<ResultRequest>, ResultRequestValidator>();

        return services;
    }
    */
}