using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.WIINGS.Business.Entities
{
    public class SiteMaxQuantity
    {
        public string Ipn { get; set; }
        public string Site { get; set; }
        public string QuantityAvailable { get; set; }
        public string SiteMaxQty { get; set; }
        public string OnHandQuantity { get; set; }
        public string SiteDoi { get; set; }
        public string CountOfStockrooms { get; set; }
    }
    public class SiteMaxQuantities : List<SiteMaxQuantity>
    {
    }
}
