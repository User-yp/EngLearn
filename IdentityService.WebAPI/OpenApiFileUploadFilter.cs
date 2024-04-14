using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IdentityService.WebAPI;

public class OpenApiFileUploadFilter: IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.RequestBody != null)
        {
            foreach (var parameter in operation.RequestBody.Content.Where(c => c.Key.Contains("multipart/form-data")))
            {
                var schema = parameter.Value.Schema;
                if (schema.Properties != null)
                {
                    foreach (var prop in schema.Properties)
                    {
                        if (prop.Value.Format == "binary")
                        {
                            prop.Value.Extensions.Add("x-ms-contentType", new OpenApiString("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));
                        }
                    }
                }
            }
        }
    }
}
