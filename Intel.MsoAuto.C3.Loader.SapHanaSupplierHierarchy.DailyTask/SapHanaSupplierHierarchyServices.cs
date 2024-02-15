using Intel.MsoAuto.C3.Loader.SapHanaSupplierHierarchy.Business.DataContext;

namespace Intel.MsoAuto.C3.Loader.SapHanaSupplierHierarchy.DailyTask {
    internal class SapHanaSupplierHierarchyServices {
        public bool SyncSapHanaSupplierHeirachyData()
        {
            return new SapHanaSupplierHierarchyContext().SyncSapHanaSupplierHierarchyData();
        }
    }
}