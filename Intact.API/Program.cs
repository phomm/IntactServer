using Intact.API.Bootstrap;
using Intact.API.Health;
using Intact.BusinessLogic.Data.Config;
using Intact.BusinessLogic.Data.Redis;
using Intact.BusinessLogic.Models;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using System.Text.Json.Serialization;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetupConfiguration();
builder.Services.AddConfigOptionsAndBind<RedisSettings>(builder.Configuration, nameof(RedisSettings), out var redisSettings);
builder.AddConfigOptions<DbSettings>();

// Add services to the container.
builder.Services
    .AddHealth(builder.Configuration)
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });
builder.Services
    .AddProblemDetails()
    .AddSwagger()
    .AddInternalServices()
    .AddDbServices(builder.Configuration)
    .AddRedis(redisSettings)
    .CustomizeAuthorization()
    .CustomizeAuthentication();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHealthChecks(HealthCheckConsts.EndpointPath, new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "Intact API Documentation";
        options.EnableDeepLinking();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGroup("api").MapIdentityApi<User>();

app.MapControllers();

app.StartRedis(redisSettings.UseInMemoryCache);

app.Run();
