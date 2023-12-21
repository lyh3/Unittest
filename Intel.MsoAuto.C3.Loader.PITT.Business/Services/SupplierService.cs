using Intel.MsoAuto.C3.Loader.PITT.Business.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Services
{
    public class SupplierService
    {
        private readonly SupplierDataContext dc;
        public SupplierService() { 
            dc = new SupplierDataContext();
        }    

        public void StartSuppliersDailyTask()
        {
            dc.SyncGlobalSupplierData();
        }
    }
}
