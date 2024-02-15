using System;
using System.Collections.Generic;
using System.Text;

namespace Intel.MsoAuto.C3.Loader.WSPW.Business.Entities
{
    public class ParentCEIDtoCEIDMapping
    {
        public string TechnologyNodeNm { get; set; }
        public string CapitalEquipmentIdentifier { get; set; }
        public string ParentCapitalEquipmentIdentifier { get; set; }
        public string ActiveInd { get; set; }
        public string AsOfSourceTs { get; set; }
        public string AsOfTargetTs { get; set; }
    }

    public class ParentCEIDtoCEIDMappingList : List<ParentCEIDtoCEIDMapping>
    {
    }
}
