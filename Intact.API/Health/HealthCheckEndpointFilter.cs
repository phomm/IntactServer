using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Intact.API.Health;
internal class HealthCheckEndpointFilter : IDocumentFilter
{
    private const string Path = HealthCheckConsts.EndpointPath;

    /// <inheritdoc />
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var openApiPathItem1 = new OpenApiPathItem();
        var operation = new OpenApiOperation
        {
            Tags = new List<OpenApiTag>
            {
                new() { Name = "Monitoring" }
            },
            Responses = new OpenApiResponses
            {
                {
                    "200", new OpenApiResponse
                    {
                        Description = "Success"
                    }
                },
                {
                    "503", new OpenApiResponse
                    {
                        Description = "Node or one of critical resources is down"
                    }
                }
            }
        };
        openApiPathItem1.AddOperation(OperationType.Get, operation);
        swaggerDoc.Paths.Add(Path, openApiPathItem1);
    }
}