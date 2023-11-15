using Microsoft.EntityFrameworkCore;

namespace test_project_Inforce_backend.Data
{
    public class ContextFactory
    {
        //TODO remove connection string to other place
        private static readonly DbContextOptions Options = new DbContextOptionsBuilder<TestProjectDbContext>()
            .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestProjectInforce;Persist Security Info=True;User ID=app_connection_login;Password=123456")
            .UseLazyLoadingProxies()
            .Options;
        public static TestProjectDbContext CreateNew()
        {
            return new TestProjectDbContext(Options);
        }
    }
}
