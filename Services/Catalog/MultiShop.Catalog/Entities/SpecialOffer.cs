using MongoDB.Bson.Serialization.Attributes;

namespace MultiShop.Catalog.Entities
{
    public class SpecialOffer
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string SpecialOfferId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string ImageUrl { get; set; }
    }
}
