using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.CC.Business.Entities
{
    public class ScaledSpendingAnalysisExport
    {
        //"Work Week": "202308",
        //"Facility": "AZFSM",
        //"Tech Node": "14nm",
        //"Org Area": "ASH",
        //"Org Unit": "ASH",
        //"Parent CEID": "BCLcu",
        //"Commodity": "Gas&Chems",
        //"IPN": "Halocarbon-14, CF4",
        //"IPN Description": "Halocarbon-14, CF4",
        //"Scaled Week Consumption $/WSE": "0",
        //"Scaled 4 Week Consumption $/WSE": "1.3475482883087400681044267877",
        //"Scaled 13 Week Consumption $/WSE": "0.9916597913181297877960729066",
        //"Scaled QTD Consumption $/WSE": "0.932392821625643251286502573",
        //"Scaled Week Cost To Intel $/WSE": "0",
        //"Scaled 4 Week Cost To Intel $/WSE": "1.3475482883087400681044267877",
        //"Scaled 13 Week Cost To Intel $/WSE": "0.9916597913181297877960729066",
        //"Scaled QTD Cost To Intel $/WSE": "0.932392821625643251286502573"
    
        [JsonProperty("Work Week")]
        public string WorkWeek { get; set; }
        public string Facility { get; set; }
        [JsonProperty("Tech Node")]
        public string TechNode { get; set; }
        [JsonProperty("Parent CEID")]
        public string ParentCEID { get; set; }
        public string Commodity { get; set; }
        public string IPN { get; set; }
        [JsonProperty("Scaled 13 Week Consumption $/WSE")]
        public string Scaled13WeekConsumption { get; set; }

    }
    public class ScaledSpendingAnalysisExportList : List<ScaledSpendingAnalysisExport>
    {
    }
}
