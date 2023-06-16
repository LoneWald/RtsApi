using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RtsApi.Extensions
{
    public class SwaggerSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(LoginModel))
            {
                schema.Example = new OpenApiObject()
                {
                    ["Email"] = new OpenApiString("Masha@yandex.ru"),
                    ["Password"] = new OpenApiString("Password"),
                };
            }
            else
                schema.Example = new OpenApiObject();
        }
    }
}
