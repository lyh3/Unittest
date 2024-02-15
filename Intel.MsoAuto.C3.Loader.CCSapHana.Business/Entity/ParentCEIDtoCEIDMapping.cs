using System;
using System.Collections.Generic;
using System.Text;

namespace Intel.MsoAuto.C3.Loader.CCSapHana.Business.Entity
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
}
