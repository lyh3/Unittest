using Intel.MsoAuto.C3.Loader.CCSapHana.Business.DataContext;
using System;
using System.Collections.Generic;
using System.Text;
using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.Loader.CCSapHana.Business.Services
{
    public class AllocatedGasChemTransactionService : IAllocatedGasChemTransactionService
    {
        public bool SyncAllocatedGasChemTransactionData()
        {
            try
            {
                AllocatedGasChemTransactionDataContext dc = new AllocatedGasChemTransactionDataContext();
                return dc.SyncAllocatedGasChemTransactionData();
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
