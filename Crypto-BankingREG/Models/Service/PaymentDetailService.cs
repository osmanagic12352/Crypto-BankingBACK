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

        public PaymentDetailService(DBContext context)
        {
            _context = context;
        }

        public void AddPaymentDetail(PaymentDetailView card)
        {
            var _card = new PaymentDetail()
            {
                NazivVlasnikaKartice = card.NazivVlasnikaKartice,
                BrojKartice = card.BrojKartice,
                DatumIstekaKartice = card.DatumIstekaKartice,
                CVV = card.CVV
            };
            _context.PaymentDetails.Add(_card);
            _context.SaveChanges();
        }

        public List<PaymentDetail> GetAllPaymentDetails()
        {
            var allPaymentDetails = _context.PaymentDetails.ToList();
            return allPaymentDetails;
        }
        public PaymentDetail GetPaymentDetailById(int cardId)
        {
            var paymentDetail = _context.PaymentDetails.FirstOrDefault(n => n.Id == cardId);
            return paymentDetail;
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
            }
            return _card;
        }

        public void DeletePaymentDetailById(int cardId)
        {
            var _card = _context.PaymentDetails.FirstOrDefault(n => n.Id == cardId);
            if (_card != null)
            {
                _context.PaymentDetails.Remove(_card);
                _context.SaveChanges();
            }
        }
    }
}
