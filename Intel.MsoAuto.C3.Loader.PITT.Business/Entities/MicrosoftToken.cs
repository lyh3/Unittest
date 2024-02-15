using Intel.MsoAuto.C3.Loader.PITT.Business.Entities.Interfaces;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities
{
    public class MicrosoftToken : IMicrosoftToken
    {
        public string token_type { get; set; } = string.Empty;
        public string scope { get; set; } = string.Empty;
        public double expires_in { get; set; }
        public double ext_expires_in { get; set; }
        public string access_token { get; set; } = string.Empty;
    }
}
