namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities.Interfaces
{
    public interface IMicrosoftToken
    {
        string access_token { get; set; }
        double expires_in { get; set; }
        double ext_expires_in { get; set; }
        string scope { get; set; }
        string token_type { get; set; }
    }
}