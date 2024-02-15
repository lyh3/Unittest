using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.WSPW.Business.Entities
{
    public class WaferStartCapacityPlan
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
        public string FabricationProcessTechnologyCd { get; set; }
        public string ManufacturingLocationCd { get; set; }
        public string SendingManufacturingLocationCd { get; set; }
        public string ReceivingManufacturingLocationCd { get; set; }
        public string SendingFabricationProcessTechnologyCd { get; set; }
        public string ReceivingFabricationProcessTechnologyCd { get; set; }
        public string FiscalYearNbr { get; set; }
        public string FiscalQuarterNbr { get; set; }
        public string FiscalMonthNbr { get; set; }
        public string PlanCycleStartDt { get; set; }
        public string ChangeDtm { get; set; }
        public string CommitTradeCapacityQty { get; set; }
        public string EquippedTradeCapacityQty { get; set; }
        public string EquippedCapacityQty { get; set; }
        public string CommitCapacityQty { get; set; }
        public string WholeCommitCapacityQty { get; set; }
        public string CommitCapacityAdjustmentQty { get; set; }
        public string ProductionTradeRatioQty { get; set; }
        public string FabEngineeringTradeRatioQty { get; set; }
        public string DivisionalEngineeringTradeRatioQty { get; set; }
        public string TechnologyDevelopmentEngineeringTradeRatioQty { get; set; }
        public string FactoryPlannedEventImpactQty { get; set; }
        public string IP_RoadmapReleasedYear { get; set; }
        public string C4EquippedCapacityQty { get; set; }
        public string C4SIBEquippedCapacityQty { get; set; }
        public string C4PSBEquippedCapacityQty { get; set; }

    }
}
