using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.SQL.Business.DataContext
{
    public interface IGridDataContext
    {
        void SyncGridSupplierContactData();
        void SyncGridSupplierIntelContactData();
    }
}
