using EcoTrack.Domain.Entities;
namespace EcoTrack.Domain.Repositories;
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<(IReadOnlyList<Product> Items, int Total)> SearchAsync(string? q, string? category, int page, int pageSize, CancellationToken ct);
    Task AddAsync(Product product, CancellationToken ct);
    Task UpdateAsync(Product product, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}
