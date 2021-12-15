using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto_BankingREG.Models
{
    public class TransakcijaModel
    {
        [Key]
        public int TransakcijaId { get; set; }


        [Column(TypeName = "nvarchar (20)")]
        public string NazivValute { get; set; }


        [Column(TypeName = "decimal (10,4)")]
        public decimal Kolicina { get; set; }


        [Column(TypeName = "nvarchar (8)")]
        public string VrstaTransakcije { get; set; }


        [Column(TypeName = "nvarchar (100)")]
        public string CryptoAdresa { get; set; }
    }
}
