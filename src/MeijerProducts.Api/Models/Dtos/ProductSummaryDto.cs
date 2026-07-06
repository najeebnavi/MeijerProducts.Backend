namespace MeijerProducts.Api.Models.Dtos;

/// <summary>
/// Lightweight product shape returned by the list endpoint. Matches the "Products API"
/// sample response: id, imageUrl, summary, title.
/// </summary>
public record ProductSummaryDto
{
    public int Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Summary { get; init; } = string.Empty;

    public string ImageUrl { get; init; } = string.Empty;
}
