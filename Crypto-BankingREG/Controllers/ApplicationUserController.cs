using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Crypto_BankingREG.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Crypto_BankingREG.Models.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Crypto_BankingREG.Models.ViewModels;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Crypto_BankingREG.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        //public ApplicationUserService _user;


        public ApplicationUserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration/*, ApplicationUserService user*/)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            //_user = user;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] ApplicationUserView userView)
        {
            var UserCheck = await _userManager.FindByNameAsync(userView.UserName);
            if (UserCheck != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Greška", Message = "Username zauzet!" });

            ApplicationUser user = new()
            {
                UserName = userView.UserName,
                Email = userView.Email,
                Admin = "NE",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, userView.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Greška", Message = "Kreiranje korisnika nije uspjelo!" });

            return Ok(new Response { Status = "Uspješno", Message = "Uspješno registriran korisnik!" });
        }

        [HttpPost]
        [Route("AdminRegister")]
        public async Task<IActionResult> Admin([FromBody] ApplicationUserView userView) 
        {
            var UserCheck = await _userManager.FindByNameAsync(userView.UserName);
            if (UserCheck != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Greška", Message = "Username zauzet!" });

            ApplicationUser user = new()
            {
                UserName = userView.UserName,
                Email = userView.Email,
                FullName = userView.FullName,
                Admin = "DA",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, userView.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Greška", Message = "Kreiranje korisnika nije uspjelo!" });


            if (!await _roleManager.RoleExistsAsync(Roles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));

            if (!await _roleManager.RoleExistsAsync(Roles.User))
                await _roleManager.CreateAsync(new IdentityRole(Roles.User));

            if (await _roleManager.RoleExistsAsync(Roles.Admin))
                await _userManager.AddToRoleAsync(user, Roles.Admin);

            return Ok(new Response { Status = "Uspješno", Message = "Uspješno registriran Administrator!" });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelView userLogin) 
        {
            var UserCheck = await _userManager.FindByNameAsync(userLogin.UserName);
            if(UserCheck != null && await _userManager.CheckPasswordAsync(UserCheck, userLogin.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(UserCheck);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, UserCheck.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach(var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var LoginKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("CryptoBanking123"));

                var token = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    expires: DateTime.Now.AddHours(2),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(LoginKey, SecurityAlgorithms.HmacSha256)
                    );
                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo });
            }

            return Unauthorized();
        }




















        //[HttpPost("add-user")]
        //public IActionResult AddUser([FromBody] ApplicationUserView user)
        //{
        //    _user.AddUser(user);
        //    return Ok();
        //}



        // POST /api/ApplicationUser/Register
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetApplicationUser()
        //{
        //    return await _context.ApplicationUsers.ToListAsync();
        //}

        //[HttpPost]
        //public async Task<ActionResult<ApplicationUser>> PostPaymentDetail(ApplicationUserView applicationUser)
        //{
        //    var app = _mapper.Map<ApplicationUser>(applicationUser);
        //    app = new ApplicationUser()
        //    {
        //        UserName = app.UserName,
        //        Email = app.Email,
        //        FullName = app.FullName,
        //        PasswordHash = applicationUser.Password
        //    };
        //    _context.ApplicationUsers.Add(app);
        //    await _context.SaveChangesAsync();

        //    return Ok();
        //}

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

