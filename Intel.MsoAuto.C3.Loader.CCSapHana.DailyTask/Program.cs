using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Intel.MsoAuto.C3.Loader.CCSapHana.Business.Entity;
using Intel.MsoAuto.C3.Loader.CCSapHana.Business.Services;
using Intel.MsoAuto.C3.MailService.Notification;
using Intel.MsoAuto.C3.MailService.Notification.Core;
using log4net;

namespace Intel.MsoAuto.C3.Loader.CCSapHana.DailyTask
{
    public class Program
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                log.Info("Please enter an argument to start data loading for one of the data loaders.");
            }

            string dataLoaderName = args[0];

            try
            {
                switch (dataLoaderName)
                {
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
                    case "CopyExactIdentifier":
                        log.Info("Starting data loader for:" + dataLoaderName);
                        new CopyExactIdentifierService().SyncCopyExactIdentifierData();
                        log.Info("Data loading completed for:" + dataLoaderName);
                        break;
                    default:
                        log.Error("No data loader found for this name:" + dataLoaderName);
                        break;
                }
            }
            catch (Exception e)
            {
                string? env = Business.Core.Settings.Config.GetRequiredAppSettingsValueValidation(Constants.ENVIRONMENT);
                e.ExceptionEmailNotification("CCSapHana.DailyTask", new Configurations(environment: env, sendEmail: true));
            }
        }
    }
}
