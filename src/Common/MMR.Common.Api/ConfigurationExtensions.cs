using Microsoft.Extensions.Configuration;

namespace MMR.Common.Api;

public static class ConfigurationExtensions
{
    public static T GetRequiredValue<T>(this IConfiguration configuration, string key)
    {
        var configurationItem = configuration.GetValue<T>(key);
        if (configurationItem is null)
        {
            throw new InvalidOperationException($"Configuration item {key} was not found.");
        }

        return configurationItem;
    }
}