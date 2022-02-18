//using AutoMapper;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Crypto_BankingREG.Models.Service
//{
//    public class ApplicationUserService : ControllerBase
//    {
//        private UserManager<ApplicationUser> _userManager;
//        private SignInManager<ApplicationUser> _signInManager;
//        private readonly IMapper _mapper;
//        private readonly DBContext _context;


//        public ApplicationUserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper, DBContext context)
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _mapper = mapper;
//            _context = context;
//        }

//        public void AddUser(ApplicationUserView applicationUser)
//        {
//            var _app = _mapper.Map<ApplicationUser>(applicationUser);

//            _app = new ApplicationUser()
//            {
//                UserName = applicationUser.UserName,
//                Email = applicationUser.Email,
//                FullName = applicationUser.FullName,
//                PasswordHash = applicationUser.Password
//            };
//            _context.ApplicationUsers.Add(_app);
//            _context.SaveChangesAsync();         
//        }
//    }
//}
