using EcoTrack.Application.Dtos;
using EcoTrack.Domain.Entities;
using EcoTrack.Domain.Repositories;

namespace EcoTrack.Application.Services;
public class ProductService(IProductRepository repo)
{
    public async Task<ProductDto?> GetAsync(Guid id, CancellationToken ct)
        => (await repo.GetByIdAsync(id, ct)) is { } p ? Map(p) : null;

    public async Task<(IReadOnlyList<ProductDto> Items, int Total)> SearchAsync(string? q, string? category, int page, int pageSize, CancellationToken ct)
    {
        var (items, total) = await repo.SearchAsync(q, category, page, pageSize, ct);
        return (items.Select(Map).ToList(), total);
    }

    public async Task<Guid> CreateAsync(CreateProductRequest r, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(r.Name)) throw new ArgumentException("Name is required");
        if (string.IsNullOrWhiteSpace(r.Category)) throw new ArgumentException("Category is required");
        var e = new Product(r.Name, r.Category, r.CaloriesPer100g, r.Co2PerUnit, r.Barcode);
        await repo.AddAsync(e, ct);
        return e.Id;
    }

    public async Task UpdateAsync(Guid id, UpdateProductRequest r, CancellationToken ct)
    {
        _ = await repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Product not found");
        var e = new Product(r.Name, r.Category, r.CaloriesPer100g, r.Co2PerUnit, r.Barcode);
        typeof(Product).GetProperty(nameof(Product.Id))!.SetValue(e, id);
        await repo.UpdateAsync(e, ct);
    }

    public Task DeleteAsync(Guid id, CancellationToken ct) => repo.DeleteAsync(id, ct);
    private static ProductDto Map(Product p) => new(p.Id, p.Name, p.Category, p.CaloriesPer100g, p.Co2PerUnit, p.Barcode);
}
