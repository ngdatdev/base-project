using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common.Filters;

/// <summary>
/// Swagger file upload
/// </summary>
public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileParams = context
            .MethodInfo.GetParameters()
            .Where(p =>
                p.ParameterType == typeof(IFormFile)
                || p.ParameterType == typeof(IEnumerable<IFormFile>)
            )
            .ToList();

        if (!fileParams.Any())
            return;

        foreach (var param in fileParams)
        {
            var mediaType = param.ParameterType == typeof(IFormFile) ? "image/*" : "image/*";
            operation.RequestBody = new OpenApiRequestBody
            {
                Content =
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties =
                            {
                                [param.Name] = new OpenApiSchema
                                {
                                    Type = "string",
                                    Format = "binary"
                                }
                            },
                            Required = new HashSet<string> { param.Name }
                        }
                    }
                }
            };
        }
    }
}
