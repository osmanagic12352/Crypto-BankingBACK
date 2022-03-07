using AutoMapper;
using Crypto_BankingREG.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto_BankingREG.Models.Service
{
    public class PaymentDetailService : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private DBContext _context;
        private readonly IMapper _mapper;

        public PaymentDetailService(DBContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

       

        public List<PaymentDetail> GetAllPaymentDetails()
        {
            var allPaymentDetails = _context.PaymentDetails.ToList();
            return allPaymentDetails;
        }
        public PaymentDetail GetPaymentDetailById(int cardId)
        {
            var paymentDetail = _context.PaymentDetails.FirstOrDefault(n => n.Id == cardId);
            if (paymentDetail != null)
            {
                return paymentDetail;
            }
            else
            {
                throw new Exception("Uneseni Id je neispravan!");
            }
        }

        public PaymentDetail UpdatePaymentDetailById(int cardId, PaymentDetailView card)
        {
            var _card = _context.PaymentDetails.FirstOrDefault(n => n.Id == cardId);
            if (_card != null)
            {
                _card.NazivVlasnikaKartice = card.NazivVlasnikaKartice;
                _card.BrojKartice = card.BrojKartice;
                _card.DatumIstekaKartice = card.DatumIstekaKartice;
                _card.CVV = card.CVV;

                _context.SaveChanges();
                return _card;
            }
            else
            {
                throw new Exception("Mjenjane podataka nije upsjelo! Da li ste sve ispravno upisali, možda pogrešan Id?");
            }
        }

        public void DeletePaymentDetailById(int id)
        {
            var _card = _context.PaymentDetails.FirstOrDefault(n => n.Id == id);
            if (_card != null)
            {
                _context.PaymentDetails.Remove(_card);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Uneseni Id je neispravan!");
            }
        }
    }
}
