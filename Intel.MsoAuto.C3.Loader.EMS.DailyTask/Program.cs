using Intel.MsoAuto.C3.Loader.EMS.Business.Entities;
using Intel.MsoAuto.C3.Loader.EMS.Business.Services;
using Intel.MsoAuto.Shared;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using log4net;
using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.Loader.EMS.DailyTask
{
    public class Program
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static void Main(string[] args)
        {
            log.Info("EMS OPO DailyTask: Main -->");

            try
            {
                log.Info("Starting data loader for Orphan PO");
                new OrphanPurchaseOrderService().SyncOPOData();
                new OrphanPurchaseOrderService().SyncNeedsData();
                log.Info("Data loading completed for Orphan PO");                        
            }
            catch (Exception e)
            {
                log.Info("EMS OPO DailyTask: Main: Caught Exception!");
                log.Info(e);
                string? env = Business.Core.Settings.Config.GetRequiredAppSettingsValueValidation(MailService.Notification.Core.Constants.ENVIRONMENT);
                new Exception("Exception caught at [Intel.MsoAuto.C3.Loader.EMS.DailyTask].").ExceptionEmailNotification("EMS.DailyTask", new Configurations(environment: env, sendEmail: true));
            }
            log.Info("EMS OPO DailyTask: Main <--");
        }
    }
}
