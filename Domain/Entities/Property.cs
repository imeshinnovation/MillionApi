using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MillionApi.Domain.Entities
{
    public class Property
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string IdOwner { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string AddressProperty { get; set; } = string.Empty;
        public decimal PriceProperty { get; set; }

        public bool IsValidPrice()
        {
            return PriceProperty > 0;
        }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
