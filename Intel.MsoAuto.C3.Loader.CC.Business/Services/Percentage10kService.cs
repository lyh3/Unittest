using Intel.MsoAuto.C3.Loader.CC.Business.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.CC.Business.Services
{
    public class Percentage10kService : IPercentage10kService
    {
        public void SyncPct10kData()
        {
            Percentage10kDataContext dc = new Percentage10kDataContext();
            dc.SyncPct10kData();
        }
    }

}
