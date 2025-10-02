using MillionApi.Domain.Entities;

namespace MillionApi.Domain.Interfaces
{
    public interface IPropertyRepository
    {
        Task<List<Property>> GetAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice);
        Task<Property> CreateAsync(Property property);
    }
}
