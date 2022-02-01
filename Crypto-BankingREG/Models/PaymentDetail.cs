using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto_BankingREG.Models
{
    public class PaymentDetail
    {
        [Key]
        public int Id { get; set; }


        [Column(TypeName = "nvarchar (100)")]
        public string NazivVlasnikaKartice { get; set; }


        [Column(TypeName = "nvarchar (16)")]
        public string BrojKartice { get; set; }


        [Column(TypeName = "nvarchar (5)")]
        public string DatumIstekaKartice { get; set; }


        [Column(TypeName = "nvarchar (3)")]
        public string CVV { get; set; }

        //Relacija 1:N - 1 kartica, više transakcija
        public List<TransakcijaModel> Transakcije { get; set; }

        //Relacija 1:1 - 1 User ima 1 karticu
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}
