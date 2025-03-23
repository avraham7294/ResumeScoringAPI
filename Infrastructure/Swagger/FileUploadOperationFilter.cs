using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var parameters = context.ApiDescription.ParameterDescriptions;

        // We will manually construct the schema for multipart/form-data
        var properties = new Dictionary<string, OpenApiSchema>();
        var requiredProperties = new HashSet<string>();

        foreach (var param in parameters)
        {
            if (param.Type == typeof(IFormFile))
            {
                properties[param.Name] = new OpenApiSchema
                {
                    Type = "string",
                    Format = "binary"
                };
                requiredProperties.Add(param.Name);
            }
            else if (param.Source?.Id == "Form")
            {
                properties[param.Name] = new OpenApiSchema
                {
                    Type = "string"
                };
                requiredProperties.Add(param.Name);
            }
        }

        if (properties.Count == 0)
            return;

        operation.RequestBody = new OpenApiRequestBody
        {
            Required = true,
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = properties,
                        Required = requiredProperties
                    }
                }
            }
        };
    }
}
