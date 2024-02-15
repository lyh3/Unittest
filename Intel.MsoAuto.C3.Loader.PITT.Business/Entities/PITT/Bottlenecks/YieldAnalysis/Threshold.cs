using Intel.MsoAuto.C3.Loader.PITT.Business.Entities.PITT.Bottlenecks.Interfaces;
using Intel.MsoAuto.C3.PITT.Business.Models.Bottlenecks.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities.PITT.Bottlenecks.YieldAnalysis
{
    public class Thresholds : List<Threshold> { }

    [BsonIgnoreExtraElements]
    public class Threshold : Interfaces.IThreshold
    {
        public string siteName { get; set; } = String.Empty;
        public int threshold { get; set; } = 0;
    }
}
