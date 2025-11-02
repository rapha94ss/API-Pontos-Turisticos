using Microsoft.EntityFrameworkCore;
using TouristSpots.Domain.Entities;

namespace TouristSpots.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<TouristSpot> TouristSpots => Set<TouristSpot>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var e = modelBuilder.Entity<TouristSpot>();
        e.ToTable("TouristSpots");
        e.HasKey(x => x.Id);
        e.Property(x => x.Name).IsRequired().HasMaxLength(200);
        e.Property(x => x.Description).IsRequired().HasMaxLength(100);
        e.Property(x => x.Location).IsRequired().HasMaxLength(300);
        e.Property(x => x.City).IsRequired().HasMaxLength(200);
        e.Property(x => x.State).IsRequired().HasMaxLength(2);
        e.Property(x => x.CreatedAt).IsRequired();
    }
}
