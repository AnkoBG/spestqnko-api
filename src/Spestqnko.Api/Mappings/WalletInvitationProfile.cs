using AutoMapper;
using Spestqnko.Api.DTOs.Wallet;
using Spestqnko.Core.Models;

namespace Spestqnko.Api.Mappings
{
    public class WalletInvitationProfile : Profile
    {
        public WalletInvitationProfile()
        {
            CreateMap<WalletInvitation, WalletInvitationDTO>();
                
            CreateMap<CreateWalletInvitationDTO, WalletInvitation>();
        }
    }
} 