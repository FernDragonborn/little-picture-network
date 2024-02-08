using LittlePictureNetworkBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LittlePictureNetworkBackend_tests
{
    public class DatabaseSetup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure services for testing, e.g., using a Dockerized SQL Server
            services.AddDbContext<PictureNetworkDbContext>(options =>
                options.UseSqlServer("Server=localhost,1433;Database=YourDb;User=YourUser;Password=YourPassword"));

            // Add other services needed for testing

        }
    }
}