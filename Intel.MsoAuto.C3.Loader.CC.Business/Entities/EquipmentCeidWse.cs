using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Intel.MsoAuto.C3.Loader.CC.Business.Entities
{
    public class EquipmentCeidWse
    {
        public string Facility { get; set; }
        public string Process { get; set; }
        public string WSE { get; set; }
        public string Outs { get; set; }

        [JsonProperty("Work Week")] 
        public string WorkWeek { get; set; }
        [JsonProperty("Tech Node")]
        public string TechNode { get; set; }
       
        [JsonProperty("Functional Area")]
        public string FunctionalArea { get; set; }
        [JsonProperty("Org Area")]
        public string OrgArea { get; set; }
        [JsonProperty("Org Unit")]
        public string OrgUnit { get; set; }
        [JsonProperty("Parent CEID")]
        public string ParentCEID { get; set; }
        [JsonProperty("Equipment CEID")]
        public string EquipmentCEID { get; set; }
        [JsonProperty("Is No Outs")]
        public string IsNoOuts { get; set; }
        [JsonProperty("Source Outs")]
        public string SourceOuts { get; set; }
        [JsonProperty("Source Outs Prime")]
        public string SourceOutsPrime { get; set; }
        [JsonProperty("Outs Prime")]
        public string OutsPrime { get; set; }
        [JsonProperty("WSE Prime")]
        public string WSEPrime { get; set; }
        [JsonProperty("Last Updated Date")]
        public string LastUpdatedDate { get; set; }
    }
    public class EquipmentCeidWseList : List<EquipmentCeidWse>
    {
    }
}
