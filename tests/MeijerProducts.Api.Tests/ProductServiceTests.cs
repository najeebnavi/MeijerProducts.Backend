using FluentAssertions;
using MeijerProducts.Api.Models.Entities;
using MeijerProducts.Api.Repositories;
using MeijerProducts.Api.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace MeijerProducts.Api.Tests;

public class ProductServiceTests
{
    private static Product SampleBanana() => new()
    {
        Id = 0,
        Title = "Bananas",
        Summary = "Fresh bananas, perfect for a healthy snack.",
        Description = "Fresh bananas, perfect for a healthy snack. Rich in potassium and vitamins.",
        Price = "$0.59/lb",
        ImageUrl = "https://example.com/banana-thumb.jpg",
        DetailImageUrl = "https://example.com/banana-full.png",
    };

    private static ProductService CreateService(Mock<IProductRepository> repo) =>
        new(repo.Object, NullLogger<ProductService>.Instance);

    [Fact]
    public async Task GetProductsAsync_maps_entities_to_summary_dtos()
    {
        var repo = new Mock<IProductRepository>();
        repo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { SampleBanana() });

        var result = await CreateService(repo).GetProductsAsync();

        result.Should().HaveCount(1);
        var dto = result[0];
        dto.Id.Should().Be(0);
        dto.Title.Should().Be("Bananas");
        dto.Summary.Should().Be("Fresh bananas, perfect for a healthy snack.");
        // The list uses the thumbnail image, not the detail image.
        dto.ImageUrl.Should().Be("https://example.com/banana-thumb.jpg");
    }

    [Fact]
    public async Task GetProductByIdAsync_returns_detail_dto_with_full_image_and_price()
    {
        var repo = new Mock<IProductRepository>();
        repo.Setup(r => r.GetByIdAsync(0, It.IsAny<CancellationToken>()))
            .ReturnsAsync(SampleBanana());

        var dto = await CreateService(repo).GetProductByIdAsync(0);

        dto.Should().NotBeNull();
        dto!.Description.Should().Contain("potassium");
        dto.Price.Should().Be("$0.59/lb");
        // The detail screen uses the full-resolution image.
        dto.ImageUrl.Should().Be("https://example.com/banana-full.png");
    }

    [Fact]
    public async Task GetProductByIdAsync_returns_null_when_not_found()
    {
        var repo = new Mock<IProductRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var dto = await CreateService(repo).GetProductByIdAsync(9999);

        dto.Should().BeNull();
    }
}
