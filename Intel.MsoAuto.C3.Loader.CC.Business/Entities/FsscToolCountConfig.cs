using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.CC.Business.Entities
{
    public class FsscToolCountConfig
    {
        //"Action": "",
        //"Work Week": "202308",
        //"Facility": "AZFSM",
        //"Building": "OC1",
        //"Process": "1270",
        //"Equipment CEID": "AFHye",
        //"Production Tool Count": "3.00000",
        //"Total Tool Count": "3.00000",
        //"Comment": "",
        //"Errors": ""
        public string Action { get; set; }
        [JsonProperty("Work Week")]
        public string WorkWeek { get; set; }
        public string Facility { get; set; }
        public string Building { get; set; }
        public string Process { get; set; }
        [JsonProperty("Equipment CEID")]
        public string EquipmentCEID { get; set; }
        [JsonProperty("Production Tool Count")]
        public string ProductionToolCount { get; set; }
        [JsonProperty("Total Tool Count")]
        public string TotalToolCount { get; set; }
        public string Comment { get; set; }
        public string Errors { get; set; }
    }
    public class FsscToolCountConfigList : List<FsscToolCountConfig>
    {
    }
}
