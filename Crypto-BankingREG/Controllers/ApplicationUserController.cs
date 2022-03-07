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
using Microsoft.Extensions.Logging;

namespace Crypto_BankingREG.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public ApplicationUserService _user;
        private readonly ILogger<ApplicationUserController> _logger;
        private DBContext _context;
        private IPasswordHasher<ApplicationUser> _passwordHasher;


        public ApplicationUserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ApplicationUserService user, ILogger<ApplicationUserController> logger, DBContext context, IPasswordHasher<ApplicationUser> passwordHash)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _user = user;
            _logger = logger;
            _context = context;
            _passwordHasher = passwordHash;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelView login)
        {
            var UserCheck = await _userManager.FindByNameAsync(login.UserName);
            if (UserCheck != null && await _userManager.CheckPasswordAsync(UserCheck, login.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(UserCheck);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, UserCheck.UserName),
                    new Claim("UserID", UserCheck.Id.ToString()),
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
                    expires: DateTime.Now.AddHours(5),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(LoginKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(TokenSettings) });
            }
            return Unauthorized();
        }
        

        /// <summary>
        /// Registracija korisnika
        /// </summary> 
        /// <remarks>
        /// Password mora sadržazi minimalno:<br>
        /// 6 znakova</br>
        /// 1 broj
        /// </remarks>
        [HttpPost("Registracija_User")]
        public async Task<IActionResult> InsertUser([FromBody] ApplicationUserView userView)
        {
            try
            {
                await _user.InsertUser(userView);
                return Ok(new Response { Status = "Uspješno", Message = "Uspješno registriran korisnik!" });
            }
            catch (Exception b)
            {
                _logger.LogError(b.ToString());
                return BadRequest(b.ToString());
            }
            
        }

        /// <summary>
        /// Registracija Admina
        /// </summary> 
        /// <remarks>
        /// Password mora sadržazi minimalno:<br>
        /// 6 znakova</br>
        /// 1 broj
        /// </remarks>
        [HttpPost]
        [Route("Registracija_Admin")]
        public async Task<IActionResult> InsertAdmin([FromBody] ApplicationUserView adminView)
        {
            try
            {
                await _user.InsertAdmin(adminView);
                return Ok(new Response { Status = "Uspješno", Message = "Uspješno registriran administrator!" });
            }
            catch (Exception c)
            {
                _logger.LogError(c.ToString());
                return BadRequest(c.ToString());
            }

        }

        [Authorize]
        [HttpGet("get-loged-UserInfo")]      
        public async Task<Object> GetLogedUser()
        {
            string userId = User.Claims.First(a => a.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            return new
            {
                user.Email,
                user.UserName,
                user.FullName
            };
        }

        [Authorize]
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(ApplicationUser userView, string password)
        {
            string userId = User.Claims.First(a => a.Type == "UserID").Value;
            var _user = await _userManager.FindByIdAsync(userId);
            if (_user != null)
            {
                _user.UserName = userView.UserName;
                _user.Email = userView.Email;
                _user.FullName = userView.FullName;
                _user.PasswordHash = _passwordHasher.HashPassword(_user, password); 

                var result = await _userManager.UpdateAsync(_user);
                return Ok(result);
            }
            else
            {
                throw new Exception("Greška! Niste unjeli dobar Id usera?");

            }
        }


        /// <summary>
        /// Dohvatanje svih korisnika
        /// </summary>

        [Authorize (Roles ="Admin")]
        [HttpGet("get-all-users")]
        public IActionResult GetAllUsers()
        {
            var allUsers = _user.GetAllUsers();
            return Ok(allUsers);
        }

        /// <summary>
        /// Dohvatanje odabranog korisnika
        /// </summary>

        [Authorize(Roles = "Admin")]
        [HttpGet("get-user-by-id/{id}")]
        public IActionResult GetUserById(string id)
        {
            var GetUser = _user.GetUserById(id);
            return Ok(GetUser);
        }

        /// <summary>
        /// Uređivanje korisnika
        /// </summary>

        [Authorize(Roles = "Admin")]
        [HttpPut("Edit-user-details/{id}")]
        public IActionResult UpdateUserById(string id, [FromBody] ApplicationUserView user)
        {
            try
            {
                var UserUpdate = _user.UpdateUserById(id, user);
                return Ok(UserUpdate);
            }
            catch (Exception d)
            {
                _logger.LogError(d.ToString());
                return BadRequest(d.ToString());
            }
            
        }


        /// <summary>
        /// Brisanje korisnika
        /// </summary> 

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete-card/{id}")]
        public IActionResult DeleteUserById(string id)
        {
            try
            {
                _user.DeleteUserById(id);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return BadRequest(e.ToString());
            }
            
        }






        //LOGIN

        ///// <summary>
        ///// Logiranje korisnika
        ///// </summary>    
        //[HttpPost]
        //[Route("login")]
        //public IActionResult Login([FromBody] LoginModelView userLogin)
        //{
        //    try
        //    {
        //        var login = _user.Login(userLogin);
        //        return Ok(login);
        //    }
        //    catch (Exception a)
        //    {
        //        _logger.LogError(a.ToString());
        //        return BadRequest(a.ToString());
        //    }
        //}


        //[HttpPost]
        //[Route("Registracija_Admin")]
        //public IActionResult InsertAdmin([FromBody] ApplicationUserView adminView)
        //{
        //    try
        //    {
        //        _user.InsertAdmin(adminView);
        //        return Ok(new Response { Status = "Uspješno", Message = "Uspješno registriran administrator!" });
        //    }
        //    catch (Exception b)
        //    {
        //        _logger.LogError(b.ToString());
        //        return BadRequest(b.ToString());
        //    }

        //}



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

