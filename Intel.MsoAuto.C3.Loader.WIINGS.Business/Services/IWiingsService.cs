using Intel.MsoAuto.C3.Loader.WIINGS.Business.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.WIINGS.Business.Services
{
    public interface IWiingsService
    {
        void SyncIpnPriceData();
        void SyncAltIpnAndQuantityData();
        void SyncGlobalSupplierData();
        void SyncSiteReceiptQuantityData();
        void SyncSiteMaxQuantityData();
    }

}
