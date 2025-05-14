using Spestqnko.Core.Models;
using Spestqnko.Core.Repositories;
using Spestqnko.Core.Services;
using Spestqnko.Service.Exceptions;
using System.Net;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Spestqnko.Service
{
    public class UserService : BaseService<User>, IUserService
    {
        public UserService(IRepositoryManager repositoryManager)
            : base(repositoryManager)
        {
        }

        public async Task<User> AddUserAsync(string username, string password)
        {
            // Validate all inputs for registration
            ValidateForRegistration(username, password);

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User()
            {
                UserName = username,
                PWHash = passwordHash,
                PWSalt = passwordSalt
            };

            await _repositoryManager.Users.AddAsync(user);
            await _repositoryManager.SaveChangesAsync();

            return user;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            // Validate credentials with minimal checks for authentication
            ValidateForAuthentication(username, password);

            var user = _repositoryManager.Users.Find(x => x.UserName == username).SingleOrDefault();

            // Check if username exists
            if (user == null || !VerifyPasswordHash(password, user))
                throw new AggregateAppException(HttpStatusCode.Unauthorized, "Invalid credentials");

            // Authentication successful
            return await Task.FromResult(user);
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) 
                throw new ArgumentNullException("password");
                
            if (string.IsNullOrWhiteSpace(password)) 
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

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
                
                // Constant-time comparison to prevent timing attacks
                int difference = 0;
                for (int i = 0; i < computedHash.Length; i++)
                {
                    // XOR the bytes and OR the result with the running difference
                    difference |= computedHash[i] ^ user.PWHash[i];
                }
                
                // If difference is 0, all bytes matched
                return difference == 0;
            }
        }

        /// <summary>
        /// Validates user input data for registration and returns a list of validation errors
        /// </summary>
        /// <param name="username">The username to validate</param>
        /// <param name="password">The password to validate</param>
        /// <returns>A list of validation error messages</returns>
        private void ValidateForRegistration(string username, string password)
        {
            var errors = new List<string>();

            // Validate username
            if (string.IsNullOrWhiteSpace(username))
            {
                errors.Add("Username is required");
            }
            else 
            {
                if (username.Length < 3)
                {
                    errors.Add("Username must be at least 3 characters long");
                }
                else if (username.Length > 50)
                {
                    errors.Add("Username cannot exceed 50 characters");
                }

                if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_\-\.]+$"))
                {
                    errors.Add("Username can only contain letters, numbers, underscores, hyphens, and periods");
                }
            }

            // Validate password
            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add("Password is required");

                throw new AggregateAppException(HttpStatusCode.BadRequest, errors);
            }

            // Check minimum length
            if (password.Length < 8)
                errors.Add("Password must be at least 8 characters long");

            // Check for uppercase letter
            if (!Regex.IsMatch(password, @"[A-Z]"))
                errors.Add("Password must contain at least one uppercase letter");

            // Check for lowercase letter
            if (!Regex.IsMatch(password, @"[a-z]"))
                errors.Add("Password must contain at least one lowercase letter");

            // Check for digit
            if (!Regex.IsMatch(password, @"\d"))
                errors.Add("Password must contain at least one digit");

            // Check for special character
            if (!Regex.IsMatch(password, @"[^\da-zA-Z]"))
                errors.Add("Password must contain at least one special character");

            if (_repositoryManager.Users.Find(x => x.UserName == username).Any())
                errors.Add($"Username {username} is already taken");

            if (errors.Count > 0)
                throw new AggregateAppException(HttpStatusCode.BadRequest, errors);
        }

        /// <summary>
        /// Validates user input data for authentication and returns a list of validation errors
        /// Authentication only checks for null or empty values
        /// </summary>
        /// <param name="username">The username to validate</param>
        /// <param name="password">The password to validate</param>
        /// <returns>A list of validation error messages</returns>
        private void ValidateForAuthentication(string username, string password)
        {
            var errors = new List<string>();

            // For authentication, we only check if credentials are provided
            if (string.IsNullOrWhiteSpace(username))
            {
                errors.Add("Username is required");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add("Password is required");
            }

            if (errors.Count > 0)
                throw new AggregateAppException(HttpStatusCode.BadRequest, errors);
        }
    }
}