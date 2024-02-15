using Intel.MsoAuto.C3.Loader.CC.Business.Entities;
using Intel.MsoAuto.C3.Loader.CC.Business.Services;
using Intel.MsoAuto.Shared;
using System;
using System.Configuration;
using System.Linq;
using log4net;
using log4net.Config;
using System.Reflection;
using Intel.MsoAuto.C3.MailService.Notification.Core;
using Intel.MsoAuto.C3.MailService.Notification;

namespace Intel.MsoAuto.C3.Loader.CC.DailyTask
{
    public class Program
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main(string[] args)
        {
            log.Info("--> Main");

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("App.config"));

            if (args.Length == 0)
            {
                log.Info("Please enter an argument to start data loading for one of the data loaders.");
            }

            string dataLoaderName = args[0];
            
            try
            {
                switch (dataLoaderName)
                {
                    case "EquipmentCeidWse":
                        log.Info("Starting data loader for:" + dataLoaderName);
                        new EquipmentCeidWseService().SyncEquipmentCeidWseData();
                        log.Info("Data loading completed for:" + dataLoaderName);
                        break;
                    case "FsscToolCountConfig":
                        log.Info("Starting data loader for:" + dataLoaderName);
                        new FsscToolCountConfigService().SyncFsscToolCountConfigData();
                        log.Info("Data loading completed for:" + dataLoaderName);
                        break;
                    case "GasChemGroupIpnConfig":
                        log.Info("Starting data loader for:" + dataLoaderName);
                        new GasChemGroupIpnConfigService().SyncGasChemGroupIpnConfigData();
                        log.Info("Data loading completed for:" + dataLoaderName);
                        break;
                    case "GasChemGroupMorwConfig":
                        log.Info("Starting data loader for:" + dataLoaderName);
                        new GasChemGroupMorwConfigService().SyncGasChemGroupMorwConfigData();
                        log.Info("Data loading completed for:" + dataLoaderName);
                        break;
                    case "OrgMappingConfig":
                        log.Info("Starting data loader for:" + dataLoaderName);
                        new OrgMappingConfigService().SyncOrgMappingConfigData();
                        log.Info("Data loading completed for:" + dataLoaderName);
                        break;
                    case "ScaledScorecard":
                        log.Info("Starting data loader for:" + dataLoaderName);
                        new ScaledScorecardService().SyncScaledScorecardData();
                        log.Info("Data loading completed for:" + dataLoaderName);
                        break;
                    case "ScaledSpendingAnalysisExport":
                        log.Info("Starting data loader for:" + dataLoaderName);
                        new ScaledSpendingAnalysisExportService().SyncScaledSpendingAnalysisExportData();
                        log.Info("Data loading completed for:" + dataLoaderName);
                        break;
                    case "TWAllocatedStarts":
                        log.Info("Starting data loader for:" + dataLoaderName);
                        new TWAllocatedStartsService().SyncTWAllocatedStartsData();
                        log.Info("Data loading completed for:" + dataLoaderName);
                        break;
                    case "Percentage10k":
                        log.Info("Starting data loader for:" + dataLoaderName);
                        new Percentage10kService().SyncPct10kData();
                        log.Info("Data loading completed for:" + dataLoaderName);
                        break;
                    default:
                        throw new Exception("No data loader found for this name:" + dataLoaderName);
                }
            }
            catch (Exception e)
            {
                string? env = Business.Core.Settings.Config.GetRequiredAppSettingsValueValidation(MailService.Notification.Core.Constants.ENVIRONMENT);
                e.ExceptionEmailNotification($"Intel.MsoAuto.C3.Loader.CC.DailyTask - Main", new Configurations(environment:env, sendEmail:true));
            }
            log.Info("<-- Main");
           

        }            
    }
}
