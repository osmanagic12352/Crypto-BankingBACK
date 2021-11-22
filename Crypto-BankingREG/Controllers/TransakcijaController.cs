using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crypto_BankingREG.Models;

namespace Crypto_BankingREG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransakcijaController : ControllerBase
    {
        private readonly AuthenticationContext _context;

        public TransakcijaController(AuthenticationContext context)
        {
            _context = context;
        }

        // GET: api/TransakcijaModel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransakcijaModel>>> GetTransakcijaModel()
        {
            return await _context.Transakcija.ToListAsync();
        }
    }
}
