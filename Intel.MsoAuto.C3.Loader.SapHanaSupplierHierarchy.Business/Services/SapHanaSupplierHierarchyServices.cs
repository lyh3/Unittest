using Intel.MsoAuto.C3.Loader.SapHanaSupplierHierarchy.Business.DataContext;

namespace Intel.MsoAuto.C3.Loader.SapHanaSupplierHierarchy.Business.Services {
    public class SapHanaSupplierHierarchyServices : ISapHanaSupplierHierarchyServices {
        public bool SyncSapHanaSupplierHeirarchyData()
        {
            return new SapHanaSupplierHierarchyContext().SyncSapHanaSupplierHierarchyData();
        }
    }
}
