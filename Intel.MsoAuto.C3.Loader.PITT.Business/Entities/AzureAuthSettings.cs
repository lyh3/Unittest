namespace Intel.MsoAuto.C3.Loader.PITT.Business.Entities
{
    public class AzureAuthSettings
    {
        public string scope = string.Empty;
        public string baseUrl = string.Empty;
        public string clientId = string.Empty;
        public string clientSecret = string.Empty;

        public AzureAuthSettings(string _scope, string _baseUrl, string _clientId, string _clientSecret)
        {
            scope = _scope;
            baseUrl = _baseUrl;
            clientId = _clientId;
            clientSecret = _clientSecret;
        }

    }
}
