using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto_BankingREG.Models.Service
{
    public class ApplicationUserService
    {
        private AuthenticationContext _context;

        public ApplicationUserService(AuthenticationContext context) 
        {
            _context = context;
        }

        public void AddApplicationUser(ApplicationUserView user)
        {
            var _user = new ApplicationUser()
            {

            };
        }
    }
}
