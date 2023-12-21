using Intel.MsoAuto.C3.Loader.PITT.Business.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Services
{
    public class UcmService
    {
        private readonly UcmDataContext dc;
        public UcmService() {
            dc = new UcmDataContext();
        }

        public void StartUcmDailyTask()
        {
            dc.SyncUcmDetailedChangeReasonsData();
            dc.SyncUcmProjectCriticalityData();
            dc.SyncUcmData();
        }
    }
}
