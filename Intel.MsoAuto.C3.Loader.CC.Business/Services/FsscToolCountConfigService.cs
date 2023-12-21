using Intel.MsoAuto.C3.Loader.CC.Business.DataContext;
using Intel.MsoAuto.C3.Loader.CC.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.CC.Business.Services
{
    public class FsscToolCountConfigService: IFsscToolCountConfigService
    {
        public void SyncFsscToolCountConfigData()
        {
            FsscToolCountConfigDataContext dc = new FsscToolCountConfigDataContext();
            dc.SyncFsscToolCountConfigData();
        }
    }
}
