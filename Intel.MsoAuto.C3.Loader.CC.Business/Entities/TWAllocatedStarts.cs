using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.CC.Business.Entities
{
    public class TWAllocatedStarts
    {
        //"Work Week": "202308",
        //"Shift": "7",
        //"Facility": "AZFSM",
        //"Tech Node": "10nm",
        //"Process": "1274",
        //"Functional Area": "DI",
        //"Org Area": "Diffusion",
        //"Org Unit": "ALD1",
        //"Parent CEID": "SPQdi",
        //"Equipment CEID": "SPQdi",
        //"Lot": "L308T8B0",
        //"Operation": "0202",
        //"Silicon Part Number": "821-3108-38",
        //"Route": "DNTW.114",
        //"Route Description": "SPQdi GROUP 1 MONS",
        //"Quantity": "25",
        //"Lot Type": "SUST",
        //"Consumption": "2101.25",
        //"Silicon Type": "Prime",
        //"Transaction Date": "24-Feb-2023 11:29:06 AM",
        //"Started By Facility": "AZFSM",
        //"Attribute1": "TWVDF-44840",
        //"Equivalent Route": "",
        //"Initial Route": "DNTW.114",
        //"Transaction Type": "Start",
        //"Last Updated Date": "27-Feb-2023 1:41:04 PM"

        [JsonProperty("Work Week")] 
        public string WorkWeek { get; set; }
        [JsonProperty("Tech Node")] 
        public string TechNode { get; set; }
        public string Process { get; set; }
        [JsonProperty("Parent CEID")]
        public string ParentCeid { get; set; }
        [JsonProperty("Equipment CEID")]
        public string EquipCeid { get; set; }
        public string Route { get; set; }
        [JsonProperty("Route Description")]
        public string RouteDesc { get; set; }
    }
    public class TWAllocatedStartsList : List<TWAllocatedStarts>
    {
    }
}
