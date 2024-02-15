using Intel.MsoAuto.C3.Loader.PITT.Business.Entities.PITT.Bottlenecks;
using Intel.MsoAuto.C3.Loader.PITT.Business.Entities.PITT.Bottlenecks.Interfaces;
using Intel.MsoAuto.C3.Loader.PITT.Business.Entities.PITT.Bottlenecks.YieldAnalysis;
using Intel.MsoAuto.C3.PITT.Business.Models.Bottlenecks.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace Intel.Loader.MsoAuto.C3.Entities.MsoAuto.Business.Models.Bottlenecks.YieldAnalysis
{
    [BsonIgnoreExtraElements]
    public class YieldAnalysisBottleneck : Bottleneck, Intel.MsoAuto.C3.Loader.PITT.Business.Entities.PITT.Bottlenecks.Interfaces.IYieldAnalysisBottleneck
    {
        public YieldAnalysisBottleneck()
        {
            base.type = Intel.MsoAuto.C3.Loader.PITT.Business.Entities.PITT.Bottlenecks.Interfaces.BottleneckTypeEnum.YieldAnalysis;
        }
        public YieldAnalysisDetails details { get; set; } = null!;
    }
}
