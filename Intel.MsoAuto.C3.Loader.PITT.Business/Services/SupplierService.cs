using Intel.MsoAuto.C3.Loader.PITT.Business.DataContext;
using Intel.MsoAuto.C3.MailService.Notification.Entities;
using Intel.MsoAuto.C3.MailService.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Services
{
    public class SupplierService
    {
        private readonly SupplierDataContext dc;
        private string? _env = null;
        public SupplierService(string env) { 
            dc = new SupplierDataContext();
            _env = env;
        }    

        public void StartSuppliersDailyTask()
        {
            try
            {
                dc.SyncGlobalSupplierData();
            }
            catch (Exception ex)
            {
                ex.ExceptionEmailNotification(GetType().Name, new Configurations(environment: _env, sendEmail: true));
             }
        }
    }
}
