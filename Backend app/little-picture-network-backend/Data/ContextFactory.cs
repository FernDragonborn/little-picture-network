using Microsoft.EntityFrameworkCore;

namespace LittlePictureNetworkBackend.Data;

public class ContextFactory
{
    //TODO remove connection string to other place
    private static readonly DbContextOptions Options = new DbContextOptionsBuilder<PictureNetworkDbContext>()
        .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestProjectInforce;Persist Security Info=True;User ID=app_connection_login;Password=123456")
        .UseLazyLoadingProxies()
        .Options;
    public static PictureNetworkDbContext CreateNew()
    {
        return new PictureNetworkDbContext(Options);
    }
}
