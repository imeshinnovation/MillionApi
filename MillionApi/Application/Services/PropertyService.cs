using MillionApi.Domain.Entities;
using MillionApi.Domain.Interfaces;

namespace MillionApi.Application.Services
{
    public class PropertyService(IPropertyRepository repository) : IPropertyService
    {
        private readonly IPropertyRepository _repository = repository;

        public Task<Property> GetPropertyById(string id)
        {
            return _repository.GetById(id);
        }

        public Task<List<Property>> GetProperties(string? name, string? address, decimal? minPrice, decimal? maxPrice)
        {
            if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
            {
                throw new ArgumentException("El precio mínimo no puede ser mayor al precio máximo");
            }
            return _repository.GetAsync(name, address, minPrice, maxPrice);
        }

        public Task<Property> CreateProperty(Property property)
        {
            return _repository.CreateAsync(property);
        }
    }
}
