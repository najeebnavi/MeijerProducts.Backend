using FluentAssertions;
using MeijerProducts.Api.Controllers;
using MeijerProducts.Api.Models.Dtos;
using MeijerProducts.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MeijerProducts.Api.Tests;

public class ProductsControllerTests
{
    [Fact]
    public async Task GetProducts_returns_200_with_the_service_result()
    {
        var service = new Mock<IProductService>();
        var expected = new List<ProductSummaryDto>
        {
            new() { Id = 0, Title = "Bananas", Summary = "Fresh bananas.", ImageUrl = "x" },
        };
        service.Setup(s => s.GetProductsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var controller = new ProductsController(service.Object);

        var result = await controller.GetProducts(CancellationToken.None);

        var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetProductById_returns_200_when_found()
    {
        var service = new Mock<IProductService>();
        var dto = new ProductDetailDto { Id = 0, Title = "Bananas", Price = "$0.59/lb" };
        service.Setup(s => s.GetProductByIdAsync(0, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var controller = new ProductsController(service.Object);

        var result = await controller.GetProductById(0, CancellationToken.None);

        var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().Be(dto);
    }

    [Fact]
    public async Task GetProductById_returns_404_problem_when_missing()
    {
        var service = new Mock<IProductService>();
        service.Setup(s => s.GetProductByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProductDetailDto?)null);

        var controller = new ProductsController(service.Object);

        var result = await controller.GetProductById(9999, CancellationToken.None);

        var problem = result.Result.Should().BeOfType<ObjectResult>().Subject;
        problem.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        problem.Value.Should().BeOfType<ProblemDetails>();
    }
}
