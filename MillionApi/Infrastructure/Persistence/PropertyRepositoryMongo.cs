using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MillionApi.Domain.Entities;
using MillionApi.Domain.Interfaces;

namespace MillionApi.Infrastructure.Persistence
{
    public class PropertyRepositoryMongo : IPropertyRepository
    {
        private readonly IMongoCollection<Property> _propertiesCollection;

        public PropertyRepositoryMongo(IOptions<MongoDBSettings> mongoSettings)
        {
            var settings = mongoSettings.Value;

            if (string.IsNullOrEmpty(settings.ConnectionString))
                throw new ArgumentException("ConnectionString cannot be null or empty");

            if (string.IsNullOrEmpty(settings.DatabaseName))
                throw new ArgumentException("DatabaseName cannot be null or empty");

            if (string.IsNullOrEmpty(settings.CollectionName))
                throw new ArgumentException("CollectionName cannot be null or empty");

            var client = new MongoClient(mongoSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoSettings.Value.DatabaseName);
            _propertiesCollection = database.GetCollection<Property>(settings.CollectionName);
        }

        public async Task<Property> GetById(string id)
        {
            return await _propertiesCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Property>> GetAll()
        {
            return await _propertiesCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Property> CreateAsync(Property property)
        {
            await _propertiesCollection.InsertOneAsync(property);
            return property;
        }

        public async Task<List<Property>> GetAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice)
        {
            var filter = Builders<Property>.Filter.Empty;

            if (!string.IsNullOrEmpty(name))
            {
                filter &= Builders<Property>.Filter.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));
            }

            if (!string.IsNullOrEmpty(address))
            {
                filter &= Builders<Property>.Filter.Regex(p => p.AddressProperty, new MongoDB.Bson.BsonRegularExpression(address, "i"));
            }

            if (minPrice.HasValue)
            {
                filter &= Builders<Property>.Filter.Gte(p => p.PriceProperty, minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                filter &= Builders<Property>.Filter.Lte(p => p.PriceProperty, maxPrice.Value);
            }

            return await _propertiesCollection.Find(filter).ToListAsync();
        }

        public async Task Add(Property property)
        {
            await _propertiesCollection.InsertOneAsync(property);
        }

        public async Task Update(Property property)
        {
            await _propertiesCollection.ReplaceOneAsync(p => p.Id == property.Id, property);
        }

        public async Task Delete(string id)
        {
            await _propertiesCollection.DeleteOneAsync(p => p.Id == id);
        }
    }
}
