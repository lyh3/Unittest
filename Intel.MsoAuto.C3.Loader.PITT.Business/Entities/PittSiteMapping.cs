using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities
{
    public class PittSiteMapping
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public string? siteName { get; set; }
        public List<PittSiteMappingItem>? siteMappings { get; set; }

    }
}
