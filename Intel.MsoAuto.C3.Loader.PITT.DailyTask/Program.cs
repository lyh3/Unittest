using System.Reflection;
using log4net;
using Intel.MsoAuto.C3.Loader.PITT.Business.Services;
using Intel.MsoAuto.C3.Loader.PITT.Business.Core;
using Intel.MsoAuto.C3.Loader.PITT.Business.Entities;
//using Intel.MsoAuto.C3.Loader.CC.Business.Services;
using Intel.MsoAuto.Shared.Extensions;

namespace Intel.MsoAuto.C3.Loader.PITT.DailyTask
{
    public class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static async Task Main(string[] args)
        {
            _log.Info("PITT DailyTask: Main -->");

            try
            {
                IConfigurationBuilder builder = new ConfigurationBuilder()
                                              .SetBasePath(Directory.GetCurrentDirectory())
                                              .AddJsonFile("appsettings.json", optional: false).AddUserSecrets<UserSecrets>();
                IConfiguration config = builder.Build();
                string encryptionKey = config.GetSection("encryptionKey").Value.ToStringSafely();
                if (encryptionKey == null || encryptionKey.Length < 1)
                    throw new ArgumentNullException("Encryption key is not defined");

                Settings settings = new Settings(config, encryptionKey);
                //Daily Tasks
                new SiteService().StartApplicableSitesDailyTask();
                new UcmService().StartUcmDailyTask();
                new SupplierService().StartSuppliersDailyTask();
                new StateModelService().StartStateModelDailyTask();
                new UserService().SyncUserDataToMongo();
                new EmailNotificationService().SendEmailNotification();//Email notification
                new BottleneckService().StartSyncYieldAnalysisForecastItemsAsync();
             }
            catch (Exception e)
            {
                _log.Info("PITT DailyTask: Main: Caught Exception!");
                _log.Info(e);
                throw new Exception(e.Message);
            }
            _log.Info("PITT DailyTask: Main <--");
        }
    }
}
