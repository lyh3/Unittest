using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.WIINGS.Business.Entities
{
    public class SiteReceiptQuantity
    {
        public string Ipn { get; set; }
        public string Site { get; set; }
        public string ReceiptQuantity { get; set; }
        public string AsOfSourceTimeStamp { get; set; }
    }
    public class SiteReceiptQuantities : List<SiteReceiptQuantity>
    {
    }
}
