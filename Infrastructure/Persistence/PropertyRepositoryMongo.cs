using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MillionApi.Domain.Entities;
using MillionApi.Domain.Interfaces;

namespace MillionApi.Infrastructure.Persistence
{
    public class PropertyRepositoryMongo : IPropertyRepository
    {
        private readonly IMongoCollection<Property> _properties;

        public PropertyRepositoryMongo(IOptions<MongoDBSettings> mongoSettings)
        {
            var settings = mongoSettings.Value;

            // Validaciones
            if (string.IsNullOrEmpty(settings.ConnectionString))
                throw new ArgumentException("ConnectionString cannot be null or empty");

            if (string.IsNullOrEmpty(settings.DatabaseName))
                throw new ArgumentException("DatabaseName cannot be null or empty");

            if (string.IsNullOrEmpty(settings.CollectionName))
                throw new ArgumentException("CollectionName cannot be null or empty");

            var client = new MongoClient(mongoSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoSettings.Value.DatabaseName);
            _properties = database.GetCollection<Property>(mongoSettings.Value.CollectionName);
        }

        public async Task<Property> GetById(string id)
        {
            return await _properties.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Property>> GetAll()
        {
            return await _properties.Find(_ => true).ToListAsync();
        }

        public async Task<Property> CreateAsync(Property property)
        {
            await _properties.InsertOneAsync(property);
            return property;
        }

        public async Task<List<Property>> GetAsync(string? city, string? type, decimal? minPrice, decimal? maxPrice)
        {
            var filterBuilder = Builders<Property>.Filter;
            var filter = filterBuilder.Empty;

            if (!string.IsNullOrEmpty(city))
                filter &= filterBuilder.Eq(p => p.AddressProperty, city);

            if (minPrice.HasValue)
                filter &= filterBuilder.Gte(p => p.PriceProperty, minPrice.Value);

            if (maxPrice.HasValue)
                filter &= filterBuilder.Lte(p => p.PriceProperty, maxPrice.Value);

            return await _properties.Find(filter).ToListAsync();
        }

        public async Task Add(Property property)
        {
            await _properties.InsertOneAsync(property);
        }

        public async Task Update(Property property)
        {
            await _properties.ReplaceOneAsync(p => p.Id == property.Id, property);
        }

        public async Task Delete(string id)
        {
            await _properties.DeleteOneAsync(p => p.Id == id);
        }
    }
}
