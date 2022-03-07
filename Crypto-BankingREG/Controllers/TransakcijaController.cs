using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crypto_BankingREG.Models;
using Crypto_BankingREG.Models.Service;
using Crypto_BankingREG.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Crypto_BankingREG.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransakcijaController : ControllerBase
    {
        public TransakcijaService _transakcije;
        private readonly ILogger<TransakcijaController> _logger;


        public TransakcijaController(TransakcijaService transakcije, ILogger<TransakcijaController> logger)
        {
            _transakcije = transakcije;
            _logger = logger;
        }

        /// <summary>
        /// Dodavanje transakcije
        /// </summary> 
        [Authorize]
        [HttpPost("add-transakcija")]
        public IActionResult Addtransakciju([FromBody] TransakcijaView transakcije, string UserID)
        {
            try
            {
                _transakcije.AddTransakciju(transakcije, UserID);
                return Ok();
            }
            catch (Exception a)
            {
                _logger.LogError(a.ToString());
                return BadRequest(a.ToString());
            }
            
        }

        /// <summary>
        /// Dohvatanje svih transakcija
        /// </summary> 
        [Authorize(Roles = "Admin")]
        [HttpGet("Get-all-transakcije")]
        public IActionResult GetAllTransakcije()
        {
            var AllTransakcije = _transakcije.GetAllTransakcije();
            return Ok(AllTransakcije);
        }

        /// <summary>
        /// Dohvatanje pojedine transakcije
        /// </summary> 
        [Authorize (Roles ="Admin")]
        [HttpGet("get-transakciju-by-id/{id}")]
        public IActionResult GetTransakcijuById(int id)
        {
            try
            {
                var card = _transakcije.GetTransakcijuById(id);
                return Ok(card);
            }
            catch (Exception b)
            {
                _logger.LogError(b.ToString());
                return BadRequest(b.ToString());
            }
        }

        /// <summary>
        /// Uređivanje transakcije
        /// </summary> 
        [Authorize(Roles = "Admin")]
        [HttpPut("Edit-transakciju/{id}")]
        public IActionResult UpdateTransakcijuById(int id, [FromBody] TransakcijaView transakcija)
        {
            try
            {
                var transakcijaUpdate = _transakcije.UpdateTransakcijuById(id, transakcija);
                return Ok(transakcijaUpdate);

            }
            catch (Exception d)
            {
                _logger.LogError(d.ToString());
                return BadRequest(d.ToString());
            }
        }

        /// <summary>
        /// Brisanje transakcije
        /// </summary> 
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete-transakciju/{id}")]
        public IActionResult DeleteTransakcijuById(int id)
        {
            try
            {
                _transakcije.DeleteTransakcijuById(id);
                return Ok();
            }
            catch (Exception c)
            {
                _logger.LogError(c.ToString());
                return BadRequest(c.ToString());
            }
        }
    }
}
