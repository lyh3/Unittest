using Intel.MsoAuto.C3.Loader.EDW.Business.Services;
using Intel.MsoAuto.Shared;
using Intel.MsoAuto.C3.MailService.Notification.Core;
using Microsoft.Extensions.Configuration;
using Intel.MsoAuto.C3.MailService.Notification;

namespace Intel.MsoAuto.C3.Loader.EDW.DailyTask {
    public class Program {
        private static void Main(string[] args)
        {
            Functions.LogInfo("--> Intel.MsoAuto.C3.Loader.EDW.DailyTask.");
            IConfiguration _configuration = new ConfigurationBuilder()
                                            .SetBasePath(AppContext.BaseDirectory)
                                            .AddJsonFile(MailService.Notification.Core.Constants.APP_SETTINGS_JSON,
                                            optional: false,
                                            reloadOnChange: true).Build();
            try
            {
                new EdwDataServices().SyncEdwData();
                Functions.LogInfo("<-- Success EDW daily task.");
            }
            catch (Exception ex)
            {
                string? env = _configuration.GetRequiredAppSettingsValueValidation(MailService.Notification.Core.Constants.ENVIRONMENT);
                ex.ExceptionEmailNotification("Intel.MsoAuto.C3.Loader.EDW.DailyTask -Main", new Configurations(environment: env, sendEmail: true));
            }
        }
    }
}
