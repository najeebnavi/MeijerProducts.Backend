using System.Text.Json;
using MeijerProducts.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeijerProducts.Api.Data;

/// <summary>
/// Ensures the database exists and is populated from the bundled seed file
/// (<c>Data/Seed/products-seed.json</c>) on startup. Seeding is idempotent: it only
/// runs when the Products table is empty, so restarts don't duplicate rows.
/// </summary>
public static class DbSeeder
{
    private static readonly JsonSerializerOptions SeedJsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public static async Task SeedAsync(
        ProductDbContext context,
        IWebHostEnvironment environment,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        // For this exercise we create the schema directly. In a production system this
        // would be `await context.Database.MigrateAsync()` with checked-in migrations.
        await context.Database.EnsureCreatedAsync(cancellationToken);

        if (await context.Products.AnyAsync(cancellationToken))
        {
            logger.LogInformation("Product catalog already seeded; skipping.");
            return;
        }

        var seedPath = Path.Combine(environment.ContentRootPath, "Data", "Seed", "products-seed.json");
        if (!File.Exists(seedPath))
        {
            logger.LogWarning("Seed file not found at {SeedPath}; starting with an empty catalog.", seedPath);
            return;
        }

        await using var stream = File.OpenRead(seedPath);
        var products = await JsonSerializer.DeserializeAsync<List<Product>>(
            stream, SeedJsonOptions, cancellationToken) ?? new List<Product>();

        if (products.Count == 0)
        {
            logger.LogWarning("Seed file contained no products.");
            return;
        }

        await context.Products.AddRangeAsync(products, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Seeded {Count} products into the catalog.", products.Count);
    }
}
