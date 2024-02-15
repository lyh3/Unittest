namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities.PITT.Bottlenecks.Interfaces
{
    public enum BottleneckTypeEnum
    {
        YieldAnalysis,
        POR
    }
    public interface IBottleneck
    {
        string? id { get; set; }
        BottleneckTypeEnum type { get; set; }
        string updatedBy { get; set; }
        DateTime updatedOn { get; set; }
    }
}
