using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

namespace MMR.Common.Api.FeatureManagement;

public class FeatureGateEndpointFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var featureMetadata = context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<FeatureEndpointMetadata>();
        if (featureMetadata is null)
        {
            return await next(context);
        }

        var featureManager = context.HttpContext.RequestServices.GetRequiredService<IFeatureManager>();
        if (await featureManager.IsEnabledAsync(featureMetadata.FeatureName))
        {
            return await next(context);
        }

        return Results.BadRequest();
    }
}