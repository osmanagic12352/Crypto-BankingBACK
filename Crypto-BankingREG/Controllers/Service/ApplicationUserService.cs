using AutoMapper;
using Crypto_BankingREG.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_BankingREG.Models.Service
{
    public class ApplicationUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApplicationUserService> _logger;
        private DBContext _context;
        private readonly IMapper _mapper;


        public ApplicationUserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, DBContext context, ILogger<ApplicationUserService> logger, IMapper mapper) { 
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> Login([FromBody] LoginModelView userLogin)
        {
            var UserCheck = await _userManager.FindByNameAsync(userLogin.UserName);
            if (UserCheck != null && await _userManager.CheckPasswordAsync(UserCheck, userLogin.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(UserCheck);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, UserCheck.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var LoginKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("CryptoBanking123"));

                var TokenSettings = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    expires: DateTime.Now.AddHours(2),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(LoginKey, SecurityAlgorithms.HmacSha256)
                    );
                var Token = new JwtSecurityTokenHandler().WriteToken(TokenSettings);
                return Token;
            }
            else
            {
                throw new Exception("Pogrešan Username ili Password!");
            }
        }


        public async Task InsertUser(ApplicationUserView userView) {


            var UserCheck = await _userManager.FindByNameAsync(userView.UserName);
            if (UserCheck != null)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Greška", Message = "Kreiranje korisnika nije uspjelo!" });
                throw new ApplicationException("Username već postoji!");
            }
            var EmailCheck = await _userManager.FindByEmailAsync(userView.Email);
            if (EmailCheck != null)
            {
                throw new ApplicationException("Email već postoji!");
            }

            var user = _mapper.Map<ApplicationUser>(userView);
                user = new ApplicationUser()
                {
                    UserName = userView.UserName,
                    Email = userView.Email,
                    FullName = userView.FullName,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
            //_context.ApplicationUsers.Add(user);
            //_context.SaveChanges();

            var result = await _userManager.CreateAsync(user, userView.Password);
            if (!result.Succeeded)
                //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Greška", Message = "Kreiranje korisnika nije uspjelo!" });
                throw new Exception("Greška u registraciji. Da li ste pripazili na dodavanje brojeva, jednog karaktera specifičnog i veliko slovo?");
        }

        public async Task InsertAdmin(ApplicationUserView adminView)
        {
            var UserCheck = await _userManager.FindByNameAsync(adminView.UserName);
            if (UserCheck != null)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Greška", Message = "Kreiranje korisnika nije uspjelo!" });
                throw new ApplicationException("Username već postoji!");
            }
            var EmailCheck = await _userManager.FindByEmailAsync(adminView.Email);
            if (EmailCheck != null)
            {
                throw new ApplicationException("Email već postoji!");
            }

            var admin = _mapper.Map<ApplicationUser>(adminView);
                admin = new ApplicationUser()
            {
                UserName = adminView.UserName,
                Email = adminView.Email,
                FullName = adminView.FullName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(admin, adminView.Password);
            if (!result.Succeeded)
                //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Greška", Message = "Kreiranje korisnika nije uspjelo!" });
                throw new Exception("Greška u Bazi!");


            if (!await _roleManager.RoleExistsAsync(Roles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));

            if (!await _roleManager.RoleExistsAsync(Roles.User))
                await _roleManager.CreateAsync(new IdentityRole(Roles.User));

            if (await _roleManager.RoleExistsAsync(Roles.Admin))
                await _userManager.AddToRoleAsync(admin, Roles.Admin);
        }


            public void DeleteUserById(string id)
        {
            var _user = _context.ApplicationUsers.FirstOrDefault(n => n.Id == id);
            if (_user != null)
            {
                _context.ApplicationUsers.Remove(_user);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Brisanje korisnika nije uspjelo!");

            }
        }

        public List<ApplicationUser> GetAllUsers()
        {
            var allUsers = _context.ApplicationUsers.ToList();
            return allUsers;
        }
        public ApplicationUser GetUserById(string Id)
        {
            var GetUser = _context.ApplicationUsers.FirstOrDefault(n => n.Id == Id);
            return GetUser;
        }

        public ApplicationUser UpdateUserById(string Id, ApplicationUserView userView)
        {
            var _user = _context.ApplicationUsers.FirstOrDefault(n => n.Id == Id);
            if (_user != null)
            {
                _user.UserName = userView.UserName;
                _user.Email = userView.Email;
                _user.FullName = userView.FullName;
                _user.PasswordHash = userView.Password;

                _context.SaveChanges();
                return _user;
            }
            else
            {
                throw new Exception("Greška! Niste unjeli dobar Id usera?");
            }
        }

    }
    
}

//public async Task InsertAdmin(ApplicationUserView adminView)
//{
//    var UserCheck = _context.ApplicationUsers.Any(x => x.UserName == adminView.UserName);
//    if (UserCheck)
//    {
//        //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Greška", Message = "Kreiranje korisnika nije uspjelo!" });
//        throw new ApplicationException("Username već postoji!");
//    }
//    var EmailCheck = _context.ApplicationUsers.Any(x => x.Email == adminView.Email);
//    if (EmailCheck)
//    {
//        throw new ApplicationException("Email već postoji!");
//    }

//    var admin = new ApplicationUser()
//    {
//        UserName = adminView.UserName,
//        Email = adminView.Email,
//        FullName = adminView.FullName,
//        PasswordHash = adminView.Password,
//        SecurityStamp = Guid.NewGuid().ToString()
//    };                   
//        _context.ApplicationUsers.Add(admin);
//        _context.SaveChanges();

//    if (!await _roleManager.RoleExistsAsync(Roles.Admin))
//        await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));

//    if (!await _roleManager.RoleExistsAsync(Roles.User))
//        await _roleManager.CreateAsync(new IdentityRole(Roles.User));

//    if (await _roleManager.RoleExistsAsync(Roles.Admin))
//        await _userManager.AddToRoleAsync(admin, Roles.Admin);

//    var result = await _userManager.CreateAsync(admin);
//    if (!result.Succeeded)
//        throw new Exception("Greška u Bazi!");

//return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Greška", Message = "Kreiranje korisnika nije uspjelo!" });




//public void InsertUser(ApplicationUserView userView)
//{


//    var UserCheck = _context.ApplicationUsers.Any(x => x.UserName == userView.UserName);
//    if (UserCheck)
//    {
//        //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Greška", Message = "Kreiranje korisnika nije uspjelo!" });
//        throw new ApplicationException("Username već postoji!");
//    }
//    var EmailCheck = _context.ApplicationUsers.Any(x => x.Email == userView.Email);
//    if (EmailCheck)
//    {
//        throw new ApplicationException("Email već postoji!");
//    }

//    var user = _mapper.Map<ApplicationUser>(userView);
//    user = new ApplicationUser()
//    {
//        UserName = userView.UserName,
//        Email = userView.Email,
//        FullName = userView.FullName,
//        PasswordHash = userView.Password,
//        SecurityStamp = Guid.NewGuid().ToString()
//    };
//    try
//    {
//        _context.ApplicationUsers.Add(user);
//        _context.SaveChanges();
//    }
//    catch
//    {

//        var result = _userManager.CreateAsync(user);
//        if (result.IsFaulted)
//            //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Greška", Message = "Kreiranje korisnika nije uspjelo!" });
//            throw new Exception("Greška u Bazi!");
//    }


//}

//var token = _mapper.Map<LoginModelView>(UserCheck);
//token.JWT = new JwtSecurityTokenHandler().WriteToken(TokenSettings);
//return token.JWT;
