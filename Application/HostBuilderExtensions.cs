using Application.Handlers;
using Application.Interfaces;
using Application.Queries;
using Client.Abstractions.Models;
using Cqrs.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureApplication(this IHostBuilder hostBuilder) =>

            hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddScoped<IRequestHandler<GetIpQuery, IpData>, GetIpQueryHandler>();
                services.AddScoped<IIpDataRepository, IpDataRepository>();
            });

    }
}