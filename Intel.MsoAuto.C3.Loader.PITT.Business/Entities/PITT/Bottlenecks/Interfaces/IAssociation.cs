namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities.PITT.Bottlenecks.Interfaces
{
    public enum AssociationType
    {
        State
    }
    public interface IAssociation
    {
        AssociationType type { get; set; }
        string value { get; set; }
    }
}
