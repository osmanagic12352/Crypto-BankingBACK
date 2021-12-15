using AutoMapper;
using Crypto_BankingREG.Models;
using Crypto_BankingREG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto_BankingREG
{
    public class Automapper : Profile
    {
        public Automapper()
        {
            CreateMap<ApplicationUserView, ApplicationUser>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

            CreateMap<PaymentDetailView, PaymentDetail>();

            CreateMap<TransakcijaView, TransakcijaModel>();
        }

    }
}
