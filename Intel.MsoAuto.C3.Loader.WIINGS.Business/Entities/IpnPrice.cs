using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.WIINGS.Business.Entities
{

    public class IpnPrice
    {
        public string Ipn { get; set; }
        public string Site { get; set; }
        public string AvgSiteNewBuyPrice { get; set; }
        public string AvgSiteRepairPrice { get; set; }
    }
}
