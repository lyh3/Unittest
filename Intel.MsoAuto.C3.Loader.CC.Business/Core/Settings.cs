using Intel.MsoAuto.C3.Loader.CC.Business.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.CC.Business.Core
{
    public class Settings
    {
        private static IConfigurationBuilder builder;
        private static IConfiguration config;
        private static string integrationDbConnString = String.Empty;
        private static string fsscToolCountConfigurationURL = String.Empty;
        private static string equipmentCeidWseURL = String.Empty;
        private static string gasChemGroupIpnConfigURL = String.Empty;
        private static string scaledScorecardURL = String.Empty;
        private static string scaledSpendingAnalysisExportURL = String.Empty;
        private static string gasChemGroupMorwConfigURL = String.Empty;
        private static string orgMappingConfigURL = String.Empty;
        private static string twAllocatedStartsURL = String.Empty;
        private static string pct10kURL = String.Empty;
        private static string fsscToolCountConfigurationSP = String.Empty;
        private static string equipmentCeidWseSP = String.Empty;
        private static string gasChemGroupIpnConfigSP = String.Empty;
        private static string scaledScorecardSP = String.Empty;
        private static string scaledSpendingAnalysisExportSP = String.Empty;
        private static string gasChemGroupMorwConfigSP = String.Empty;
        private static string orgMappingConfigSP = String.Empty;
        private static string twAllocatedStartsSP = String.Empty;
        private static string pct10kSP = String.Empty;

        static Settings()
        {
            builder = new ConfigurationBuilder()
                   .AddJsonFile("appSettings.json", false, true);
            config = builder.Build();
        }
        public static string IntegrationDbConnString
        {
            get
            {
                if (String.IsNullOrWhiteSpace(integrationDbConnString))
                {
                    integrationDbConnString = config.GetSection("IntegrationDbOptions:ConnectionString").Value;
                }
                return integrationDbConnString;
            }
        }
        public static string Pct10kURL
        {
            get
            {
                if (String.IsNullOrWhiteSpace(pct10kURL))
                {
                    pct10kURL = config.GetSection("URLs:Pct10kURL").Value;
                }
                return pct10kURL;
            }
        }
        public static string Pct10kSP
        {
            get
            {
                if (String.IsNullOrWhiteSpace(pct10kSP))
                {
                    pct10kSP = config.GetSection("IntegrationDbOptions:Pct10kSP").Value;
                }
                return pct10kSP;
            }
        }

        public static string FsscToolCountConfigurationURL
        {
            get
            {
                if (String.IsNullOrWhiteSpace(fsscToolCountConfigurationURL))
                {
                    fsscToolCountConfigurationURL = config.GetSection("URLs:FsscToolCountConfigurationURL").Value;
                }
                return fsscToolCountConfigurationURL;
            }
        }
        public static string FsscToolCountConfigurationSP
        {
            get
            {
                if (String.IsNullOrWhiteSpace(fsscToolCountConfigurationSP))
                {
                    fsscToolCountConfigurationSP = config.GetSection("IntegrationDbOptions:FsscToolCountConfigurationSP").Value;
                }
                return fsscToolCountConfigurationSP;
            }
        }
        public static string EquipmentCeidWseURL
        {
            get
            {
                if (String.IsNullOrWhiteSpace(equipmentCeidWseURL))
                {
                    equipmentCeidWseURL = config.GetSection("URLs:EquipmentCeidWseURL").Value;
                }
                return equipmentCeidWseURL;
            }
        }
        public static string EquipmentCeidWseSP
        {
            get
            {
                if (String.IsNullOrWhiteSpace(equipmentCeidWseSP))
                {
                    equipmentCeidWseSP = config.GetSection("IntegrationDbOptions:EquipmentCeidWseSP").Value;
                }
                return equipmentCeidWseSP;
            }
        }

        public static string GasChemGroupIpnConfigURL
        {
            get
            {
                if (String.IsNullOrWhiteSpace(gasChemGroupIpnConfigURL))
                {
                    gasChemGroupIpnConfigURL = config.GetSection("URLs:GasChemGroupIpnConfigURL").Value;
                }
                return gasChemGroupIpnConfigURL;
            }
        }
        public static string GasChemGroupIpnConfigSP
        {
            get
            {
                if (String.IsNullOrWhiteSpace(gasChemGroupIpnConfigSP))
                {
                    gasChemGroupIpnConfigSP = config.GetSection("IntegrationDbOptions:GasChemGroupIpnConfigSP").Value;
                }
                return gasChemGroupIpnConfigSP;
            }
        }
        public static string ScaledScorecardURL
        {
            get
            {
                if (String.IsNullOrWhiteSpace(scaledScorecardURL))
                {
                    scaledScorecardURL = config.GetSection("URLs:ScaledScorecardURL").Value;
                }
                return scaledScorecardURL;
            }
        }
        public static string ScaledScorecardSP
        {
            get
            {
                if (String.IsNullOrWhiteSpace(scaledScorecardSP))
                {
                    scaledScorecardSP = config.GetSection("IntegrationDbOptions:ScaledScorecardSP").Value;
                }
                return scaledScorecardSP;
            }
        }
        public static string ScaledSpendingAnalysisExportURL
        {
            get
            {
                if (String.IsNullOrWhiteSpace(scaledSpendingAnalysisExportURL))
                {
                    scaledSpendingAnalysisExportURL = config.GetSection("URLs:ScaledSpendingAnalysisExportURL").Value;
                }
                return scaledSpendingAnalysisExportURL;
            }
        }
        public static string ScaledSpendingAnalysisExportSP
        {
            get
            {
                if (String.IsNullOrWhiteSpace(scaledSpendingAnalysisExportSP))
                {
                    scaledSpendingAnalysisExportSP = config.GetSection("IntegrationDbOptions:ScaledSpendingAnalysisExportSP").Value;
                }
                return scaledSpendingAnalysisExportSP;
            }
        }
        public static string GasChemGroupMorwConfigURL
        {
            get
            {
                if (String.IsNullOrWhiteSpace(gasChemGroupMorwConfigURL))
                {
                    gasChemGroupMorwConfigURL = config.GetSection("URLs:GasChemGroupMorwConfigURL").Value;
                }
                return gasChemGroupMorwConfigURL;
            }
        }
        public static string GasChemGroupMorwConfigSP
        {
            get
            {
                if (String.IsNullOrWhiteSpace(gasChemGroupMorwConfigSP))
                {
                    gasChemGroupMorwConfigSP = config.GetSection("IntegrationDbOptions:GasChemGroupMorwConfigSP").Value;
                }
                return gasChemGroupMorwConfigSP;
            }
        }
        public static string OrgMappingConfigURL
        {
            get
            {
                if (String.IsNullOrWhiteSpace(orgMappingConfigURL))
                {
                    orgMappingConfigURL = config.GetSection("URLs:OrgMappingConfigURL").Value;
                }
                return orgMappingConfigURL;
            }
        }
        public static string OrgMappingConfigSP
        {
            get
            {
                if (String.IsNullOrWhiteSpace(orgMappingConfigSP))
                {
                    orgMappingConfigSP = config.GetSection("IntegrationDbOptions:OrgMappingConfigSP").Value;
                }
                return orgMappingConfigSP;
            }
        }
        public static string TWAllocatedStartsURL
        {
            get
            {
                if (String.IsNullOrWhiteSpace(twAllocatedStartsURL))
                {
                    twAllocatedStartsURL = config.GetSection("URLs:TWAllocatedStartsURL").Value;
                }
                return twAllocatedStartsURL;
            }
        }
        public static string TWAllocatedStartsSP
        {
            get
            {
                if (String.IsNullOrWhiteSpace(twAllocatedStartsSP))
                {
                    twAllocatedStartsSP = config.GetSection("IntegrationDbOptions:TWAllocatedStartsSP").Value;
                }
                return twAllocatedStartsSP;
            }
        }
    }
}
