namespace MeijerProducts.Api.Models.Entities;

/// <summary>
/// Persistence entity for a product. This is the single source of truth stored in
/// the database. API responses are projected from this into purpose-built DTOs so the
/// storage shape and the wire shape can evolve independently.
/// </summary>
public class Product
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Price { get; set; } = string.Empty;

    /// <summary>Thumbnail image used on the list screen.</summary>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Full-resolution image used on the detail screen. The sample data ships a
    /// different asset for the detail view, so we keep both rather than collapsing them.
    /// </summary>
    public string DetailImageUrl { get; set; } = string.Empty;
}
