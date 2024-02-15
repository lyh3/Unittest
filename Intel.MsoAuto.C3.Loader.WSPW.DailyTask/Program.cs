using Intel.MsoAuto.C3.Loader.WSPW.Business.Entities;
using Intel.MsoAuto.C3.Loader.WSPW.Business.Services;
using Intel.MsoAuto.Shared;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using log4net;
using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;

namespace Intel.MsoAuto.C3.Loader.WSPW.DailyTask
{
    public class Program
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static void Main(string[] args)
        {
            log.Info("WSPW DailyTask: Main -->");

            if (args.Length == 0)
            {
                log.Info("Please enter an argument to start data loading for one of the data loaders.");
            }

            string dataLoaderName = args[0];

            try
            {
                switch (dataLoaderName)
                {
                    case "WsDemandForecast":
                        log.Info("Starting data loader for:" + dataLoaderName);
                        new WaferStartDemandForecastService().SyncWaferStartDemandForecastData();
                        log.Info("Data loading completed for:" + dataLoaderName);
                        break;
                    case "WsCapacityPlanning":
                        log.Info("Starting data loader for:" + dataLoaderName);
                        new WaferStartCapacityPlanService().SyncWaferStartCapacityPlanData();
                        log.Info("Data loading completed for:" + dataLoaderName);
                        break;
                    case "AllocatedGasChemTransaction":
                        log.Info("Starting data loader for:" + dataLoaderName);
                        new AllocatedGasChemTransactionService().SyncAllocatedGasChemTransactionData();
                        log.Info("Data loading completed for:" + dataLoaderName);
                        break;
                    case "AllocatedSpareTransaction":
                        log.Info("Starting data loader for:" + dataLoaderName);
                        new AllocatedSpareTransactionService().SyncAllocatedSpareTransactionData();
                        log.Info("Data loading completed for:" + dataLoaderName);
                        break;
                    case "ParentCEIDtoCEIDMapping":
                        log.Info("Starting data loader for:" + dataLoaderName);
                        new ParentCEIDtoCEIDMappingService().SyncParentCEIDtoCEIDMappingData();
                        log.Info("Data loading completed for:" + dataLoaderName);
                        break;
                    case "ParentCeidPercentWspw":
                        log.Info("Starting data loader for:" + dataLoaderName);
                        new ParentCeidPercentWspwService().SyncParentCeidPercentWspwData();
                        log.Info("Data loading completed for:" + dataLoaderName);
                        break;
                    default:
                        log.Info("No data loader found for this name:" + dataLoaderName);
                        break;
                }
            }
            catch (Exception e)
            {
                log.Info("WSPW DailyTask: Main: Caught Exception!");
                log.Info(e);
                string? env = Business.Core.Settings.Config?.GetRequiredAppSettingsValueValidation(MailService.Notification.Core.Constants.ENVIRONMENT);
                e.ExceptionEmailNotification("EMS.DailyTask -Main", new Configurations(environment: env, sendEmail: true));
            }
            log.Info("WSPW DailyTask: Main <--");            
        }            
    }
}
