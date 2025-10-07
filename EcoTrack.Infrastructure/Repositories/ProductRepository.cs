using EcoTrack.Domain.Entities;
using EcoTrack.Domain.Repositories;
using EcoTrack.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcoTrack.Infrastructure.Repositories;
public class ProductRepository(EcoTrackDbContext db) : IProductRepository
{
    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct) =>
        db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<(IReadOnlyList<Product> Items, int Total)> SearchAsync(string? q, string? category, int page, int pageSize, CancellationToken ct)
    {
        var query = db.Products.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(p => EF.Functions.Like(p.Name, $"%{q}%"));
        if (!string.IsNullOrWhiteSpace(category)) query = query.Where(p => p.Category == category);
        var total = await query.CountAsync(ct);
        var items = await query.OrderBy(p => p.Name).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items, total);
    }

    public async Task AddAsync(Product product, CancellationToken ct) { db.Products.Add(product); await db.SaveChangesAsync(ct); }
    public async Task UpdateAsync(Product product, CancellationToken ct) { db.Products.Update(product); await db.SaveChangesAsync(ct); }
    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var p = await db.Products.FindAsync(new object?[] { id }, ct);
        if (p is null) return; db.Products.Remove(p); await db.SaveChangesAsync(ct);
    }
}
