using Intact.API.Bootstrap;
using Intact.BusinessLogic.Data.Config;
using Intact.BusinessLogic.Data.Redis;
using Intact.BusinessLogic.Data.RedisDI;
using Intact.BusinessLogic.Models;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetupConfiguration();
builder.Services.AddConfigOptionsAndBind<RedisSettings>(builder.Configuration, nameof(RedisSettings), out var redisSettings);

builder.AddConfigOptions<DbSettings>();

// Add services to the container.
builder.Services.AddHealth(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddSwagger();
builder.Services.AddInternalServices();
builder.Services.AddDbServices(builder.Configuration);
builder.Services.AddRedis(redisSettings);

builder.Services.CustomizeAuthorization();
builder.Services.CustomizeAuthentication();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CustomCorsPolicy", config =>
    {
        config
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true);
    });
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHealthChecks("/_health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseDeveloperExceptionPage();

// Configure the HTTP request pipeline.
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

app.UseCors("CustomCorsPolicy");

app.UseAuthorization();

app.MapGroup("api").MapIdentityApi<User>();

app.MapControllers();

app.StartRedisInfrastructure(redisSettings.UseInMemoryCache);

app.Run();
