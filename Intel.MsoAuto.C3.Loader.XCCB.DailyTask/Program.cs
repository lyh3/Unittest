using System.Reflection;
using log4net;
using Intel.MsoAuto.C3.Loader.XCCB.Business.Services;
using Intel.MsoAuto.C3.Loader.XCCB.Business.Entities;
using Microsoft.Extensions.Configuration;
using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.Loader.XCCB.DailyTask {
    public class Program
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static void Main(string[] args)
        {
            log.Info("XCCB DailyTask: Main -->");
            IConfiguration? config = null;
            try
            {

                IConfigurationBuilder builder = new ConfigurationBuilder()
                                              .SetBasePath(Directory.GetCurrentDirectory())
                                              .AddJsonFile("appsettings.json", optional: false).AddUserSecrets<UserSecrets>();
                config = builder.Build();
                
                string? encryptionKey = config.GetSection("encryptionKey").Value;
                if (encryptionKey == null || encryptionKey.Length < 1)
                    throw new ArgumentNullException("Encryption key is not defined");

                Business.Core.Settings settings = new Business.Core.Settings(config, encryptionKey);
                new XccbService().SyncXccbDocumentsToMongo();
            }
            catch (Exception e)
            {
                log.Info("XCCB DailyTask: Main: Caught Exception!");
                log.Info(e);
                string? env = config.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
                new Exception("Exception caught at [Intel.MsoAuto.C3.Loader.XCCB.DailyTask].").ExceptionEmailNotification("XCCB.DailyTask", new Configurations(environment: env, sendEmail: true));
            }
            log.Info("XCCB DailyTask: Main <--");
        }
    }
}
