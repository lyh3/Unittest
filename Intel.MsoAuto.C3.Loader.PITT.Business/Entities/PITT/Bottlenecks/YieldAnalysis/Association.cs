using System.Text.Json.Serialization;
using Intel.MsoAuto.C3.Loader.PITT.Business.Entities.PITT.Bottlenecks.Interfaces;
using Intel.MsoAuto.C3.PITT.Business.Models.Bottlenecks.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities.PITT.Bottlenecks.YieldAnalysis
{
    public class Associations : List<Association> { }

    [BsonIgnoreExtraElements]
    public class Association : Interfaces.IAssociation
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Interfaces.AssociationType type { get; set; }
        public string value { get; set; } = string.Empty;
    }
}
