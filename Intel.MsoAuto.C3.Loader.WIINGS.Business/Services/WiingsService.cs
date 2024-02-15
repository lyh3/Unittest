using Intel.MsoAuto.C3.Loader.WIINGS.Business.DataContext;
using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.Loader.WIINGS.Business.Services
{
    public class WiingsService : IWiingsService
    {
        public void SyncIpnPriceData()
        {
            try
            {
                WiingsDataContext dc = new WiingsDataContext();
                dc.SyncIpnPriceData();
            }
            catch (Exception ex)
            {
                string? env = Core.Settings.Config.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
                ex.ExceptionEmailNotification($"Intel.MsoAuto.C3.Loader.WIINGS.Business -{GetType().Name}", new Configurations(environment: env, sendEmail: true));
            }
        }
        public void SyncAltIpnAndQuantityData()
        {
            try
            {
                WiingsDataContext dc = new WiingsDataContext();
                dc.SyncAltIpnAndQuantityData();
            }
            catch (Exception ex)
            {
                string? env = Core.Settings.Config.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
                ex.ExceptionEmailNotification($"Intel.MsoAuto.C3.Loader.WIINGS.Business -{GetType().Name}", new Configurations(environment: env, sendEmail: true));
            }
        }

        public void SyncGlobalSupplierData()
        {
            try
            {
                WiingsDataContext dc = new WiingsDataContext();
                dc.SyncGlobalSupplierData();
            }
            catch (Exception ex)
            {
                string? env = Core.Settings.Config.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
                ex.ExceptionEmailNotification($"Intel.MsoAuto.C3.Loader.WIINGS.Business -{GetType().Name}", new Configurations(environment: env, sendEmail: true));
            }
        }
        public void SyncSiteReceiptQuantityData()
        {
            try
            {
                 WiingsDataContext dc = new WiingsDataContext();
                dc.SyncSiteReceiptQuantityData();           }
            catch (Exception ex)
            {
                string? env = Core.Settings.Config.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
                ex.ExceptionEmailNotification($"Intel.MsoAuto.C3.Loader.WIINGS.Business -{GetType().Name}", new Configurations(environment: env, sendEmail: true));
            }
        }
        public void SyncSiteMaxQuantityData()
        {
            try
            {
                 WiingsDataContext dc = new WiingsDataContext();
                dc.SyncSiteMaxQuantityData();
            }
            catch (Exception ex)
            {
                string? env = Core.Settings.Config.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
                ex.ExceptionEmailNotification($"Intel.MsoAuto.C3.Loader.WIINGS.Business -{GetType().Name}", new Configurations(environment: env, sendEmail: true));
            }
        }
    }
}
