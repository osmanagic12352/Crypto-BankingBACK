using AutoMapper;
using Crypto_BankingREG.Models;
using Crypto_BankingREG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto_BankingREG
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Automapper : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        public Automapper()
        {
            CreateMap<ApplicationUserView, ApplicationUser>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

            CreateMap<PaymentDetailView, PaymentDetail>();

            CreateMap<TransakcijaView, TransakcijaModel>();

            CreateMap<LoginModelView, ApplicationUser>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
        }

    }
}
