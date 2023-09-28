using Intact.API.Bootstrap;
using Intact.BusinessLogic.Data.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetupConfiguration();

builder.AddConfigOptions<DbSettings>();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwagger();
builder.Services.AddInternalServices(builder.Configuration);

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

app.Run();
