using System;
using System.Collections.Generic;
using System.Text;

namespace Intel.MsoAuto.C3.Loader.EMS.Business.Services
{
    public interface IOrphanPurchaseOrderService
    {
        void SyncOPOData();
        void SyncNeedsData();
    }
}
