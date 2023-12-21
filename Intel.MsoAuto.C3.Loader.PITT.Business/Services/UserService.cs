using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intel.MsoAuto.C3.Loader.PITT.Business.DataContext;
using Intel.MsoAuto.C3.Loader.PITT.Business.Services.interfaces;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Services
{
    public class UserService: IUserService
    {
        private readonly UserDataContext dc;
        public UserService()
        {
            dc = new UserDataContext();
        }

        public void SyncUserDataToMongo()
        {
            dc.SyncUserDataToMongo();
        }
    }
}
