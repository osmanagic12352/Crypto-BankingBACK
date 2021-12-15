using Crypto_BankingREG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto_BankingREG.Models.Service
{
    public class PaymentDetailService
    {
        private AuthenticationContext _context;

        public PaymentDetailService(AuthenticationContext context)
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
            var cardId = _context.PaymentDetails.FirstOrDefault(n => n.UplataId == cardId)
            return cardId;
        }
    }
}
