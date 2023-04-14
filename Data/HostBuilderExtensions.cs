using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Data
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureDatabase(this IHostBuilder hostBuilder) =>

        hostBuilder.ConfigureServices((hostContext, services) =>
        {
            // just a simple sql database, with more time this would have been a cloud hosted document db to better suit the data
            // but for a demo, i kept it simple and ran on a local db
            services.AddDbContext<IpContext>(options =>
                options.UseSqlServer(hostContext.Configuration.GetConnectionString("SqlConnectionString")));
        });

    }
}
