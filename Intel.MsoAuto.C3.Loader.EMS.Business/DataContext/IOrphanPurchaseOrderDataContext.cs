using System;
using System.Collections.Generic;
using System.Text;

namespace Intel.MSOAuto.C3.Loader.EMS.Business.DataContext
{
    public interface IOrphanPurchaseOrderDataContext
    {
        void SyncOPOData();
        void SyncNeedsData();
    }
}
