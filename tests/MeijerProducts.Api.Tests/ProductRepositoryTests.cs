using FluentAssertions;
using MeijerProducts.Api.Data;
using MeijerProducts.Api.Models.Entities;
using MeijerProducts.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MeijerProducts.Api.Tests;

public class ProductRepositoryTests
{
    private static ProductDbContext CreateContext()
    {
        // A distinct in-memory database per test keeps them isolated.
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ProductDbContext(options);
    }

    private static async Task SeedAsync(ProductDbContext context)
    {
        context.Products.AddRange(
            new Product { Id = 2, Title = "Broccoli Crowns" },
            new Product { Id = 0, Title = "Bananas" },
            new Product { Id = 1, Title = "Gala Apples" });
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetAllAsync_returns_all_products_ordered_by_id()
    {
        await using var context = CreateContext();
        await SeedAsync(context);
        var repo = new ProductRepository(context);

        var products = await repo.GetAllAsync();

        products.Select(p => p.Id).Should().ContainInOrder(0, 1, 2);
    }

    [Fact]
    public async Task GetByIdAsync_returns_matching_product()
    {
        await using var context = CreateContext();
        await SeedAsync(context);
        var repo = new ProductRepository(context);

        var product = await repo.GetByIdAsync(1);

        product.Should().NotBeNull();
        product!.Title.Should().Be("Gala Apples");
    }

    [Fact]
    public async Task GetByIdAsync_returns_null_for_unknown_id()
    {
        await using var context = CreateContext();
        await SeedAsync(context);
        var repo = new ProductRepository(context);

        var product = await repo.GetByIdAsync(9999);

        product.Should().BeNull();
    }
}
