using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// Swagger operation filter to support file upload (multipart/form-data) in Swagger UI.
/// </summary>
public class FileUploadOperationFilter : IOperationFilter
{
    /// <summary>
    /// Applies the operation filter to modify the Swagger documentation for endpoints that accept file uploads.
    /// </summary>
    /// <param name="operation">The OpenAPI operation to modify.</param>
    /// <param name="context">The current operation filter context.</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var parameters = context.ApiDescription.ParameterDescriptions;

        // Prepare schema properties for multipart/form-data
        var properties = new Dictionary<string, OpenApiSchema>();
        var requiredProperties = new HashSet<string>();

        foreach (var param in parameters)
        {
            if (param.Type == typeof(IFormFile))
            {
                // 🔸 For file parameters
                properties[param.Name] = new OpenApiSchema
                {
                    Type = "string",
                    Format = "binary"
                };
                requiredProperties.Add(param.Name);
            }
            else if (param.Source?.Id == "Form")
            {
                // 🔸 For additional form fields
                properties[param.Name] = new OpenApiSchema
                {
                    Type = "string"
                };
                requiredProperties.Add(param.Name);
            }
        }

        // If there are no file/form parameters, do nothing
        if (properties.Count == 0)
            return;

        // Add request body schema to the operation
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
