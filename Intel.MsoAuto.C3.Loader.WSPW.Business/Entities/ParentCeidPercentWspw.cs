using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.WSPW.Business.Entities
{
    public class ParentCeidPercentWspw
    {
        public string AsOfSourceDtm { get; set; }
        public string AsOfTargetDtm { get; set; }
        public string CommodityNm { get; set; }
        public string FacilityNm { get; set; }
        public string FiveKToolCnt { get; set; }
        public string FiveKWaferStartPerWeekPct { get; set; }
        public string LastUpdatedDtm { get; set; }
        public string OrganizationAreaNm { get; set; }
        public string OrganizationUnitNm { get; set; }
        public string ParentCapitalEquipmentId { get; set; }
        public string ScalingWaferStartEquivalentRte { get; set; }
        public string TechnologyNodeNm { get; set; }
        public string ToolCnt { get; set; }
        public string VariablePct { get; set; }
        public string WorkWeekNbr { get; set; }
        
    }

    public class ParentCeidPercentWspwList : List<ParentCeidPercentWspw>
    {
    }
}
