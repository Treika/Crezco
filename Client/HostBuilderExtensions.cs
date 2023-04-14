using Client;
using Client.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;

namespace Abstraction
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureAbstraction(this IHostBuilder hostBuilder) =>
            hostBuilder.ConfigureServices((context, services) =>
            {
                var refitSettings = new RefitSettings();
                var apilayerSettingsSection = context.Configuration.GetSection(nameof(ApiLayerSettings));
                var apiLayerSettings = new ApiLayerSettings(apilayerSettingsSection);
                services
                    .AddRefitClient<IIpLookupApi>(refitSettings)
                    .ConfigureHttpClient(c =>
                    {
                        c.BaseAddress = new Uri(apiLayerSettings.BaseAddress);
                        // would usually pull keys from local usersecrets or pipeline injection, but added here for simplicity
                        c.DefaultRequestHeaders.Add("apikey", apiLayerSettings.ApiKey);
                    });
            });
    }
}
