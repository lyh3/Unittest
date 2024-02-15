using Intel.MsoAuto.C3.PITT.Business.Models;
using Intel.MsoAuto.C3.PITT.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.MsoAuto.C3.Loader.PITT.Business.Services
{
    public class MockUserService : IUserService
    {
        public Task<User?> GetCurrentUser(string? email = null, bool forceInitialization = false)
        {
            User mockUser = new User()
            {
                email = email
            };

            return new Task<User?>(() => mockUser);
        }

        public Task<User?> GetUserByEmail(string email)
        {
            User mockUser = new User()
            {
                email = email
            };

            return new Task<User?>(() => mockUser);
        }
    }
}
