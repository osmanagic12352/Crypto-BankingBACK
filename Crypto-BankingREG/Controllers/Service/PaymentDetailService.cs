using AutoMapper;
using Crypto_BankingREG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto_BankingREG.Models.Service
{
    public class PaymentDetailService
    {
        private DBContext _context;
        private readonly IMapper _mapper;

        public PaymentDetailService(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddPaymentDetail(PaymentDetailView card, string id)
        {
            var _user = _context.ApplicationUsers.FirstOrDefault(n => n.Id == id);
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
                    UserId = id
                };
                    _context.PaymentDetails.Add(_card);
                    _context.SaveChanges();
            }
            else
            {
                throw new Exception("Unjeli ste nepostojeći Id korisnika ili se isti već koristi!");
            }            
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

        public void DeletePaymentDetailById(int cardId)
        {
            var _card = _context.PaymentDetails.FirstOrDefault(n => n.Id == cardId);
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
