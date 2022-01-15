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
    public class UserWalletCategoryRepository : Repository<UserWalletCategory>, IUserWalletCategoryRepository
    {
        public UserWalletCategoryRepository(DbContext context)
            : base(context)
        { }
    }
}
