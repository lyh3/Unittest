using Intel.MsoAuto.C3.Loader.CCSapHana.Business.Entity;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.CCSapHana.Business.DataContext
{
    public interface ICopyExactIdentifierDataContext
    {
        bool SyncCopyExactIdentifierData();
        string GetHanaValue(HanaDataReader reader, string colName);
        void UpdateCopyExactIdentifierDataToDb(CopyExactIdentifiers ceidList);

    }
}
