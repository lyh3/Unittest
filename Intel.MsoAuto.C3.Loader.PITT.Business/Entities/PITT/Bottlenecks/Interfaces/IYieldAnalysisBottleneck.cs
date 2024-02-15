using Intel.MsoAuto.C3.Loader.PITT.Business.Entities.PITT.Bottlenecks.YieldAnalysis;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities.PITT.Bottlenecks.Interfaces
{
    public interface IYieldAnalysisBottleneck
    {
        YieldAnalysisDetails details { get; set; }
        string? id { get; set; }
        BottleneckTypeEnum type { get; set; }
    }
}
