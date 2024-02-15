//using Intel.MsoAuto.C3.Loader.WIINGS.Business.Entities;
using Intel.MsoAuto.C3.Loader.WIINGS.Business.Services;
//using Intel.MsoAuto.Shared;
using System;
using System.Configuration;
using System.Linq;
using log4net;
using System.Reflection;
using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.Loader.WSPW.DailyTask
{
    public class Program
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main()
        {
            log.Info("WiingsDailyTask: Main -->");

            try
            {
                log.Info("Starting data loader for Wiings");
                new WiingsService().SyncIpnPriceData();
                new WiingsService().SyncAltIpnAndQuantityData();
                new WiingsService().SyncGlobalSupplierData();
                log.Info("Data loading completed for Wiings");
            }
            catch (Exception e)
            {
                log.Error("WiingsDailyTask: Main: Caught Exception!");
                log.Error(e);
                string? env = WIINGS.Business.Core.Settings.Config.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
                new Exception("Exception caught at [Intel.MsoAuto.C3.Loader.WSPW.DailyTask].").ExceptionEmailNotification("WSPW.DailyTask", new Configurations(environment:env, sendEmail:true));
            }

            new WiingsService().SyncSiteReceiptQuantityData();
            new WiingsService().SyncSiteMaxQuantityData();

            log.Info("WiingsDailyTask: Main <--");          
        }        
    }
}
