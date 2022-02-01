using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto_BankingREG.Models.Service
{
    public class ApplicationUserService : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly MainContext _context;


        public ApplicationUserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper, MainContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ActionResult<ApplicationUser>> PostUser(ApplicationUserView applicationUser)
        {
            var app = _mapper.Map<ApplicationUser>(applicationUser);
            app = new ApplicationUser()
            {
                UserName = app.UserName,
                Email = app.Email,
                FullName = app.FullName,
                PasswordHash = applicationUser.Password
            };
            _context.ApplicationUsers.Add(app);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
