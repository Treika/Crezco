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

                services
                    .AddRefitClient<IIpLookupApi>(refitSettings)
                    .ConfigureHttpClient(c =>
                    {
                        c.BaseAddress = new Uri("https://api.apilayer.com");
                        c.DefaultRequestHeaders.Add("apikey", "RlbK2zrgzBlF4uAyqTLduLVA8TrfYeCk");
                    });
            });
    }
}
