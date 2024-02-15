namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities
{
    public class AAMSettings
    {
        public string appID { get; set; } = string.Empty;
        public string safeName { get; set; } = string.Empty;
        public string certificateThumbprint { get; set; } = string.Empty;
        public string genericAccount { get; set; } = string.Empty;

        public AAMSettings(string _appId, string _safeName, string _certThumbprint, string _genericAccount)
        {
            appID = _appId;
            safeName = _safeName;
            certificateThumbprint = _certThumbprint;
            genericAccount = _genericAccount;
        }
    }
}
