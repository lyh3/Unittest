using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.CC.Business.Entities
{
    public class OrgMappingConfig
    {
        //"Action": "",
        //"Facility": "AZFSM",
        //"Tech Node": "10nm",
        //"Org Area": "Bulk Gas",
        //"Org Unit": "Bulk Gas",
        //"Parent CEID": "BulkGas_CC",
        //"Errors": ""
        public string Action { get; set; }
        public string Facility { get; set; }
        [JsonProperty("Tech Node")]
        public string TechNode { get; set; }
        [JsonProperty("Org Area")]
        public string OrgArea { get; set; }
        [JsonProperty("Org Unit")]
        public string OrgUnit { get; set; }
        [JsonProperty("Parent CEID")]
        public string ParentCeid { get; set; }
        public string Errors { get; set;}

    }
    public class OrgMappingConfigList : List<OrgMappingConfig>
    {
    }
}
