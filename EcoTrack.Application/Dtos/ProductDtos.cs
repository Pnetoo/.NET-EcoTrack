namespace EcoTrack.Application.Dtos;
public record ProductDto(Guid Id, string Name, string Category, decimal? CaloriesPer100g, decimal? Co2PerUnit, string? Barcode);
public record CreateProductRequest(string Name, string Category, decimal? CaloriesPer100g, decimal? Co2PerUnit, string? Barcode);
public record UpdateProductRequest(string Name, string Category, decimal? CaloriesPer100g, decimal? Co2PerUnit, string? Barcode);
