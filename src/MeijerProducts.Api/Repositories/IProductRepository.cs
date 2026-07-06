using MeijerProducts.Api.Models.Entities;

namespace MeijerProducts.Api.Repositories;

/// <summary>
/// Abstraction over product persistence. Keeping data access behind an interface lets the
/// service layer stay ignorant of EF Core and lets tests substitute a fake or in-memory store.
/// </summary>
public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
