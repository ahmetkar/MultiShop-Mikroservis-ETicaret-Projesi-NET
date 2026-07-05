using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MultiShop.Catalog.DTOs.AboutDTOs
{
    public class ResultAboutDto
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AboutId { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
