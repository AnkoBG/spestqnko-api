using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spestqnko.Api.DTOs.Wallet;
using Spestqnko.Core.Models;
using Spestqnko.Core.Services;
using Spestqnko.Service.Exceptions;
using System.Net;

namespace Spestqnko.Api.Controllers
{
    [Authorize]
    [Route("api/wallet")]
    [ApiController]
    public class WalletController : BaseController
    {
        private readonly IWalletService _walletService;

        public WalletController(ILogger<WalletController> logger, IWalletService walletService)
            : base(logger)
        {
            _walletService = walletService;
        }

        /// <summary>
        /// Gets all wallets
        /// </summary>
        /// <returns>A list of all wallets</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wallet>>> GetAllWallets()
            => Ok(await _walletService.GetAll());

        /// <summary>
        /// Gets a specific wallet by ID
        /// </summary>
        /// <param name="id">The ID of the wallet to retrieve</param>
        /// <returns>The wallet with the specified ID</returns>
        [HttpGet("{id}")]
        public ActionResult<Wallet> GetWalletById(Guid id)
        {
            var wallet = _walletService.GetById(id);
            
            if (wallet == null)
            {
                throw new AppException(HttpStatusCode.NotFound, $"Wallet with ID {id} not found");
            }
            
            return Ok(wallet);
        }

        /// <summary>
        /// Creates a new wallet for the current user
        /// </summary>
        /// <param name="dto">The wallet creation details</param>
        /// <returns>The newly created wallet</returns>
        [HttpPost]
        public async Task<ActionResult<Wallet>> CreateWallet([FromBody] CreateWalletDTO dto)
        {
            if (User == null)
            {
                throw new AppException(HttpStatusCode.Unauthorized, "User authentication required");
            }

            var wallet = await _walletService.CreateWalletAsync(dto.Name, User.Id, dto.MonthlyIncome);
            
            return CreatedAtAction(
                nameof(GetWalletById), 
                new { id = wallet.Id }, 
                wallet);
        }
    }
} 