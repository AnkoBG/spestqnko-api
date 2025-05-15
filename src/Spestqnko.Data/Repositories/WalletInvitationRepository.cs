using Microsoft.EntityFrameworkCore;
using Spestqnko.Core.Models;
using Spestqnko.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spestqnko.Data.Repositories
{
    public class WalletInvitationRepository : Repository<WalletInvitation>, IWalletInvitationRepository
    {
        public WalletInvitationRepository(DbContext context)
            : base(context)
        { }
    }
} 