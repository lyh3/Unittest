using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.CC.Business.Entities
{
    public class Percentage10k
    {
        [JsonProperty("Work Week")]
        public string WorkWeek { get; set; }
        [JsonProperty("Facility")]
        public string Facility { get; set; }
        [JsonProperty("Tech Node")]
        public string TechNode { get; set; }
        [JsonProperty("Org Area")]
        public string OrgArea { get; set; }
        [JsonProperty("Org Unit")]
        public string OrgUnit { get; set; }
        [JsonProperty("Parent CEID")]
        public string ParentCEID { get; set; }
        [JsonProperty("Commodity")]
        public string Commodity { get; set; }
        [JsonProperty("Variable %")]
        public string VariablePercent { get; set; }
        [JsonProperty("Tool Count")]
        public string ToolCount { get; set; }
        [JsonProperty("10K Tool Count")]
        public string TenKToolCount { get; set; }
        [JsonProperty("Scaled WSE")]
        public string ScaledWSE { get; set; }
        [JsonProperty("Percent of 10K")]
        public string Percentof10K { get; set; }
        [JsonProperty("Last Updated Date")]
        public string LastUpdatedDate { get; set; }
    }

    public class Percentage10KList : List<Percentage10k>
    {
    }
}
