using Intel.MsoAuto.C3.Loader.CC.Business.DataContext;
using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.Loader.CC.Business.Services
{
    public class FsscToolCountConfigService: IFsscToolCountConfigService
    {
        public void SyncFsscToolCountConfigData()
        {
            try
            {
                FsscToolCountConfigDataContext dc = new FsscToolCountConfigDataContext();
                dc.SyncFsscToolCountConfigData();
            }
            catch (Exception ex)
            {
                string? env = Core.Settings.Config.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
                ex.ExceptionEmailNotification($"Intel.MsoAuto.C3.Loader.CC.DailyTask -{GetType().Name}", new Configurations(environment: env, sendEmail: true));
            }
        }
    }
}
