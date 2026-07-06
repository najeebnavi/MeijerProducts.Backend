using MeijerProducts.Api.Data;
using MeijerProducts.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeijerProducts.Api.Repositories;

/// <summary>
/// EF Core-backed implementation of <see cref="IProductRepository"/>.
/// Reads are issued <c>AsNoTracking</c> since the API never mutates the entities it returns.
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly ProductDbContext _context;

    public ProductRepository(ProductDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}
