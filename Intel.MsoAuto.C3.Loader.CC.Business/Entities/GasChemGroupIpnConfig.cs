using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.CC.Business.Entities
{
    public class GasChemGroupIpnConfig
    {
        //"Action": "",
        //"IPN": "033015005",
        //"IPN Description": "MICROSCOPE HALOGEN BULB,12V/100W",
        //"Gas Chem Group": "NOT CHEMICAL OR GAS",
        //"Errors": ""
        public string Action { get; set; }
        public string IPN { get; set; }
        [JsonProperty("IPN Description")]
        public string IPNDesc { get; set; }
        [JsonProperty("Gas Chem Group")]
        public string GasChemGrp { get; set; }
        public string Errors { get; set; }
    }
    public class GasChemGroupIpnConfigList : List<GasChemGroupIpnConfig>
    {
    }
}
