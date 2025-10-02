using MillionApi.Domain.Entities;
using MillionApi.Domain.Interfaces;

namespace MillionApi.Application.Services
{
    public class PropertyService
    {
        private readonly IPropertyRepository _repository;

        public PropertyService(IPropertyRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Property>> GetProperties(string? name, string? address, decimal? minPrice, decimal? maxPrice)
        {
            return _repository.GetAsync(name, address, minPrice, maxPrice);
        }

        public Task<Property> CreateProperty(Property property)
        {
            return _repository.CreateAsync(property);
        }
    }
}
