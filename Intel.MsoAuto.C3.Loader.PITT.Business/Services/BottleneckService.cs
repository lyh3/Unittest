using Intel.MsoAuto.C3.Loader.PITT.Business.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Services
{
    public class BottleneckService
    {
        private readonly BottleneckDataContext dc;
        public BottleneckService()
        {
            dc = new BottleneckDataContext();
        }

        public void StartSyncYieldAnalysisForecastItemsAsync()
        {
            dc.SyncYieldAnalysisForecastItemsAsync();
        }
    }

}
       
