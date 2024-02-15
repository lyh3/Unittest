using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Intel.MsoAuto.C3.PITT.Business.Models.Bottlenecks.Interfaces;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities.PITT.Bottlenecks.YieldAnalysis
{
    [BsonIgnoreExtraElements]
    public class YieldAnalysisForecastItem : ForecastItem, C3.Loader.PITT.Business.Entities.PITT.Bottlenecks.Interfaces.IYieldAnalysisForecastItem
    {

    }

    public class YieldAnalysisForecastItems : List<YieldAnalysisForecastItem> { }
}
