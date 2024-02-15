using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.WIINGS.Business.Entities
{
    public class GlobalSupplier
    {
        public string BusinessPartyId { get; set; }
        public string BusinessOrgName { get; set; }
        public string ParentBusinessPartyId { get; set; }
        public string ParentBusinessOrgName { get; set; }
        public string GlobalBusinessPartyId { get; set; }
        public string GlobalBusinessOrgName { get; set; }
    }
}
