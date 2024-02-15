using Intel.MsoAuto.C3.PITT.Business.Models.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Core
{
    public class Constants
    {
        public const string ENVIRONMENT = "environment";
        public const string DEV_CONFIG = "dev";
        public const string INT_CONFIG = "int";
        public const string PROD_CONFIG = "prod"; 
        public const string PITT_SITE_MAPPING = "pittSiteMapping";
        public const string EXTERNAL_PITT_SITE_PROCESS_MAPPING = "externalSiteProcessMapping";
        public const string APPLICABLE_SITES_MAPPING = "applicableSitesMapping";
        public const string PITT_UCM = "ucm";
        public const string PITT_PROJECTS = "projects";
        public const string PITT_DETAILED_CHANGE_REASONS = "detailedChangeReasons";
        public const string PITT_PROJECT_CRITICALITIES = "projectCriticalities";
        public const string PITT_PROCESS = "processes";
        public const string PITT_GLOBAL_SUPPLIERS = "globalSuppliers";
        public const string PITT_STATE_MODEL_ASSOCIATION_PROJECTS = "stateModelAssociationsProjects";
        public const string PITT_USERS = "users";
        public const string PITT_XCCB_DOCS = "xccbDocuments";
        public const string YIELD_ANALYSIS_FORECAST_ITEMS = "yieldAnalysisForecastItems";
        public const string BOTTLENECKS = "bottlenecks";

        public const string WAITING_YA_ID = "64c2ae2aadce775a1a39caef";
        public const string PITT_STATE_FWP_IN_PROCESS = "65453dcc036eb25a5b04382e";

        public const string AR_ASSIGNED_TEMPLATE = "ARAssignedNotificationTemplate";
        public const string AR_ASSIGNED_OVERDUE_TEMPLATE = "ARAssignedOverdueNotificationTemplate";
        public const string BOTTLENECK_TEMPLATE = "BottleneckNotificationTemplate";
        public const string EPROGRESSION_STATUS_TEMPLATE = "ProgressionStatusNotificationTemplate";
    }
}
