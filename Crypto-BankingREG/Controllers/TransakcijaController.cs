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

namespace Crypto_BankingREG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransakcijaController : ControllerBase
    {
        public TransakcijaService _transakcije;
        public TransakcijaController(TransakcijaService transakcije)
        {
            _transakcije = transakcije;
        }

        [HttpPost("add-transakcija")]
        public IActionResult Addtransakciju([FromBody] TransakcijaView transakcije)
        {
            _transakcije.AddTransakciju(transakcije);
            return Ok();
        }

        [HttpGet("Get-all-transakcije")]
        public IActionResult GetAllTransakcije()
        {
            var AllTransakcije = _transakcije.GetAllTransakcije();
            return Ok(AllTransakcije);
        }

        [HttpGet("get-transakciju-by-id/{id}")]
        public IActionResult GetTransakcijuById(int id)
        {
            var card = _transakcije.GetTransakcijuById(id);
            return Ok(card);
        }

        [HttpPut("Edit-transakciju/{id}")]
        public IActionResult UpdateTransakcijuById(int id, [FromBody] TransakcijaView transakcija)
        {
            var transakcijaUpdate = _transakcije.UpdateTransakcijuById(id, transakcija);
            return Ok(transakcijaUpdate);
        }

        [HttpDelete("Delete-transakciju/{id}")]
        public IActionResult DeleteTransakcijuById(int id)
        {
            _transakcije.DeleteTransakcijuById(id);
            return Ok();
        }
    }
}
