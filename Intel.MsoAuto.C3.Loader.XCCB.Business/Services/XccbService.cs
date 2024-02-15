using Intel.MsoAuto.C3.Loader.XCCB.Business.DataContext;
using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.Loader.XCCB.Business.Services
{
    public class XccbService: IXccbService
    {
        private XccbDataContext dc;
        public void SyncXccbDocumentsToMongo()
        {
            try
            {
                dc = new XccbDataContext();
                dc.SyncXccbDocumentsToMongo();
            }
            catch (Exception ex)
            {
                string? env = Core.Settings.Config.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
                ex.ExceptionEmailNotification($"Intel.MsoAuto.C3.Loader.XCCB.DailyTask -{GetType().Name}", new Configurations(environment: env, sendEmail: true));
            }
        }
    }
}
