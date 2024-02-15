using Intel.MSOAuto.C3.Loader.EMS.Business.DataContext;
using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.Loader.EMS.Business.Services
{
    public class OrphanPurchaseOrderService : IOrphanPurchaseOrderService
    {
        public void SyncOPOData()
        {
            try
            {
                OrphanPurchaseOrderDataContext dc = new OrphanPurchaseOrderDataContext();
                dc.SyncOPOData();
            }
            catch (Exception ex)
            {
                string? env = Core.Settings.Config.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
                ex.ExceptionEmailNotification($"Intel.MsoAuto.C3.Loader.EMS.DailyTask -{GetType().Name}", new Configurations(environment: env, sendEmail: true));
            }
        }
        public void SyncNeedsData()
        {
            try
            {
                OrphanPurchaseOrderDataContext dc = new OrphanPurchaseOrderDataContext();
                dc.SyncNeedsData();
            }
            catch (Exception ex)
            {
                string? env = Core.Settings.Config.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
                ex.ExceptionEmailNotification($"Intel.MsoAuto.C3.Loader.EMS.DailyTask -{GetType().Name}", new Configurations(environment: env, sendEmail: true));
            }
        }
    }
}
