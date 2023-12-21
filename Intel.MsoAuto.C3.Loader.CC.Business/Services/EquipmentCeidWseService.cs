using Intel.MsoAuto.C3.Loader.CC.Business.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.CC.Business.Services
{
    public class EquipmentCeidWseService : IEquipmentCeidWseService
    {
        public void SyncEquipmentCeidWseData()
        {
            EquipmentCeidWseDataContext dc = new EquipmentCeidWseDataContext();
            dc.SyncEquipmentCeidWseData();
        }
    }

}
