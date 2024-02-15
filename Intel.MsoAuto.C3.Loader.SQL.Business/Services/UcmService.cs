using Intel.MsoAuto.C3.Loader.SQL.Business.DataContext;
using Intel.MsoAuto.C3.MailService.Notification.Core;
using Intel.MsoAuto.C3.MailService.Notification;

namespace Intel.MsoAuto.C3.Loader.SQL.Business.Services
{
    public class UcmService : IUcmService
    {
        private UcmDataContext dc;
        public void SyncUcmData()
        {
            try
            {
                dc = new UcmDataContext();
                dc.SyncUcmData();
            }
            catch (Exception ex)
            {
                string? env = Core.Settings.Config.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
                ex.ExceptionEmailNotification($"{GetType().Name}", new Configurations(environment: env, sendEmail: true));
            }
        }
        public void SyncChangeCriticalityData()
        {
            try
            {
                dc = new UcmDataContext();
                dc.SyncChangeCriticalityData();
            }
            catch (Exception ex)
            {
                string? env = Core.Settings.Config.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
                ex.ExceptionEmailNotification("Intel.MsoAuto.C3.Loader.SQL.DailyTask-SyncChangeCriticalityData", new Configurations(environment: env, sendEmail: true));
            }
        }
        public void SyncDetailedChangeReasonData()
        {
            try
            {
                dc = new UcmDataContext();
                dc.SyncDetailedChangeReasonData();
            }
            catch (Exception ex)
            {
                string? env = Core.Settings.Config.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
                ex.ExceptionEmailNotification($"Intel.MsoAuto.C3.Loader.SQL.DailyTask-{GetType().Name}", new Configurations(environment: env, sendEmail: true));
            }
        }
    }

}
