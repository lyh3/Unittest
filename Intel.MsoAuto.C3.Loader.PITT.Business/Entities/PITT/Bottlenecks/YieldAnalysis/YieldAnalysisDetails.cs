
using MongoDB.Bson.Serialization.Attributes;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities.PITT.Bottlenecks.YieldAnalysis
{
    [BsonIgnoreExtraElements]
    public class YieldAnalysisDetails
    {
        public Thresholds thresholds { get; set; } = new Thresholds();
        public Associations associations { get; set; } = new Associations();
    }
}
