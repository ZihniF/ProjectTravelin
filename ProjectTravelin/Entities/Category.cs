using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectTravelin.Entities
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string IconUrl { get; set; }

        public bool IsStatus { get; set; }
    }
}