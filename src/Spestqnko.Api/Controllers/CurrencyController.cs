using Microsoft.AspNetCore.Mvc;
using Spestqnko.Api.DTOs.Currency;
using Spestqnko.Core.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Spestqnko.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication by default
    public class CurrencyController : BaseController
    {
        private readonly ICurrencyService _currencyService;
        private readonly IMapper _mapper;

        public CurrencyController(
            ILogger<CurrencyController> logger,
            ICurrencyService currencyService,
            IMapper mapper) 
            : base(logger)
        {
            _currencyService = currencyService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrencyDTO>>> GetAll()
        {
            var currencies = await _currencyService.GetAllAsync();

            return Ok(_mapper.Map<IEnumerable<CurrencyDTO>>(currencies));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CurrencyDTO>> GetById(Guid id)
        {
            var currency = await _currencyService.GetByIdAsync(id);
            
            if (currency == null)
            {
                return NotFound();
            }
            
            var currencyDto = _mapper.Map<CurrencyDTO>(currency);
            return Ok(currencyDto);
        }
    }
} 