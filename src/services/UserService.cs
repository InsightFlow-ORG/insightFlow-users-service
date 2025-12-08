using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using userService.src.models;
using userService.src.dtos;

namespace userService.src.services
{
    public interface IUserService
    {
        AppUser CreateUser(CreateUserRequest request);
        AppUser GetUserById(Guid id);
        List<UserResponse> GetAllUsers();
        UserResponse GetUserResponseById(Guid id);
        AppUser UpdateUser(Guid id, UpdateUserRequest request);
        bool DeleteUser(Guid id);
        bool UserExists(Guid id);
        bool EmailExists(string email);
        bool UserNameExists(string userName);
    }

    public class UserService : IUserService
    {
        private static List<AppUser> _users = new List<AppUser>();

        /// <summary>
        /// Creates a new user with validation
        /// </summary>
        public AppUser CreateUser(CreateUserRequest request)
        {
            // Validar email único
            if (EmailExists(request.Email))
                throw new InvalidOperationException("El correo electrónico ya está registrado.");

            // Validar username único
            if (UserNameExists(request.UserName))
                throw new InvalidOperationException("El nombre de usuario ya existe.");

            // Validar campos requeridos
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.LastName))
                throw new InvalidOperationException("Nombre y apellido son requeridos.");

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                throw new InvalidOperationException("Email y contraseña son requeridos.");

            var user = new AppUser
            {
                Id = Guid.NewGuid(), // UUID V4
                Name = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                DateOfBirth = request.DateOfBirth,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = HashPassword(request.Password),
                IsActive = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _users.Add(user);
            return user;
        }

        /// <summary>
        /// Gets all active users (non-sensitive information)
        /// </summary>
        public List<UserResponse> GetAllUsers()
        {
            return _users
                .Where(u => u.IsActive == 1)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Name = u.Name,
                    LastName = u.LastName,
                    Email = u.Email,
                    UserName = u.UserName,
                    DateOfBirth = u.DateOfBirth,
                    Address = u.Address,
                    IsActive = u.IsActive
                })
                .ToList();
        }

        /// <summary>
        /// Gets a specific user by ID
        /// </summary>
        public AppUser GetUserById(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");
            return user;
        }

        /// <summary>
        /// Gets user response by ID (non-sensitive information)
        /// </summary>
        public UserResponse GetUserResponseById(Guid id)
        {
            var user = GetUserById(id);

            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                DateOfBirth = user.DateOfBirth,
                Address = user.Address,
                IsActive = user.IsActive
            };
        }

        /// <summary>
        /// Updates user information (only name and username can be edited)
        /// </summary>
        public AppUser UpdateUser(Guid id, UpdateUserRequest request)
        {
            var user = GetUserById(id);

            // Validar que el nuevo username sea único (si es diferente)
            if (!user.UserName.Equals(request.UserName) && UserNameExists(request.UserName))
                throw new InvalidOperationException("El nombre de usuario ya existe.");

            user.Name = request.Name;
            user.LastName = request.LastName;
            user.UserName = request.UserName;
            user.UpdatedAt = DateTime.UtcNow;

            return user;
        }

        /// <summary>
        /// Soft delete - marks user as inactive
        /// </summary>
        public bool DeleteUser(Guid id)
        {
            var user = GetUserById(id);
            user.IsActive = 0;
            user.UpdatedAt = DateTime.UtcNow;
            return true;
        }

        /// <summary>
        /// Checks if user exists
        /// </summary>
        public bool UserExists(Guid id)
        {
            return _users.Any(u => u.Id == id);
        }

        /// <summary>
        /// Checks if email is already registered
        /// </summary>
        public bool EmailExists(string email)
        {
            return _users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Checks if username is already taken
        /// </summary>
        public bool UserNameExists(string userName)
        {
            return _users.Any(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Hashes password using SHA256
        /// </summary>
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}