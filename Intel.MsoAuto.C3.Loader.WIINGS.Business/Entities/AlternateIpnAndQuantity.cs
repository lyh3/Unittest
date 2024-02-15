using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.WIINGS.Business.Entities
{
    public class AlternateIpnAndQuantity
    {
        public string AltIpn { get; set; }
        public string PrimaryIpn { get; set; }
        public string AltIpnDesc { get; set; }
        public string SiteOfAltIpn { get; set; }
        public string AvlQty { get; set; }
        public string Last90DaySiteIssues { get; set; }
    }
}
