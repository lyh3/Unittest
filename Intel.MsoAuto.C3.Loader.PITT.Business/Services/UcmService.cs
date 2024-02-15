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
    public class UcmService
    {
        private readonly UcmDataContext dc;
        private string? _env = null;
        public UcmService(string? env)
        {
            dc = new UcmDataContext();
            _env = env;
        }

        public void StartUcmDailyTask()
        {
            try
            {
                dc.SyncUcmDetailedChangeReasonsData();
                dc.SyncUcmProjectCriticalityData();
                dc.SyncUcmData();
            }
            catch (Exception ex)
            {
                ex.ExceptionEmailNotification(GetType().Name, new Configurations(environment: _env, sendEmail: true));
            }
        }
    }
}
