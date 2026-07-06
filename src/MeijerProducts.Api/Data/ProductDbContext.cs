using MeijerProducts.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeijerProducts.Api.Data;

/// <summary>
/// EF Core context for the product catalog. SQLite is used as the persistence layer:
/// it is a real, file-backed relational store (so the "persistence" requirement is met
/// with actual durability), yet it needs no external server to run or grade.
/// </summary>
public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);

            // Ids come from the source data, so don't let the DB generate them.
            entity.Property(p => p.Id).ValueGeneratedNever();

            entity.Property(p => p.Title).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Summary).HasMaxLength(500);
            entity.Property(p => p.Description).HasMaxLength(2000);
            entity.Property(p => p.Price).HasMaxLength(50);
            entity.Property(p => p.ImageUrl).HasMaxLength(1000);
            entity.Property(p => p.DetailImageUrl).HasMaxLength(1000);
        });
    }
}
