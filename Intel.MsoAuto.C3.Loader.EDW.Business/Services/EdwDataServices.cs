using Intel.MsoAuto.C3.Loader.EDW.Business.DataContext;

namespace Intel.MsoAuto.C3.Loader.EDW.Business.Services {
    public class EdwDataServices : IEdwDataServices {
        public void SyncEdwData()
        {
            new C3CommonDataContext().SyncEdwData();
        }
    }
}
