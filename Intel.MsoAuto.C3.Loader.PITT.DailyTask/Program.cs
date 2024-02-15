using System.Reflection;
using log4net;
using Intel.MsoAuto.C3.Loader.PITT.Business.Entities;
using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.Loader.PITT.DailyTask
{
    public class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static async Task Main(string[] args)
        {
            _log.Info("PITT DailyTask: Main -->");
            string? _env = null;
            try
            {
                IConfigurationBuilder builder = new ConfigurationBuilder()
                                          .SetBasePath(Directory.GetCurrentDirectory())
                                          .AddJsonFile("appsettings.json", optional: false).AddUserSecrets<UserSecrets>();
                IConfiguration config = builder.Build();
                _env = config.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
                string encryptionKey = config.GetSection("encryptionKey").Value;
                if (encryptionKey == null || encryptionKey.Length < 1)
                    throw new ArgumentNullException("Encryption key is not defined");

                Business.Core.Settings settings = new Business.Core.Settings(config, encryptionKey);

                // create hosting object and DI layer
                using IHost host = CreateHostBuilder(args).Build();

                // create a service scope
                using IServiceScope scope = host.Services.CreateScope();

                IHostBuilder CreateHostBuilder(string[] strings)
                {
                    return Host.CreateDefaultBuilder()
                        .ConfigureServices((_, services) =>
                        {
                            // DailyTask stuff
                            services.AddSingleton<Business.Services.StateModelService>();
                        });
                }

                IServiceProvider services = scope.ServiceProvider;

                //Daily Tasks
                new Business.Services.SiteService(_env).StartApplicableSitesDailyTask();
                new Business.Services.UcmService(_env).StartUcmDailyTask();
                new Business.Services.SupplierService(_env).StartSuppliersDailyTask();
                await services.GetRequiredService<Business.Services.StateModelService>().StartStateModelDailyTask();
                new Business.Services.UserService(_env).SyncUserDataToMongo();
                new Business.Services.BottleneckService(_env).StartSyncYieldAnalysisForecastItemsAsync();
                new Business.Services.EmailNotificationService(_log, _env).SendEmailNotification();
            }
            catch (Exception ex)
            {
                ex.ExceptionEmailNotification($"Intel.MsoAuto.C3.Loader.PITT.DailyTask - Main", new Configurations(environment: _env, sendEmail: true));
            }
            _log.Info("PITT DailyTask: Main <--");
        }
    }
}
