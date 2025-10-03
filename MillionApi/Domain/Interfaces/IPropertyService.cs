using MillionApi.Domain.Entities;

namespace MillionApi.Domain.Interfaces;

public interface IPropertyService
{
    Task<Property> GetPropertyById(string id);
    Task<List<Property>> GetProperties(string? name, string? address, decimal? minPrice, decimal? maxPrice);
    Task<Property> CreateProperty(Property property);
}