using Intel.MsoAuto.C3.Loader.PITT.Business.Entities;
using Microsoft.Identity.Client;
using System.Reflection;
using log4net;
using System.Text.Json;
using Intel.MsoAuto.Shared.Extensions;
using Intel.MsoAuto.C3.Loader.PITT.Business.Services;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Core
{
    public class AzureAuthentication
    {

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public async Task<RequestToken?> GetAccessToken(string username, bool forceRefresh = false)
        {
            try
            {
                AzureAuthSettings appRegistrationToken = Settings.AzureAuthSettings;
                string scope = appRegistrationToken.scope;
                string authorityUrl = appRegistrationToken.baseUrl + "oauth2/v2.0/token";
                string clientId = appRegistrationToken.clientId;
                string clientSecret = appRegistrationToken.clientSecret;

                AAMService aamService = new AAMService();
                string? password = aamService.GetPasswordByUsername(username);

                if (password.IsNullOrEmpty())
                {
                    return null;
                }

                using (HttpClient client = new HttpClient())
                {
                    Dictionary<string, string> formBody = new Dictionary<string, string>();
                    formBody.Add("client_id", clientId);
                    formBody.Add("scope", scope);
                    formBody.Add("client_secret", clientSecret);
                    formBody.Add("grant_type", "password");
                    formBody.Add("username", username + "@intel.com");
                    formBody.Add("password", password);
                    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, authorityUrl)
                    {
                        Content = new FormUrlEncodedContent(formBody)
                    })
                    {
                        HttpResponseMessage res = await client.SendAsync(request);
                        string jsonString = await res.Content.ReadAsStringAsync();

                        if (res.IsSuccessStatusCode)
                        {

                            MicrosoftToken? mToken = JsonSerializer.Deserialize<MicrosoftToken>(jsonString);

                            if (mToken.IsNotNull())
                            {
                                DateTime now = DateTime.UtcNow;
                                DateTime expiresOn = now.AddSeconds(mToken.expires_in);
                                return new RequestToken
                                {
                                    accessToken = mToken.access_token,
                                    tokenType = mToken.token_type,
                                    expiresOn = expiresOn,
                                };
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception("Credentials provided are invalid or you do not have access to the services.\n\n" + e.Message);
            }

            return null;
        }



    }
}
