using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spestqnko.Api.DTOs.Wallet;
using Spestqnko.Core.Models;
using Spestqnko.Core.Services;
using Spestqnko.Service.Exceptions;
using System.Net;
using AutoMapper;
using Spestqnko.Api.Settings;
using Microsoft.Extensions.Options;

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
        private readonly WalletInvitationSettings _invitationSettings;

        /// <summary>
        /// Initializes a new instance of the WalletController
        /// </summary>
        /// <param name="logger">The logger instance</param>
        /// <param name="walletService">The wallet service for wallet operations</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="invitationSettings">Wallet invitation settings</param>
        public WalletController(
            ILogger<WalletController> logger, 
            IWalletService walletService, 
            IMapper mapper,
            IOptions<WalletInvitationSettings> invitationSettings)
            : base(logger)
        {
            _walletService = walletService;
            _mapper = mapper;
            _invitationSettings = invitationSettings.Value;
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

        /// <summary>
        /// Creates a wallet invitation that can be shared with other users
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST /api/wallet/invite
        ///     {
        ///        "walletId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///        "expirationHours": 48
        ///     }
        /// Sample response:
        ///     {
        ///       "invitationCode": "4f7e6d5c-9b8a-7c6d-5e4f-3a2b1c0d9e8f",
        ///       "walletId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///       "walletName": "Household Budget",
        ///       "createdAt": "2023-04-20T12:34:56.789Z",
        ///       "expiresAt": "2023-04-22T12:34:56.789Z",
        ///       "isValid": true
        ///     }
        /// </remarks>
        /// <param name="dto">The data transfer object containing the wallet ID and expiration time</param>
        /// <returns>The created wallet invitation with its details</returns>
        /// <response code="201">Returns the created wallet invitation</response>
        /// <response code="400">If the invitation data is invalid</response>
        /// <response code="401">If the user is not authorized</response>
        /// <response code="404">If the wallet is not found</response>
        [HttpPost("invite")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WalletInvitationDTO>> CreateWalletInvitation([FromBody] CreateWalletInvitationDTO dto)
        {
            int expirationHours = dto.ExpirationHours > 0 ? dto.ExpirationHours : _invitationSettings.ExpirationTimeInHours;
            var invitation = await _walletService.CreateInvitationAsync(dto.WalletId, User.Id, expirationHours);
            var invitationDto = _mapper.Map<WalletInvitationDTO>(invitation);
            return CreatedAtAction(
                nameof(GetWalletById),
                new { id = invitation.WalletId },
                invitationDto);
        }

        /// <summary>
        /// Accepts a wallet invitation and joins the associated wallet
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST /api/wallet/accept-invitation
        ///     {
        ///        "invitationCode": "4f7e6d5c-9b8a-7c6d-5e4f-3a2b1c0d9e8f"
        ///     }
        /// Sample response:
        ///     {
        ///       "name": "Household Budget",
        ///       "allocatedIncome": 0,
        ///       "currency": {
        ///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "name": "US Dollar",
        ///         "code": "USD",
        ///         "symbol": "$"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="dto">The data transfer object containing the invitation code</param>
        /// <returns>The joined wallet with its details</returns>
        /// <response code="200">Returns the joined wallet</response>
        /// <response code="400">If the invitation is invalid, expired, or already used</response>
        /// <response code="401">If the user is not authorized</response>
        /// <response code="404">If the invitation is not found</response>
        [HttpPost("accept-invitation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WalletDTO>> AcceptWalletInvitation([FromBody] AcceptWalletInvitationDTO dto)
        {
            var wallet = await _walletService.AcceptInvitationAsync(dto.Id, User.Id);
            var walletDto = _mapper.Map<WalletDTO>(wallet);
            return Ok(walletDto);
        }

        /// <summary>
        /// Gets all invitations for a wallet, optionally filtering only valid invitations
        /// </summary>
        /// <param name="walletId">The wallet ID</param>
        /// <param name="validOnly">If true, only valid invitations are returned</param>
        /// <returns>List of WalletInvitationDTO</returns>
        [HttpGet("{walletId}/invitations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WalletInvitationDTO>>> GetInvitationsByWalletId(Guid walletId, [FromQuery] bool validOnly = false)
        {
            var invitations = await _walletService.GetInvitationsByWalletIdAsync(walletId, validOnly);
            var dtos = _mapper.Map<IEnumerable<WalletInvitationDTO>>(invitations);
            return Ok(dtos);
        }
    }
} 