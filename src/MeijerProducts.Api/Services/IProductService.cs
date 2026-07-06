using MeijerProducts.Api.Models.Dtos;

namespace MeijerProducts.Api.Services;

/// <summary>
/// Application service that turns persisted products into the DTOs the API exposes.
/// This is where any future business rules (filtering, pricing logic, enrichment) would live,
/// keeping controllers thin and repositories focused on data access.
/// </summary>
public interface IProductService
{
    Task<IReadOnlyList<ProductSummaryDto>> GetProductsAsync(CancellationToken cancellationToken = default);

    Task<ProductDetailDto?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default);
}
