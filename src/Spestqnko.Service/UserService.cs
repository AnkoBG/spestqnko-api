using Spestqnko.Core;
using Spestqnko.Core.Models;
using Spestqnko.Core.Services;
using Spestqnko.Service.Exceptions;
using System.Net;
using System.Text;

namespace Spestqnko.Service
{
    public class UserService : BaseModelService<User>, IUserService
    {
        public UserService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public async Task<User> AddUserAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException(HttpStatusCode.BadRequest, "Password is required");

            if (_unitOfWork.Users.Find(x => x.UserName == username).Any())
                throw new AppException(HttpStatusCode.BadRequest, $"Username {username} is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            var user = new User()
            {
                UserName = username,
                PWHash = passwordHash,
                PWSalt = passwordSalt
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();

            return user;
        }

        public async Task<User>? Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new AppException(HttpStatusCode.BadRequest, "Missing Password or Username.");

            var user = _unitOfWork.Users.Find(x => x.UserName == username).SingleOrDefault();

            // check if username exists
            if (user == null || !VerifyPasswordHash(password, user))
                throw new AppException("Invalid credentials");

            // authentication successful
            return user;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, User user)
        {
            if (password == null)
                throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (user.PWHash.Length != 64)
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "user.PWHash");
            if (user.PWSalt.Length != 128)
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "user.PWSalt");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(user.PWSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != user.PWHash[i]) return false;
                }
            }

            return true;
        }

    }
}