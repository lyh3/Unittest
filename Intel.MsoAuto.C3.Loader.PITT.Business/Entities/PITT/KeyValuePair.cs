
using MongoDB.Bson.Serialization.Attributes;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities.PITT
{
    [BsonIgnoreExtraElements]
    public class KeyValuePair
    {
        public string? key { get; set; } 
        public string? value { get; set; }

    }

    public class KeyValuePairs : List<KeyValuePair> { }
}
