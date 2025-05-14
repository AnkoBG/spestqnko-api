using Microsoft.EntityFrameworkCore;
using Spestqnko.Core.Models;
using Spestqnko.Core.Repositories;
using Spestqnko.Core.Services;

namespace Spestqnko.Service
{
    public class CurrencyService : BaseService<Currency>, ICurrencyService
    {
        public CurrencyService(IRepositoryManager repositoryManager)
            : base(repositoryManager)
        {
        }
    }
} 