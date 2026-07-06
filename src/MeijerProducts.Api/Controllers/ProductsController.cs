using MeijerProducts.Api.Models.Dtos;
using MeijerProducts.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeijerProducts.Api.Controllers;

/// <summary>
/// Endpoints for the product catalog:
///   GET /api/products        -> list of product summaries  (Products API)
///   GET /api/products/{id}   -> full product detail         (Product Detail API)
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>Returns all products as lightweight summaries for the list screen.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ProductSummaryDto>>> GetProducts(
        CancellationToken cancellationToken)
    {
        var products = await _productService.GetProductsAsync(cancellationToken);
        return Ok(products);
    }

    /// <summary>Returns the full detail for a single product, or 404 if it doesn't exist.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDetailDto>> GetProductById(
        int id, CancellationToken cancellationToken)
    {
        var product = await _productService.GetProductByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return Problem(
                title: "Product not found",
                detail: $"No product exists with id {id}.",
                statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(product);
    }
}
