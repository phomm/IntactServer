using Intact.API.Bootstrap;
using Intact.BusinessLogic.Data.Config;
using Intact.BusinessLogic.Data.Redis;
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

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHealthChecks("/_health", new HealthCheckOptions
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
