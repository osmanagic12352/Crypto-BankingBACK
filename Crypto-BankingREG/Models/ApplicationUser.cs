using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto_BankingREG.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "nvarchar(150)")]
        public string FullName { get; set; }

        //Relacija 1:N - 1 User, više transakcija
        public List<TransakcijaModel> Transakcije { get; set; }

        //Relacija 1:1 - 1 User ima 1 karticu
        public PaymentDetail PaymentDetail { get; set; }
    }
}
