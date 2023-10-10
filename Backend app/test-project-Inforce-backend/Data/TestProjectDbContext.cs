using Microsoft.EntityFrameworkCore;
using test_project_Inforce_backend.Models;

namespace test_project_Inforce_backend.Data;
public class TestProjectDbContext : DbContext
{
    public TestProjectDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Photo> Photos { get; set; } = null!;

    public DbSet<Album> Albums { get; set; } = null!;


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        base.ConfigureConventions(builder);

        builder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter, DateOnlyComparer>()
            .HaveColumnType("date");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}
