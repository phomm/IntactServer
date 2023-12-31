using Intact.API.Bootstrap;
using Intact.BusinessLogic.Data.Config;
using Intact.BusinessLogic.Data.Redis;
using Intact.BusinessLogic.Data.RedisDI;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetupConfiguration();
builder.Services.AddConfigOptionsAndBind<RedisSettings>(builder.Configuration, nameof(RedisSettings), out var redisSettings);

builder.AddConfigOptions<DbSettings>();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwagger();
builder.Services.AddInternalServices(builder.Configuration);
builder.Services.AddRedis(redisSettings);

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllers();

app.StartRedisInfrastructure(redisSettings.UseInMemoryCache);

app.Run();
