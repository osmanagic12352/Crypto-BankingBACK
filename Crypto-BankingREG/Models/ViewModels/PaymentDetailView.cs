using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto_BankingREG.Models.ViewModels
{
    public class PaymentDetailView
    {
        public string NazivVlasnikaKartice { get; set; }
        public string BrojKartice { get; set; }
        public string DatumIstekaKartice { get; set; }
        public string CVV { get; set; }
    }
}
