using Intel.MsoAuto.C3.Loader.PITT.Business.DataContext;
using Intel.MsoAuto.C3.Loader.PITT.Business.Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Services
{
    public class SiteService : ISiteService
    {
        private readonly SiteDataContext dc;
        public SiteService() {
            dc = new SiteDataContext();
        }
      
        public void StartApplicableSitesDailyTask()
        {
            dc.SyncApplicableSiteMappingsData();
        }
    }
}
