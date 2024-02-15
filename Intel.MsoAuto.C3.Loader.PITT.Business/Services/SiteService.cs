using Intel.MsoAuto.C3.Loader.PITT.Business.DataContext;
using Intel.MsoAuto.C3.Loader.PITT.Business.Services.interfaces;
using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Services
{
    public class SiteService : ISiteService
    {
        private readonly SiteDataContext dc;
        private readonly string? _env = null;
        public SiteService(string env) {
            dc = new SiteDataContext();
            _env = env;
        }
      
        public void StartApplicableSitesDailyTask()
        {
            try
            {
                dc.SyncApplicableSiteMappingsData();
            }
            catch(Exception ex) {
                ex.ExceptionEmailNotification(GetType().Name, new Configurations(environment: _env, sendEmail: true));
            }
        }
    }
}
