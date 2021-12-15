using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto_BankingREG.Models.ViewModels
{
    public class TransakcijaView
    {
        public string NazivValute { get; set; }
        public string Kolicina { get; set; }
        public string VrstaTransakcije { get; set; }
        public string CryptoAdresa { get; set; }
    }
}
