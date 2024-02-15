using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.EMS.Business.Entities
{
    public class NeedsTableType
    {
        //public string NeedId { get; set; }
        public string PoNumber { get; set; }

        public NeedsInfo NeedsInfo { get; set; }
    }

    public class NeedsInfo
    {
        [JsonProperty("needid")]
        public string NeedId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }

        [JsonProperty("pr")]
        public Pr Pr { get; set; }

        [JsonProperty("poLines")]
        public PoLine[] PoLines { get; set; }
    }

    public class Pr
    {
        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("due")]
        public string Due { get; set; }
    }
    
}
