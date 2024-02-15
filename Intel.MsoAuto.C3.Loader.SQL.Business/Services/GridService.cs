using Intel.MsoAuto.C3.Loader.SQL.Business.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.Loader.SQL.Business.Services
{
    public class GridService : IGridService
    {
        private GridDataContext dc;
        public void SyncGridSupplierContactData()
        {
            try
            {
                dc = new GridDataContext();
                dc.SyncGridSupplierContactData();
            }
            catch (Exception ex) 
            {
                string? env = Core.Settings.Config.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
                ex.ExceptionEmailNotification($"Intel.MsoAuto.C3.Loader.SQL.DailyTask-{GetType().Name}", new Configurations(environment: env, sendEmail: true));
            }
        }
        public void SyncGridSupplierIntelContactData()
        {
            try
            {
                dc = new GridDataContext();
                dc.SyncGridSupplierIntelContactData();
            }
            catch (Exception ex)
            {
                string? env = Core.Settings.Config.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
                ex.ExceptionEmailNotification($"Intel.MsoAuto.C3.Loader.SQL.DailyTask-{GetType().Name}", new Configurations(environment: env, sendEmail: true));
            }
        }
    }
}
