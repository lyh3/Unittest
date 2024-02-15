using Intel.MsoAuto.C3.Loader.CCSapHana.Business.DataContext;
using Intel.MsoAuto.C3.MailService.Notification.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.Loader.CCSapHana.Business.Services
{
    public class AllocatedSpareTransactionService : IAllocatedSpareTransactionService
    {
        public bool SyncAllocatedSpareTransactionData()
        {
            try
            {
                AllocatedSpareTransactionDataContext dc = new AllocatedSpareTransactionDataContext();
                return dc.SyncAllocatedSpareTransactionData();
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
