namespace MeijerProducts.Api.Models.Dtos;

/// <summary>
/// Full product shape returned by the detail endpoint. Matches the "Product Detail API"
/// sample response: description, id, imageUrl, price, summary, title.
/// </summary>
public record ProductDetailDto
{
    public int Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Summary { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string Price { get; init; } = string.Empty;

    /// <summary>Full-resolution image for the detail screen.</summary>
    public string ImageUrl { get; init; } = string.Empty;
}
