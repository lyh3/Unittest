namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities.Interfaces
{
    public interface IRequestToken
    {
        string accessToken { get; set; }
        DateTimeOffset expiresOn { get; set; }
        string tokenType { get; set; }
    }
}