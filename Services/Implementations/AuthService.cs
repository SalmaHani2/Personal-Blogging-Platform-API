using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Personal_Blogging_Platform_API.DTOs.Auth;
using Personal_Blogging_Platform_API.Models;
using Personal_Blogging_Platform_API.Repositories.Interfaces;
using Personal_Blogging_Platform_API.Services.Interfaces;

namespace Personal_Blogging_Platform_API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthService(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public async Task<UserResponseDto> RegisterAsync(RegisterDto dto)
        {
            var existing = await _repo.GetByEmailAsync(dto.Email);
            if (existing != null) throw new ApplicationException("Email already in use");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = HashPasswordStatic(dto.Password),
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();

            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _repo.GetByEmailAsync(dto.Email);
            if (user == null) throw new ApplicationException("Invalid credentials");

            if (!VerifyPassword(dto.Password, user.PasswordHash)) throw new ApplicationException("Invalid credentials");

            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = _config.GetSection("Jwt").GetValue<string>("Secret") ?? "change_this_to_a_long_secret_in_production";
            var key = Encoding.ASCII.GetBytes(secret);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_config.GetSection("Jwt").GetValue<int>("ExpiresInMinutes", 60)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Simple PBKDF2 hashing
        public static string HashPasswordStatic(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[16];
            rng.GetBytes(salt);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(32);
            var result = new byte[49]; // 16 salt + 32 hash + 1 version
            Buffer.BlockCopy(salt, 0, result, 1, 16);
            Buffer.BlockCopy(hash, 0, result, 17, 32);
            result[0] = 0; // version
            return Convert.ToBase64String(result);
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            var bytes = Convert.FromBase64String(storedHash);
            if (bytes.Length != 49) return false;
            var salt = new byte[16];
            Buffer.BlockCopy(bytes, 1, salt, 0, 16);
            var hash = new byte[32];
            Buffer.BlockCopy(bytes, 17, hash, 0, 32);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            var computed = pbkdf2.GetBytes(32);
            return computed.SequenceEqual(hash);
        }
    }
}
