using Intel.MsoAuto.C3.Loader.SQL.Business.Entities;
using Intel.MsoAuto.C3.Loader.SQL.Business.Services;
using Intel.MsoAuto.Shared;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using log4net;
using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.Loader.SQL.DailyTask
{
    public class Program
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static void Main(string[] args)
        {
            log.Info("SQL DailyTask: Main -->");

            if (args.Length == 0)
            {
                log.Info("Please enter an argument to start data loading for one of the data loaders.");
            }

            string dataLoaderName = args[0];
            
            try
            {
                switch (dataLoaderName)
                {
                    case "Grid":
                        log.Info("Starting data loader for:" + dataLoaderName);
                        new GridService().SyncGridSupplierContactData();
                        new GridService().SyncGridSupplierIntelContactData();
                        log.Info("Data loading completed for:" + dataLoaderName);
                        break;
                    case "Ucm":
                        log.Info("Starting data loader for:" + dataLoaderName);
                        new UcmService().SyncUcmData();
                        log.Info("Data loading completed for:" + dataLoaderName);
                        break;
                    case "ChangeCriticality":
                        log.Info("Starting data loader for:" + dataLoaderName);
                        new UcmService().SyncChangeCriticalityData();
                        log.Info("Data loading completed for:" + dataLoaderName);
                        break;
                    case "DetailedChangeReason":
                        log.Info("Starting data loader for:" + dataLoaderName);
                        new UcmService().SyncDetailedChangeReasonData();
                        log.Info("Data loading completed for:" + dataLoaderName);
                        break;
                    default:
                        log.Info("No data loader found for this name:" + dataLoaderName);
                        break;
                }
            }
            catch (Exception e)
            {
                log.Info("SQL DailyTask: Main: Caught Exception!");
                log.Info(e);
                string? env =  Business.Core.Settings.Config.GetRequiredAppSettingsValueValidation(MailService.Notification.Core.Constants.ENVIRONMENT);
                e.ExceptionEmailNotification("Intel.MsoAuto.C3.Loader.SQL.DailyTask -Main", new Configurations(environment: env, sendEmail: true));
            }
            log.Info("SQL DailyTask: Main <--");            
        }            
    }
}
