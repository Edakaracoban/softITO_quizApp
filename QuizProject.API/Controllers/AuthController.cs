using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuizProject.API.Dtos;
using QuizProject.API.Services;
using QuizProject.API.Settings;
using QuizProject.Data.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using QuizProject.Data;
using Microsoft.EntityFrameworkCore;

namespace QuizProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly JwtSettings _jwtSettings;
        public AuthController(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IOptions<JwtSettings> jwtSettings, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _jwtSettings = jwtSettings.Value;
            _appDbContext = appDbContext;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model) //yapıldı çalışıyor
        {
            var validRoles = new[] { Roles.Admin, Roles.Teacher, Roles.Student };
            if (!validRoles.Contains(model.Role))
                return BadRequest("Invalid role specified. Allowed roles are Admin, Teacher, Student.");
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                BirthDate = model.BirthDate,
                ProfileImageUrl = model.ProfileImageUrl
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            await _userManager.AddToRoleAsync(user, model.Role);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Base64UrlEncode(token);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth",
                new { userId = user.Id, token = encodedToken }, Request.Scheme);
            await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                $"Please confirm your account by clicking this link: {confirmationLink}");
            return Ok("Registration successful. Please check your email to confirm your account.");
        }
        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token) //yapıldı çalışıyor
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest("UserId and token are required.");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest("User not found.");
            var decodedToken = Base64UrlDecode(token);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest("Email confirmation failed: " + errors);
            }
            return Ok("Email confirmed successfully!");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model) //yapıldı çalışıyor
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized("Invalid credentials.");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return Unauthorized("Email not confirmed.");

            if (await _userManager.IsLockedOutAsync(user))
                return Unauthorized("This account is disabled.");

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized("Invalid credentials.");

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains(model.Role))
                return Unauthorized($"User does not have the role {model.Role}.");

            var token = await GenerateJwtToken(user);

            return Ok(new { token, role = model.Role });
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user) //yapıldı çalışıyor
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            if (!string.IsNullOrEmpty(role))
                claims.Add(new Claim(ClaimTypes.Role, role));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private static string Base64UrlEncode(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }
        private static string Base64UrlDecode(string input)
        {
            string base64 = input
                .Replace('-', '+')
                .Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            var bytes = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(bytes);
        }
        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto model) //yapıldı çalışıyor
        {
            if (string.IsNullOrEmpty(model.Email))
                return BadRequest("Email is required.");
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return Ok("If the email is registered and confirmed, a reset link has been sent.");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Base64UrlEncode(token);
            var resetLink = Url.Action(nameof(ResetPassword), "Auth",
                new { email = user.Email, token = encodedToken }, Request.Scheme);
            await _emailSender.SendEmailAsync(user.Email, "Reset your password",
                $"Please reset your password by clicking here: {resetLink}");

            return Ok("If the email is registered and confirmed, a reset link has been sent.");
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model) //yapıldı çalışıyor
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("User not found.");
            var decodedToken = Base64UrlDecode(model.Token);
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest("Password reset failed: " + errors);
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault();
            var token = await GenerateJwtToken(user);
            return Ok(new { token, role = userRole });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return NotFound("User not found.");

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                    return BadRequest(string.Join(", ", result.Errors.Select(e => e.Description)));

                return Ok("User deleted successfully.");
            }
            catch (DbUpdateException)
            {
                // Foreign key hatası olabilir
                return StatusCode(500, "This user cannot be deleted because they have related data (e.g., quiz results, enrollments).");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error while deleting user: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto model)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            var currentUserRoles = await _userManager.GetRolesAsync(currentUser);
            if (!currentUserRoles.Contains(Roles.Admin) && currentUserId != id)
            {
                return Unauthorized("You can only update your own profile.");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");
            user.FullName = model.FullName;
            user.BirthDate = model.BirthDate ?? user.BirthDate;
            user.ProfileImageUrl = model.ProfileImageUrl;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok("User updated successfully.");
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(string id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            var currentUserRoles = await _userManager.GetRolesAsync(currentUser);
            if (!currentUserRoles.Contains(Roles.Admin) && currentUserId != id)
            {
                return Unauthorized("You can only access your own data.");
            }
            var user = await _appDbContext.Users
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.Id,
                    u.FullName,
                    u.Email,
                    u.UserName,
                    u.BirthDate,
                    u.ProfileImageUrl,
                    u.PhoneNumber,
                    u.PhoneNumberConfirmed,
                    u.LockoutEnd,
                    u.LockoutEnabled,
                    Role = _userManager.GetRolesAsync(u).Result.FirstOrDefault(),
                    QuizComments = u.QuizComments.Select(qc => new
                    {
                        qc.Id,
                        qc.CommentText,
                        qc.CreatedAt,
                        qc.QuizId,
                        UserFullName = qc.UserFullName
                    }).ToList()
                })
                .FirstOrDefaultAsync();
            if (user == null)
                return NotFound();
            var role = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(id));
            var userRole = role.FirstOrDefault();
            var result = new
            {
                user.Id,
                user.FullName,
                user.Email,
                user.UserName,
                user.BirthDate,
                user.ProfileImageUrl,
                user.PhoneNumber,
                user.PhoneNumberConfirmed,
                user.LockoutEnd,
                user.LockoutEnabled,
                Role = userRole,
                user.QuizComments
            };
            return Ok(result);
        }
       
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            var userList = new List<object>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userRole = roles.FirstOrDefault();
                bool isLocked = user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow;
                userList.Add(new
                {
                    user.Id,
                    user.FullName,
                    user.Email,
                    user.UserName,
                    user.BirthDate,
                    user.PhoneNumber,
                    user.ProfileImageUrl,
                    Role = userRole,
                    IsLocked = isLocked // 🔥 yeni alan burada
                });
            }
            return Ok(userList);
        }
        [HttpPut("{id}/profile")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserProfile(string id, [FromBody] UpdateUserProfileDto model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");
            user.FullName = model.FullName;
            user.BirthDate = model.BirthDate ?? user.BirthDate;
            if (!string.IsNullOrEmpty(model.ProfileImageBase64))
            {
                var uploadsFolder = Path.Combine("wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);
                var imageBytes = Convert.FromBase64String(model.ProfileImageBase64);
                var fileName = $"{Guid.NewGuid()}.jpg";
                var filePath = Path.Combine(uploadsFolder, fileName);
                await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);
                user.ProfileImageUrl = $"/uploads/{fileName}";
            }
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok("User profile updated successfully.");
        }
        [HttpPut("{id}/lockstatus")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserLockStatus(string id, [FromBody] UpdateUserLockStatusDto model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");
            if (model.IsLocked)
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTimeOffset.MaxValue;
            }
            else
            {
                user.LockoutEnd = null;
                user.LockoutEnabled = false;
            }
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest("Failed to update user lock status.");
            return Ok(model.IsLocked ? "User locked (passive)." : "User unlocked (active).");
        }
        [HttpPost("createuser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminCreateUser([FromBody] AdminCreateUserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var validRoles = new[] { Roles.Admin, Roles.Teacher, Roles.Student };
            if (!validRoles.Contains(model.Role))
                return BadRequest("Invalid role specified. Allowed roles are Admin, Teacher, Student.");

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
                return BadRequest("User with this email already exists.");

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                BirthDate = model.BirthDate ?? DateTime.MinValue,
                PhoneNumber = model.PhoneNumber,
                EmailConfirmed = true, // Admin oluşturursa direkt onaylı
                ProfileImageUrl = "/img/profile/default.png" // Varsayılan profil resmi
            };

            if (!string.IsNullOrEmpty(model.ProfileImageBase64))
            {
                try
                {
                    var uploadsFolder = Path.Combine("wwwroot", "img", "profile");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    string base64Data = model.ProfileImageBase64;
                    string extension = ".jpg"; // default

                    if (base64Data.StartsWith("data:image/png"))
                        extension = ".png";
                    else if (base64Data.StartsWith("data:image/jpeg"))
                        extension = ".jpg";
                    else if (base64Data.StartsWith("data:image/webp"))
                        extension = ".webp";
                    else if (base64Data.StartsWith("data:image/jpg"))
                        extension = ".jpg";

                    // "data:image/png;base64," kısmını temizle
                    var base64String = base64Data.Contains(",") ? base64Data.Split(',')[1] : base64Data;
                    var imageBytes = Convert.FromBase64String(base64String);

                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);
                    user.ProfileImageUrl = $"/img/profile/{fileName}";
                }
                catch (FormatException)
                {
                    return BadRequest("Invalid profile image format.");
                }
            }

            var createResult = await _userManager.CreateAsync(user, model.Password);
            if (!createResult.Succeeded)
            {
                var errors = createResult.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }

            var roleResult = await _userManager.AddToRoleAsync(user, model.Role);
            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }

            return Ok("User created successfully by admin.");
        }
    }
}




