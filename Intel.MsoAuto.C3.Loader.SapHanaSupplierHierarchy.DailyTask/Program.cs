using Intel.MsoAuto.Shared;
using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.Loader.SapHanaSupplierHierarchy.DailyTask {
    public class Program {
        private static void Main(string[] args)
        {
            Functions.LogInfo("--> Starting data loader Supplier Hierachy.");
            try
            {
                new SapHanaSupplierHierarchyServices().SyncSapHanaSupplierHeirachyData();
            }
            catch (Exception ex)
            {
                string? env = Business.Core.Settings.Configuration.GetRequiredAppSettingsValueValidation(MailService.Notification.Core.Constants.ENVIRONMENT);
                ex.ExceptionEmailNotification($"Intel.MsoAuto.C3.Loader.PITT.DailyTask - Main", new Configurations(environment: env, sendEmail: true));
            }
            Functions.LogInfo("<-- data loader Supplier Hierachy completed.");
        }
    }
}
