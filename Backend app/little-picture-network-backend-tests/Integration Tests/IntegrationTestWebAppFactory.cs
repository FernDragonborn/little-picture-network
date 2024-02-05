using LittlePictureNetworkBackend;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;

namespace LittlePictureNetworkBackend_tests.Integration_Tests
{
    public sealed class IntegrationTestWebAppFactory<TDbContext> : WebApplicationFactory<Program> where TDbContext : DbContext
    {
        private readonly string _connectionString;

        public IntegrationTestWebAppFactory(IntegrationTests_AlbumController controllerFixture)
        {
            _connectionString = controllerFixture._msSqlContainer.GetConnectionString();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.Remove(services.SingleOrDefault(service => typeof(DbContextOptions<TDbContext>) == service.ServiceType));
                services.Remove(services.SingleOrDefault(service => typeof(DbConnection) == service.ServiceType));
                services.AddDbContext<TDbContext>((_, option) => option.UseSqlServer(_connectionString));
            });
        }
    }
}
