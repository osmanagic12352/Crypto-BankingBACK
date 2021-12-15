using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Crypto_BankingREG.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Crypto_BankingREG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly AuthenticationContext _context;


        public ApplicationUserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper, AuthenticationContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _context = context;
        }

        // POST /api/ApplicationUser/Register
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetApplicationUser()
        {
            return await _context.ApplicationUsers.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> PostPaymentDetail(ApplicationUserView applicationUser)
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

        //[HttpPost]
        //[Route("Register")]
        //public IActionResult PostApplicationUser(ApplicationUserModel model)
        //{
        //    var applicationUser = _mapper.Map<ApplicationUser>(model);
        //    applicationUser = new ApplicationUser()
        //    {
        //        UserName = model.UserName,
        //        Email = model.Email,
        //        FullName = model.FullName,
        //        PasswordHash = model.Password
        //    };
        //    //try
        //    //{
        //    //    var result = await _userManager.CreateAsync(applicationUser, model.Password);
        //    //    return Ok(result);
        //    //}
        //    //catch (Exception ex)
        //    //{

        //    //    throw ex;
        //    //}
        //    return Ok();

            
        //    }

        }
    }

