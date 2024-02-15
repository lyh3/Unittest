using System.Text.Json.Serialization;
using Intel.MsoAuto.C3.Loader.PITT.Business.Entities.PITT.Bottlenecks.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities.PITT.Bottlenecks
{
    [BsonIgnoreExtraElements]
    public class Bottleneck : IBottleneck
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonId]
        public string? id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BottleneckTypeEnum type { get; set; }
        public string updatedBy { get; set; } = string.Empty;
        public DateTime updatedOn { get; set; } = DateTime.UtcNow;
    }
}
