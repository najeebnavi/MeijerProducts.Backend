using MeijerProducts.Api.Models.Dtos;
using MeijerProducts.Api.Models.Entities;

namespace MeijerProducts.Api.Mapping;

/// <summary>
/// Hand-written projections from the <see cref="Product"/> entity to the API DTOs.
/// Kept explicit (rather than reaching for AutoMapper) because there are only two small
/// maps here — explicit projection is easier to read, is trivially unit-testable, and lets
/// EF Core translate the Select into SQL when used inside a query.
/// </summary>
public static class ProductMappingExtensions
{
    public static ProductSummaryDto ToSummaryDto(this Product product) => new()
    {
        Id = product.Id,
        Title = product.Title,
        Summary = product.Summary,
        ImageUrl = product.ImageUrl,
    };

    public static ProductDetailDto ToDetailDto(this Product product) => new()
    {
        Id = product.Id,
        Title = product.Title,
        Summary = product.Summary,
        Description = product.Description,
        Price = product.Price,
        // The detail screen shows the full-resolution asset.
        ImageUrl = product.DetailImageUrl,
    };
}
