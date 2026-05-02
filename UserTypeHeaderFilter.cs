using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.OpenApi;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BillingService.Attributes;

namespace BillingService.Swagger
{
    public class UserTypeHeaderFilter : IOpenApiOperationTransformer
    {
        public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
        {
            var hasRequireUserType = context.Description.ActionDescriptor?.EndpointMetadata
                ?.OfType<RequireUserTypeAttribute>()
                .Any() ?? false;

            if (!hasRequireUserType)
                return Task.CompletedTask;

            operation.Parameters ??= new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "usertype",
                In = ParameterLocation.Header,
                Description = "User type for authorization (must be 'billing')",
                Required = true,
                Schema = new OpenApiSchema { Type = "string" }
            });

            return Task.CompletedTask;
        }
    }
}