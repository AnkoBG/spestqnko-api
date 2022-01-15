using Spestqnko.Core;
using Spestqnko.Core.Models;
using Spestqnko.Core.Services;

namespace Spestqnko.Service
{
    public class UserService : BaseService<User>, IUserService
    {
        public UserService(IUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
        }

        public async Task<User> AddUserAsync(string username, string password)
        {
            var user = new User()
            {
                UserName = username,
                PasswordHash = password
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();

            return user;
        }
    }
}