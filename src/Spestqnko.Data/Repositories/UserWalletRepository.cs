using Microsoft.EntityFrameworkCore;
using Spestqnko.Core.Models;
using Spestqnko.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Spestqnko.Data.Repositories
{
    public class UserWalletRepository : Repository<UserWallet>, IUserWalletRepository
    {
        public UserWalletRepository(DbContext context)
            : base(context)
        { }
    }
}
