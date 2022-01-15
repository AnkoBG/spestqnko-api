using Spestqnko.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spestqnko.Core.Services
{
    public interface IUserService : IService<User>
    {
        public Task<User> AddUserAsync(string username, string password);
    }
}
