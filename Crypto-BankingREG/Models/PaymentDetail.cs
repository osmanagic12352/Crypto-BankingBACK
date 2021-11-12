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
        public int UplataId { get; set; }


        [Column(TypeName = "nvarchar (100)")]
        public string NazivVlasnikaKartice { get; set; }


        [Column(TypeName = "nvarchar (16)")]
        public string BrojKartice { get; set; }


        [Column(TypeName = "nvarchar (5)")]
        public string DatumIstekaKartice { get; set; }


        [Column(TypeName = "nvarchar (3)")]
        public string CVV { get; set; }
    }
}
