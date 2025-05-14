using AutoMapper;
using Spestqnko.Api.DTOs.Currency;
using Spestqnko.Core.Models;

namespace Spestqnko.Api.Mappings
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<Currency, CurrencyDTO>();
            CreateMap<CurrencyDTO, Currency>();
        }
    }
} 