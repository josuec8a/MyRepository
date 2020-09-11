using ASP.Net_Core_3._0_Web_API.ApplicationCore.Entities;
using ASP.Net_Core_3._0_Web_API.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_Core_3._0_Web_API.Infraestructure.Data
{
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        public UserRepository(CatalogContext context) : base(context) { }

        public Task<User> GetByEmail(string email) => FirstOrDefault(w => w.Email == email);
    }
}
