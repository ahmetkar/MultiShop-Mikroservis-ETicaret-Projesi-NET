using MongoDB.Bson.Serialization.Attributes;

namespace MultiShop.Catalog.Entities
{
    public class ProductFilter
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ProductFilterId { get; set; }

        public string CategoryId { get; set; }

        public string FilterId { get; set; }

        [BsonIgnore]
        public Category Category { get; set; }

        [BsonIgnore]
        public Filter Filter { get; set; }
    }
}
