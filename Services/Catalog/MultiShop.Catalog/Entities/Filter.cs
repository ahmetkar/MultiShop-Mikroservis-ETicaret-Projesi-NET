using MongoDB.Bson.Serialization.Attributes;

namespace MultiShop.Catalog.Entities
{
    public class Filter
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string FilterId { get; set; }

        public string FilterTitle { get; set; }

        public string FilterName { get; set; }
    }
}
