using Microsoft.EntityFrameworkCore;
using Spestqnko.Core.Models;
using Spestqnko.Core.Repositories;
using Spestqnko.Core.Services;
using Spestqnko.Service.Exceptions;
using System.Net;
using System.Security.Cryptography;

namespace Spestqnko.Service
{
    public class UserService : BaseModelService<User>, IUserService
    {
        public UserService(IUserRepository userRepository, DbContext dbContext)
            : base(userRepository, dbContext)
        {
        }

        public async Task<User> AddUserAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException(HttpStatusCode.BadRequest, "Password is required");

            if (_repository.Find(x => x.UserName == username).Any())
                throw new AppException(HttpStatusCode.BadRequest, $"Username {username} is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            var user = new User()
            {
                UserName = username,
                PWHash = passwordHash,
                PWSalt = passwordSalt
            };

            await _repository.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new AppException(HttpStatusCode.BadRequest, "Missing Password or Username.");

            var user = _repository.Find(x => x.UserName == username).SingleOrDefault();

            // check if username exists
            if (user == null || !VerifyPasswordHash(password, user))
                throw new AppException("Invalid credentials");

            // authentication successful
            return await Task.FromResult(user);
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new HMACSHA512())
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

            using (var hmac = new HMACSHA512(user.PWSalt))
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