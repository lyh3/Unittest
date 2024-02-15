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
    public class BottleneckService
    {
        private readonly BottleneckDataContext dc;
        private string? _env = null;
        public BottleneckService(string? env)
        {
            dc = new BottleneckDataContext();
            _env = env;
        }

        public void StartSyncYieldAnalysisForecastItemsAsync()
        {
            try
            {
                dc.SyncYieldAnalysisForecastItemsAsync();
            }
            catch (Exception ex)
            {
                ex.ExceptionEmailNotification(GetType().Name, new Configurations(environment: _env, sendEmail: true));
            }
        }
    }
}
       
