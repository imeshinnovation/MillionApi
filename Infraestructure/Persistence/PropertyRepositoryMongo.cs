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
            var client = new MongoClient(mongoSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoSettings.Value.DatabaseName);
            _properties = database.GetCollection<Property>(mongoSettings.Value.CollectionName);
        }

        public async Task<List<Property>> GetAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice)
        {
            var filter = Builders<Property>.Filter.Empty;

            if (!string.IsNullOrEmpty(name))
                filter &= Builders<Property>.Filter.Regex("Name", new MongoDB.Bson.BsonRegularExpression(name, "i"));

            if (!string.IsNullOrEmpty(address))
                filter &= Builders<Property>.Filter.Regex("AddressProperty", new MongoDB.Bson.BsonRegularExpression(address, "i"));

            if (minPrice.HasValue)
                filter &= Builders<Property>.Filter.Gte("PriceProperty", minPrice.Value);

            if (maxPrice.HasValue)
                filter &= Builders<Property>.Filter.Lte("PriceProperty", maxPrice.Value);

            return await _properties.Find(filter).ToListAsync();
        }

        public async Task<Property> CreateAsync(Property property)
        {
            await _properties.InsertOneAsync(property);
            return property;
        }
    }

    public class MongoDBSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CollectionName { get; set; } = null!;
    }
}
