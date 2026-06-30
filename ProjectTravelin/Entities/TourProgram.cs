using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectTravelin.Entities
{
    public class TourProgram
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TourProgramId { get; set; }

        public string TourId { get; set; }

        public int DayNumber { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }
    }
}
