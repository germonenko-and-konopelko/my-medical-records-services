using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MMR.Common.Api.FeatureManagement;

public static class FeatureManagementEndpointBuilderExtensions
{
    public static TBuilder AddFeatureEndpointFilter<TBuilder>(
        this TBuilder endpointBuilder,
        string featureFlagName
    ) where TBuilder : IEndpointConventionBuilder
        => endpointBuilder
            .WithMetadata(new FeatureEndpointMetadata(featureFlagName))
            .AddEndpointFilter<TBuilder, FeatureGateEndpointFilter>();
}