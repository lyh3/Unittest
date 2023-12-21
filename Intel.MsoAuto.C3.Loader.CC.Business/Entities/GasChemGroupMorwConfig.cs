using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.CC.Business.Entities
{
    public class GasChemGroupMorwConfig
    {
        //"Action": "",
        //"Process": "1001",
        //"Equipment CEID": "BulkGas-C4_CC",
        //"Gas Chem Group": "Argon, Ar",
        //"MORw Cons": "1.84",
        //"UOM": "$/wse",
        //"Comment": "3/22/22 Rachel copy 1002",
        //"Errors": ""
        public string Action { get; set; }
        public string Process { get; set; }
        [JsonProperty("Equipment CEID")]
        public string EquipmentCeid { get; set; }
        [JsonProperty("Gas Chem Group")]
        public string GasChemGroup { get; set; }
        [JsonProperty("MORw Cons")]
        public string MorwCons { get; set; }
        public string Errors { get; set; }
    }

    public class GasChemGroupMorwConfigList : List<GasChemGroupMorwConfig>
    {

    }
}
