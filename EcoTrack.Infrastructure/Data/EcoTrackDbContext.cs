using Microsoft.EntityFrameworkCore;
using EcoTrack.Domain.Entities;

namespace EcoTrack.Infrastructure.Data;

public class EcoTrackDbContext(DbContextOptions<EcoTrackDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Product>(e =>
        {
            e.ToTable("PRODUCTS");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("ID");
            e.Property(x => x.Name).HasMaxLength(200).IsRequired().HasColumnName("NAME");
            e.Property(x => x.Category).HasMaxLength(120).IsRequired().HasColumnName("CATEGORY");
            e.Property(x => x.CaloriesPer100g).HasColumnType("NUMBER(10,2)").HasColumnName("KCAL_100G");
            e.Property(x => x.Co2PerUnit).HasColumnType("NUMBER(10,3)").HasColumnName("CO2_PER_UNIT");
            e.Property(x => x.Barcode).HasMaxLength(64).HasColumnName("BARCODE");
        });
    }
}
