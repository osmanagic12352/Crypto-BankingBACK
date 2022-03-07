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
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace Crypto_BankingREG.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentDetailController : ControllerBase
    {
        public PaymentDetailService _card;
        private readonly ILogger<PaymentDetailController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private DBContext _context;
        private readonly IMapper _mapper;

        public PaymentDetailController(PaymentDetailService card, ILogger<PaymentDetailController> logger, UserManager<ApplicationUser> userManager, DBContext context, IMapper mapper)
        {
            _logger = logger;
            _card = card;
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Dodavanje kartice
        /// </summary> 
        [Authorize]
        [HttpPost("add-card")]
        public async Task<IActionResult> AddPaymentDetail(PaymentDetailView card)
        {
            string userId = User.Claims.First(a => a.Type == "UserID").Value;
            var _user = await _userManager.FindByIdAsync(userId);
            if (_user != null)
            {
                if (_context.PaymentDetails.Any(a => a.BrojKartice == card.BrojKartice))
                    throw new Exception("Sljedeći broj kartice se već koristi:" + card.BrojKartice);
                var _card = _mapper.Map<PaymentDetail>(card);
                _card = new PaymentDetail()
                {
                    NazivVlasnikaKartice = card.NazivVlasnikaKartice,
                    BrojKartice = card.BrojKartice,
                    DatumIstekaKartice = card.DatumIstekaKartice,
                    CVV = card.CVV,
                    UserId = userId
                };
                _context.PaymentDetails.Add(_card);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                throw new Exception("Unjeli ste nepostojeći Id korisnika ili se isti već koristi!");
            }
        }

        /// <summary>
        /// Dohvatanje svih kartica
        /// </summary> 
        [Authorize (Roles ="Admin")]
        [HttpGet("get-all-cards")]
        public IActionResult GetAllPaymentDetails()
        {
            var allPaymentDetails = _card.GetAllPaymentDetails();
            return Ok(allPaymentDetails);
        }

        /// <summary>
        /// Dohvatanje odabrane korisnika
        /// </summary> 
        [Authorize (Roles ="Admin")]
        [HttpGet("get-card-by-id/{id}")]
        public IActionResult GetPaymentDetailById(int id)
        {
            try
            {
                var card = _card.GetPaymentDetailById(id);
                return Ok(card);

            }
            catch (Exception d)
            {
                _logger.LogError(d.ToString());
                return BadRequest(d.ToString());
            }
        }

        /// <summary>
        /// Uređivanje podataka kartice
        /// </summary> 
        [Authorize (Roles ="Admin")]
        [HttpPut("Edit-card-details/{id}")]
        public IActionResult UpdatePaymentDetailById(int id, [FromBody]PaymentDetailView card)
        {
            try
            {
                var cardUpdate = _card.UpdatePaymentDetailById(id, card);
                return Ok(cardUpdate);
            }
            catch (Exception b)
            {
                _logger.LogError(b.ToString());
                return BadRequest(b.ToString());
            }
        }

        /// <summary>
        /// Brisanje kartice
        /// </summary> 
        [Authorize (Roles ="Admin")]
        [HttpDelete("Delete-card/{id}")]
        public IActionResult DeletePaymentDetailById(int id)
        {
            try
            {
                _card.DeletePaymentDetailById(id);
                return Ok();
            }
            catch (Exception c)
            {
                _logger.LogError(c.ToString());
                return BadRequest(c.ToString());
            }
            
        }





















        //private readonly AuthenticationContext _context;

        //public PaymentDetailController(AuthenticationContext context)
        //{
        //    _context = context;
        //}

        //// GET: api/PaymentDetail
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<PaymentDetail>>> GetPaymentDetails()
        //{
        //    return await _context.PaymentDetails.ToListAsync();
        //}

        //// GET: api/PaymentDetail/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<PaymentDetail>> GetPaymentDetail(int id)
        //{
        //    var paymentDetail = await _context.PaymentDetails.FindAsync(id);

        //    if (paymentDetail == null)
        //    {
        //        return NotFound();
        //    }

        //    return paymentDetail;
        //}

        //// PUT: api/PaymentDetail/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutPaymentDetail(int id, PaymentDetail paymentDetail)
        //{
        //    if (id != paymentDetail.UplataId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(paymentDetail).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PaymentDetailExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/PaymentDetail
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<PaymentDetail>> PostPaymentDetail(PaymentDetail paymentDetail)
        //{
        //    _context.PaymentDetails.Add(paymentDetail);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetPaymentDetail", new { id = paymentDetail.UplataId }, paymentDetail);
        //}

        //// DELETE: api/PaymentDetail/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeletePaymentDetail(int id)
        //{
        //    var paymentDetail = await _context.PaymentDetails.FindAsync(id);
        //    if (paymentDetail == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.PaymentDetails.Remove(paymentDetail);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool PaymentDetailExists(int id)
        //{
        //    return _context.PaymentDetails.Any(e => e.UplataId == id);
        //}
    }
}
