using AutoMapper;
using Spestqnko.Api.DTOs.Wallet;
using Spestqnko.Core.Models;

namespace Spestqnko.Api.Mappings
{
    public class WalletProfile : Profile
    {
        public WalletProfile()
        {
            CreateMap<Wallet, WalletDTO>()
                .ForMember(dest => dest.AllocatedIncome, opt => opt.MapFrom(src => 
                    src.UserWallets.Count > 0 ? src.UserWallets.Sum(uw => uw.AllocatedIncome) : 0));
                
            CreateMap<CreateWalletDTO, Wallet>();
        }
    }
} 