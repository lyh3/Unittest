using Intel.MsoAuto.C3.Loader.SQL.Business.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.SQL.Business.Services
{
    public interface IUcmService
    {
        void SyncUcmData();
        void SyncChangeCriticalityData();
        void SyncDetailedChangeReasonData();
    }
}
