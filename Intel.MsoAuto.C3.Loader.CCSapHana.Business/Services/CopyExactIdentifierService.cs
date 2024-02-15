using Intel.MsoAuto.C3.Loader.CCSapHana.Business.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.Loader.CCSapHana.Business.Services
{
    public class CopyExactIdentifierService : ICopyExactIdentifierService
    {
        public bool SyncCopyExactIdentifierData()
        {
            try
            {
                CopyExactIdentifierDataContext dc = new CopyExactIdentifierDataContext();
                return dc.SyncCopyExactIdentifierData();
            }
            catch (Exception ex)
            {
                string? env = Core.Settings.Config.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
                ex.ExceptionEmailNotification($"Intel.MsoAuto.C3.Loader.CCSapHana.Business -{GetType().Name}", new Configurations(environment: env, sendEmail: true));
                return false;
            }
        }
    }
}
