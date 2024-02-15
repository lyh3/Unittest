using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Intel.MsoAuto.C3.Loader.WSPW.Business.Entities
{
    public class WaferStartDemandForecast
    {
        public string AsOfSourceTs { get; set; }
        public string AsOfTargetTs { get; set; }
        public string SecurityLabelNm { get; set; }
        public string ProductRoadmapVersionId { get; set; }
        public string ProductRoadmapVersionNm { get; set; }
        public string ProductRoadmapTypeCd { get; set; }
        public string ProductRoadmapSubTypeCd { get; set; }
        public string HorizonTimePeriodStartTxt { get; set; }
        public string HorizonTimePeriodEndTxt { get; set; }
        public string PlanCycleReleasedTimePeriodTxt { get; set; }
        public string PlanCycleStartDt { get; set; }
        public string ChangeDtm { get; set; }
        public string FabricationProcessTechnologyCd { get; set; }
        public string ManufacturingLocationCd { get; set; }
        public string ItemId { get; set; }
        public string ItemCharacteristicDerivedValueTxt { get; set; }
        public string DotProcessValueTxt { get; set; }
        public string ItemCodeNm { get; set; }
        public string FiscalYearNbr { get; set; }
        public string FiscalQuarterNbr { get; set; }
        public string FiscalMonthNbr { get; set; }
        public string ProductStartQty { get; set; }
        public string DivisionalEngineeringStartUnallocatedQty { get; set; }
        public string FabEngineeringStartUnallocatedQty { get; set; }
        public string TechnologyDevelopmentEngineeringStartUnallocatedQty { get; set; }

        public string IP_RoadmapReleaseYear { get; set; }
                
    }
}
