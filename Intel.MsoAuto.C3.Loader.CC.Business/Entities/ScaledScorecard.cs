using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.CC.Business.Entities
{
    public class ScaledScorecard
    {
        //"Work Week": "202308",
        //"Facility": "AZFSM",
        //"Tech Node": "14nm",
        //"Org Area": "ASH",
        //"Org Unit": "ASH",
        //"Parent CEID": "BCLcu",
        //"Commodity": "Gas&Chems",
        //"Scaled Week Consumption $/WSE": "0.4503293513124304510047784251",
        //"Week Consumption Delta To FSSC Goal": "-1.4396706486875695489952215749",
        //"Scaled 4 Week Consumption $/WSE": "2.139607900113507377979568672",
        //"4 Week Consumption Delta To FSSC Goal": "0.249607900113507377979568672",
        //"Scaled 13 Week Consumption $/WSE": "1.8348397786974259634293093833",
        //"13 Week Consumption Delta To FSSC Goal": "-0.0551602213025740365706906167",
        //"Scaled QTD Consumption $/WSE": "1.5315385491970983941967883936",
        //"QTD Consumption Delta To FSSC Goal": "-0.3584614508029016058032116064",
        //"Scaled Week Cost To Intel $/WSE": "0.4503293513124304510047784251",
        //"Week Cost To Intel Delta To FSSC Goal": "-1.4396706486875695489952215749",
        //"Scaled 4 Week Cost To Intel $/WSE": "2.139607900113507377979568672",
        //"4 Week Cost To Intel Delta To FSSC Goal": "0.249607900113507377979568672",
        //"Scaled 13 Week Cost To Intel $/WSE": "1.8348397786974259634293093833",
        //"13 Week Cost To Intel Delta To FSSC Goal": "-0.0551602213025740365706906167",
        //"Scaled QTD Cost To Intel $/WSE": "1.5315385491970983941967883936",
        //"QTD Cost To Intel Delta To FSSC Goal": "-0.3584614508029016058032116064",
        //"FSSC": "1.89"

        [JsonProperty("Work Week")] 
        public string WorkWeek { get; set; }
        public string Facility { get; set; }
        [JsonProperty("Tech Node")]
        public string TechNode { get; set; }
        [JsonProperty("Parent CEID")]
        public string ParentCEID { get; set; }
        public string Commodity { get; set; }
        [JsonProperty("Scaled 13 Week Consumption $/WSE")]
        public string Scaled13WeekConsumption { get; set; }

    }
    public class ScaledScorecardList : List<ScaledScorecard>
    {
    }
}
