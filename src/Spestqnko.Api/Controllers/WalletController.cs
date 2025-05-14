using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spestqnko.Api.DTOs.Wallet;
using Spestqnko.Core.Models;
using Spestqnko.Core.Services;
using Spestqnko.Service.Exceptions;
using System.Net;
using AutoMapper;

namespace Spestqnko.Api.Controllers
{
    /// <summary>
    /// API controller for managing wallets
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : BaseController
    {
        private readonly IWalletService _walletService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the WalletController
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="walletService">The wallet service for wallet operations</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public WalletController(ILogger<WalletController> logger, IWalletService walletService, IMapper mapper)
            : base(logger)
        {
            _walletService = walletService;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all wallets available to the current user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /api/wallet
        /// Sample response:
        ///     [
        ///       {
        ///         "name": "Household Budget",
        ///         "allocatedIncome": 5000,
        ///         "currency": {
        ///           "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///           "name": "US Dollar",
        ///           "code": "USD",
        ///           "symbol": "$"
        ///         }
        ///       }
        ///     ]
        /// </remarks>
        /// <returns>A collection of wallets with their properties and relationships</returns>
        /// <response code="200">Returns the list of wallets</response>
        /// <response code="401">If the user is not authorized</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<WalletDTO>>> GetAllWallets()
        {
            var wallets = await _walletService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<WalletDTO>>(wallets));
        }

        /// <summary>
        /// Retrieves a specific wallet by its unique identifier
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /api/wallet/3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// Sample response:
        ///     {
        ///       "name": "Household Budget",
        ///       "allocatedIncome": 5000,
        ///       "currency": {
        ///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "name": "US Dollar",
        ///         "code": "USD",
        ///         "symbol": "$"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="id">The unique identifier of the wallet</param>
        /// <returns>The wallet with the specified ID including its relationships</returns>
        /// <response code="200">Returns the requested wallet</response>
        /// <response code="404">If the wallet is not found</response>
        /// <response code="401">If the user is not authorized</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<WalletDTO>> GetWalletById(Guid id)
        {
            var wallet = await _walletService.GetByIdAsync(id) 
                ?? throw new AggregateAppException(HttpStatusCode.NotFound, $"Wallet with ID {id} not found");
            return Ok(_mapper.Map<WalletDTO>(wallet));
        }

        /// <summary>
        /// Creates a new wallet and associates it with the current user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST /api/wallet
        ///     {
        ///        "name": "Household Budget",
        ///        "allocatedIncome": 5000
        ///     }
        /// Sample response:
        ///     {
        ///       "name": "Household Budget",
        ///       "allocatedIncome": 5000,
        ///       "currency": {
        ///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "name": "US Dollar",
        ///         "code": "USD",
        ///         "symbol": "$"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="dto">The data transfer object containing wallet creation details</param>
        /// <returns>The newly created wallet with its ID and relationships</returns>
        /// <response code="201">Returns the newly created wallet</response>
        /// <response code="400">If the wallet data is invalid</response>
        /// <response code="401">If the user is not authorized</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<WalletDTO>> CreateWallet([FromBody] CreateWalletDTO dto)
        {
            var wallet = await _walletService.CreateWalletAsync(dto.Name, User.Id, dto.AllocatedIncome);
            var walletDto = _mapper.Map<WalletDTO>(wallet);
            
            return CreatedAtAction(
                nameof(GetWalletById), 
                new { id = wallet.Id }, 
                walletDto);
        }
    }
} 