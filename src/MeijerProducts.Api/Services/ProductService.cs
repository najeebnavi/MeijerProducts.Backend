using MeijerProducts.Api.Mapping;
using MeijerProducts.Api.Models.Dtos;
using MeijerProducts.Api.Repositories;

namespace MeijerProducts.Api.Services;

/// <inheritdoc />
public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository repository, ILogger<ProductService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ProductSummaryDto>> GetProductsAsync(
        CancellationToken cancellationToken = default)
    {
        var products = await _repository.GetAllAsync(cancellationToken);
        _logger.LogInformation("Returning {Count} products for the list endpoint.", products.Count);

        return products.Select(p => p.ToSummaryDto()).ToList();
    }

    public async Task<ProductDetailDto?> GetProductByIdAsync(
        int id, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            _logger.LogInformation("Product {ProductId} was not found.", id);
            return null;
        }

        return product.ToDetailDto();
    }
}
