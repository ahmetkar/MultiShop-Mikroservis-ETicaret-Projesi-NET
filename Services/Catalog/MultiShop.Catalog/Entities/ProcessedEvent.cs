using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MultiShop.Catalog.Entities
{
    [BsonIgnoreExtraElements]
    public class ProcessedEvent
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.String)]
        public string EventId { get; set; } = string.Empty;

        public string HandlerName { get; set; } = string.Empty;

        public DateTime ProcessedAt { get; set; }
    }
}

