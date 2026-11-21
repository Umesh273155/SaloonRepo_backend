
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SalonSphere.Application.DTOs;
using SalonSphere.Application.Interfaces;
using SalonSphere.Core.Entities;
using SalonSphere.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SalonSphere.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly EmailOtpSettings _otpSettings;
        private readonly IEmailService _emailService;

        public AuthService(ApplicationDbContext context,IConfiguration config,IEmailService emailService,IOptions<EmailOtpSettings> otpSettings)
        {
            _context = context;
            _config = config;
            _emailService = emailService;
            _otpSettings = otpSettings.Value;
        }

        // ==========================================
        // REGISTER
        // ==========================================

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

            User user;
            Salon salon;

            // 3. Create Salon and User inside transaction
            await using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Create Salon
                    salon = new Salon
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

                    // Create Salon Owner
                    user = new User
                    {
                        SalonId = salon.SalonId,
                        RoleId = ownerRole.RoleId,
                        FullName = request.FullName.Trim(),
                        Email = email,
                        Phone = request.Phone.Trim(),
                        PasswordHash = HashPassword(request.Password),
                        IsActive = true,
                        IsEmailVerified = false,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    // Commit database transaction
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            // 4. Send OTP after database commit
            bool emailSent = await CreateAndSendOtpAsync(user);

            // 5. Registration response
            return new ApiResponse<RegisterResDto>
            {
                Success = true,

                Message = emailSent ? "Registration successful. OTP sent to your email."
                                     : "Registration successful, but OTP email could not be sent. Please resend OTP.",

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

        // ==========================================
        // LOGIN
        // ==========================================

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

            // 4. Check email verification
            if (!user.IsEmailVerified)
            {
                return new ApiResponse<LoginResDto>
                {
                    Success = false,
                    Message = "Please verify your email before logging in.",
                    Data = null
                };
            }

            // 5. Get user role
            var role = await _context.Roles
                .FirstOrDefaultAsync(x => x.RoleId == user.RoleId);

            if (role == null)
            {
                return new ApiResponse<LoginResDto>
                {
                    Success = false,
                    Message = "User role not found.",
                    Data = null
                };
            }

            // 6. Generate JWT
            var token = GenerateJwtToken(
                user.UserId,
                user.Email,
                role.RoleName);

            // 7. Return success response
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

        // ==========================================
        // CHANGE PASSWORD
        // ==========================================

        public async Task<ApiResponse<object?>> ChangePasswordAsync(int userId,ChangePasswordReqDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return new ApiResponse<object?>
                {
                    Success = false,
                    Message = "User not found.",
                    Data = null
                };
            }

            // Verify current password
            if (!VerifyPassword(request.CurrentPassword,user.PasswordHash))
            {
                return new ApiResponse<object?>
                {
                    Success = false,
                    Message = "Current password is incorrect.",
                    Data = null
                };
            }

            // Prevent using the same password
            if (request.CurrentPassword == request.NewPassword)
            {
                return new ApiResponse<object?>
                {
                    Success = false,
                    Message = "New password must be different from current password.",
                    Data = null
                };
            }

            // Hash and update new password
            user.PasswordHash = HashPassword(request.NewPassword);

            await _context.SaveChangesAsync();

            return new ApiResponse<object?>
            {
                Success = true,
                Message = "Password changed successfully.",
                Data = null
            };
        }

        // ==========================================
        // GENERATE AND SEND OTP
        // ==========================================

        private async Task<bool> CreateAndSendOtpAsync(User user)
        {
            try
            {
                // Mark previous active OTPs as used
                var previousOtps = await _context.EmailOtps
                    .Where(x => x.UserId == user.UserId && !x.IsUsed)
                    .ToListAsync();

                foreach (var previousOtp in previousOtps)
                {
                    previousOtp.IsUsed = true;
                }

                // Generate secure 6-digit OTP
                string otp = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

                var emailOtp = new EmailOtp
                {
                    UserId = user.UserId,

                    OtpHash = HashOtp(user.UserId,user.Email,otp),

                    ExpiresAt = DateTime.UtcNow.AddMinutes(_otpSettings.ExpiryMinutes),

                    Attempts = 0,
                    IsUsed = false,
                    CreatedAt = DateTime.UtcNow
                };

                _context.EmailOtps.Add(emailOtp);

                await _context.SaveChangesAsync();

                // Prepare email
                string safeName = WebUtility.HtmlEncode(user.FullName);

                string htmlMessage = $@"
                    <div style='font-family:Arial;padding:20px;'>
                        <h2>Welcome to SalonSphere!</h2>

                        <p>Hello {safeName},</p>

                        <p>Your email verification OTP is:</p>

                        <h1 style='letter-spacing:8px;'>{otp}</h1>

                        <p>This OTP is valid for
                        {_otpSettings.ExpiryMinutes} minutes.</p>

                        <p>Please do not share this OTP with anyone.</p>

                        <p>Thank you,<br/>SalonSphere Team</p>
                    </div>";

                // Send email
                await _emailService.SendEmailAsync(user.Email,"SalonSphere - Email Verification OTP",htmlMessage);

                return true;
            }
            catch
            {
                return false;
            }
        }

        // ==========================================
        // HASH OTP USING HMAC-SHA256
        // ==========================================

        private string HashOtp(int userId,string email,string otp)
        {
            if (string.IsNullOrWhiteSpace(_otpSettings.SecretKey))
            {
                throw new InvalidOperationException("OTP secret key is not configured.");
            }

            string input = $"{userId}:{email}:{otp}";

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_otpSettings.SecretKey));

            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToHexString(hash);
        }

        // ==========================================
        // VERIFY OTP HASH
        // ==========================================

        private bool VerifyOtpHash(int userId,string email,string otp,string storedHash)
        {
            try
            {
                byte[] expectedHash = Convert.FromHexString(storedHash);

                byte[] actualHash = Convert.FromHexString(HashOtp(userId, email, otp));

                return CryptographicOperations.FixedTimeEquals(actualHash,expectedHash);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        // ==========================================
        // VERIFY EMAIL OTP
        // ==========================================

        public async Task<ApiResponse<object?>> VerifyEmailAsync(VerifyEmailReqDto request)
        {
            string email = request.Email.Trim().ToLowerInvariant();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return new ApiResponse<object?>
                {
                    Success = false,
                    Message = "Invalid email or OTP.",
                    Data = null
                };
            }

            if (user.IsEmailVerified)
            {
                return new ApiResponse<object?>
                {
                    Success = true,
                    Message = "Email is already verified.",
                    Data = null
                };
            }

            // Get latest unused OTP
            var otpRecord = await _context.EmailOtps
                .Where(x => x.UserId == user.UserId && !x.IsUsed)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            if (otpRecord == null)
            {
                return new ApiResponse<object?>
                {
                    Success = false,
                    Message = "OTP not found. Please request a new OTP.",
                    Data = null
                };
            }

            // Check expiry
            if (otpRecord.ExpiresAt <= DateTime.UtcNow)
            {
                otpRecord.IsUsed = true;

                await _context.SaveChangesAsync();

                return new ApiResponse<object?>
                {
                    Success = false,
                    Message = "OTP has expired. Please request a new OTP.",
                    Data = null
                };
            }

            // Check maximum attempts
            if (otpRecord.Attempts >= _otpSettings.MaxAttempts)
            {
                otpRecord.IsUsed = true;

                await _context.SaveChangesAsync();

                return new ApiResponse<object?>
                {
                    Success = false,
                    Message = "Maximum OTP attempts exceeded. Please request a new OTP.",
                    Data = null
                };
            }

            // Increment attempts
            otpRecord.Attempts++;

            bool isValid = VerifyOtpHash(user.UserId,user.Email,request.Otp,otpRecord.OtpHash);

            if (!isValid)
            {
                if (otpRecord.Attempts >= _otpSettings.MaxAttempts)
                {
                    otpRecord.IsUsed = true;
                }

                await _context.SaveChangesAsync();

                return new ApiResponse<object?>
                {
                    Success = false,
                    Message = "Invalid OTP.",
                    Data = null
                };
            }

            // OTP verified successfully
            otpRecord.IsUsed = true;
            user.IsEmailVerified = true;

            await _context.SaveChangesAsync();

            return new ApiResponse<object?>
            {
                Success = true,
                Message = "Email verified successfully.",
                Data = null
            };
        }

        // ==========================================
        // RESEND OTP
        // ==========================================

        public async Task<ApiResponse<object?>> ResendOtpAsync(ResendOtpReqDto request)
        {
            string email = request.Email.Trim().ToLowerInvariant();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return new ApiResponse<object?>
                {
                    Success = false,
                    Message = "User not found.",
                    Data = null
                };
            }

            if (user.IsEmailVerified)
            {
                return new ApiResponse<object?>
                {
                    Success = false,
                    Message = "Email is already verified.",
                    Data = null
                };
            }

            // Get latest OTP
            var latestOtp = await _context.EmailOtps
                .Where(x => x.UserId == user.UserId)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            // Check resend cooldown
            if (latestOtp != null)
            {
                var elapsedSeconds = (DateTime.UtcNow - latestOtp.CreatedAt).TotalSeconds;

                if (elapsedSeconds < _otpSettings.ResendCooldownSeconds)
                {
                    int remainingSeconds = _otpSettings.ResendCooldownSeconds - (int)elapsedSeconds;

                    return new ApiResponse<object?>
                    {
                        Success = false,
                        Message = $"Please wait {remainingSeconds} seconds before requesting another OTP.",
                        Data = null
                    };
                }
            }

            // Generate and send new OTP
            bool emailSent = await CreateAndSendOtpAsync(user);

            return new ApiResponse<object?>
            {
                Success = emailSent,

                Message = emailSent  ? "A new OTP has been sent to your email." : "Unable to send OTP email. Please try again later.",
                Data = null
            };
        }

        // ==========================================
        // GENERATE JWT TOKEN
        // ==========================================

        private string GenerateJwtToken(int userId,string email,string role)
        {
            var jwtSection = _config.GetSection("Jwt");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));

            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var expiryMinutes = Convert.ToDouble(jwtSection["ExpiryInMinutes"]);

            var expires = DateTime.UtcNow.AddMinutes(expiryMinutes);

            var claims = new[]
            {
                new Claim( ClaimTypes.NameIdentifier,userId.ToString()),

                new Claim(ClaimTypes.Email,email),

                new Claim(ClaimTypes.Role,role)
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

        // ==========================================
        // VERIFY PASSWORD
        // ==========================================

        private bool VerifyPassword(string password,string storedHash)
        {
            try
            {
                var parts = storedHash.Split('$');

                if (parts.Length != 3 || parts[0] != "PBKDF2")
                {
                    return false;
                }

                byte[] salt = Convert.FromBase64String(parts[1]);

                byte[] expectedHash = Convert.FromBase64String(parts[2]);

                byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(password,salt,600000,HashAlgorithmName.SHA256,32);

                return CryptographicOperations.FixedTimeEquals(actualHash,expectedHash);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        // ==========================================
        // HASH PASSWORD USING PBKDF2
        // ==========================================

        private static string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password,salt,600000,HashAlgorithmName.SHA256,32);

            return $"PBKDF2${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }
    }
}
