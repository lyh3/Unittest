using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities
{
    public class DetailedChangeReason
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public string? name { get; set; }
        public bool? isActive { get; set; }
        public DateTime? updatedOn { get; set; }
        public DateTime? createdOn { get; set; }

    }
}
