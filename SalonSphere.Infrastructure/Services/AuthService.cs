
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SalonSphere.Application.DTOs;
using SalonSphere.Application.Interfaces;
using SalonSphere.Core.Entities;
using SalonSphere.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SalonSphere.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        public AuthService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<ApiResponse<RegisterResDto>> RegisterAsync(RegisterReqDto request)
        {
            string email = request.Email.Trim().ToLowerInvariant();

            // 1. Check duplicate email
            bool emailExists = await _context.Users.AnyAsync(x => x.Email == email);

            if (emailExists)
            {
                return new ApiResponse<RegisterResDto>
                {
                    Success = false,
                    Message = "This email is already registered.",
                    Data = null
                };
            }

            // 2. Verify SalonOwner role
            var ownerRole = await _context.Roles.FirstOrDefaultAsync(x => x.RoleName == "SalonOwner");

            if (ownerRole == null)
            {
                return new ApiResponse<RegisterResDto>
                {
                    Success = false,
                    Message = "SalonOwner role not found.",
                    Data = null
                };
            }

            // 3. Begin transaction
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 4. Create Salon
                var salon = new Salon
                {
                    SalonName = request.SalonName.Trim(),
                    Phone = request.Phone.Trim(),
                    Email = email,
                    Address = request.Address,
                    City = request.City,
                    State = request.State,
                    Pincode = request.Pincode,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Salons.Add(salon);
                await _context.SaveChangesAsync();

                // 5. Create Salon Owner
                var user = new User
                {
                    SalonId = salon.SalonId,
                    RoleId = ownerRole.RoleId,
                    FullName = request.FullName.Trim(),
                    Email = email,
                    Phone = request.Phone.Trim(),
                    PasswordHash = HashPassword(request.Password),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // 6. Commit transaction
                await transaction.CommitAsync();

                // 7. Return success response
                return new ApiResponse<RegisterResDto>
                {
                    Success = true,
                    Message = "Registration successful.",
                    Data = new RegisterResDto
                    {
                        UserId = user.UserId,
                        SalonId = salon.SalonId,
                        FullName = user.FullName,
                        Email = user.Email,
                        SalonName = salon.SalonName
                    }
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }



        public async Task<ApiResponse<LoginResDto>> LoginAsync(LoginReqDto request)
        {
            string email = request.Email.Trim().ToLowerInvariant();

            // 1. Find user
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return new ApiResponse<LoginResDto>
                {
                    Success = false,
                    Message = "Invalid email or password.",
                    Data = null
                };
            }

            // 2. Check account status
            if (!user.IsActive)
            {
                return new ApiResponse<LoginResDto>
                {
                    Success = false,
                    Message = "Your account is inactive.",
                    Data = null
                };
            }

            // 3. Verify password
            if (!VerifyPassword(request.Password, user.PasswordHash))
            {
                return new ApiResponse<LoginResDto>
                {
                    Success = false,
                    Message = "Invalid email or password.",
                    Data = null
                };
            }

            // 4. Get user role
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.RoleId == user.RoleId);

            if (role == null)
            {
                return new ApiResponse<LoginResDto>
                {
                    Success = false,
                    Message = "User role not found.",
                    Data = null
                };
            }

            // 5. Generate JWT
            var token = GenerateJwtToken(user.UserId,user.Email,role.RoleName);

            // 6. Return success response
            return new ApiResponse<LoginResDto>
            {
                Success = true,
                Message = "Login successful.",
                Data = new LoginResDto
                {
                    UserId = user.UserId,
                    SalonId = user.SalonId,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = role.RoleName,
                    AccessToken = token,
                    TokenType = "Bearer",
                    ExpiresAt = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiryInMinutes"]))
                }
            };
        }



        private string GenerateJwtToken(int userId,string email,string role)
        {
            var jwtSection = _config.GetSection("Jwt");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));

            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var expiryMinutes = Convert.ToDouble(jwtSection["ExpiryInMinutes"]);

            var expires = DateTime.UtcNow.AddMinutes(expiryMinutes);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        private bool VerifyPassword(string password, string storedHash)
        {
            try
            {
                var parts = storedHash.Split('$');

                if (parts.Length != 3 || parts[0] != "PBKDF2")
                    return false;

                byte[] salt = Convert.FromBase64String(parts[1]);
                byte[] expectedHash = Convert.FromBase64String(parts[2]);

                byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 600000, HashAlgorithmName.SHA256, 32);

                return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        // Password Hashing using PBKDF2
        private static string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 600000, HashAlgorithmName.SHA256, 32);

            return $"PBKDF2${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }
    }
}
