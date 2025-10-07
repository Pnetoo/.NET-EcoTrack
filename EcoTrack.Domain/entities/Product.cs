namespace EcoTrack.Domain.Entities;
public class Product
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = default!;
    public string Category { get; private set; } = default!;
    public decimal? CaloriesPer100g { get; private set; }
    public decimal? Co2PerUnit { get; private set; }
    public string? Barcode { get; private set; }
    public Product(string name, string category, decimal? calories, decimal? co2, string? barcode)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required");
        if (string.IsNullOrWhiteSpace(category)) throw new ArgumentException("Category is required");
        Name = name.Trim(); Category = category.Trim();
        CaloriesPer100g = calories; Co2PerUnit = co2; Barcode = barcode?.Trim();
    }
}
