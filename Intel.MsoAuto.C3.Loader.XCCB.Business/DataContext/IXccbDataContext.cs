using Intel.MsoAuto.C3.Loader.XCCB.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.XCCB.Business.DataContext
{
    public interface IXccbDataContext
    {
        void SyncXccbDocumentsToMongo();
    }
}
