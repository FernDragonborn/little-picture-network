using LittlePictureNetworkBackend;
using LittlePictureNetworkBackend.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace LittlePictureNetworkBackend_tests.Integration_Tests;
class IntegrationTestWebAppFactory : WebApplicationFactory<Program>
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services
                .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<PictureNetworkDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<PictureNetworkDbContext>(options =>
            {
                options.UseSqlServer("???");
            });
        });
    }
}

