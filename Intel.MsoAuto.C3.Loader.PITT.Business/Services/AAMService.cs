using AAM_Dotnet;
using Intel.MsoAuto.C3.Loader.PITT.Business.Core;
using log4net;
using System.Reflection;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Services
{
    public class AAMService
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string? GetPasswordByUsername(string username)
        {
            try
            {
                AAMConfiguration config = new AAMConfiguration()
                {
                    appID = Settings.AAMSettings.appID,
                    safeName = Settings.AAMSettings.safeName,
                    certificateThumbprint = Settings.AAMSettings.certificateThumbprint
                };

                AAMConsumer.ConfigureAAMConsumer(config);
                string pwd = AAMConsumer.GetCredentials(username);

                return pwd;
            }
            catch (Exception e)
            {
                log.Error(e);
                throw new Exception("Could not get the credentials from AAM.\n\n" + e.Message);
            }

        }

    }
}
