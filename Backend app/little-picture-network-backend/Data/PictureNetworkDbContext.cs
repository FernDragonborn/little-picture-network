using Microsoft.EntityFrameworkCore;
using LittlePictureNetworkBackend.Models;

namespace LittlePictureNetworkBackend.Data;
public class PictureNetworkDbContext : DbContext
{
    public PictureNetworkDbContext(DbContextOptions options) : base(options)
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
