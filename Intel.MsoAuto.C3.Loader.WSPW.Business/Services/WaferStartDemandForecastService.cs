using Intel.MsoAuto.C3.Loader.WSPW.Business.DataContext;
using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.Loader.WSPW.Business.Services {
    public class WaferStartDemandForecastService : IWaferStartDemandForecastService {
        private WaferStartDemandForecastDataContext dc;
        public void SyncWaferStartDemandForecastData()
        {
            try
            {
                dc = new WaferStartDemandForecastDataContext();
                dc.SyncWaferStartDemandForecastData();
            }
            catch (Exception ex)
            {
                string? env = Core.Settings.Config?.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
                ex.ExceptionEmailNotification($"Intel.MsoAuto.C3.Loader.WSPW.DailyTask -{GetType().Name}", new Configurations(environment: env, sendEmail: true));
            }

        }
    }
}
