using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using QuizProject.MVC.ViewModels;
using System.Threading.Tasks;
using QuizProject.MVC.Services;
using QuizProject.Data.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using System;
using QuizProject.Models.ViewModels;
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly SignInManager<ApplicationUser> _signInManager;
    public AccountController(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IHttpClientFactory httpClientFactory, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _emailSender = emailSender;
        _httpClientFactory = httpClientFactory;
        _signInManager = signInManager;
    }
    [HttpGet]
    public IActionResult Index() => View();
    [HttpGet]
    public IActionResult Login(string role, string returnUrl)
    {
        if (string.IsNullOrEmpty(role) && !string.IsNullOrEmpty(returnUrl))
        {
            if (returnUrl.Contains("Admin", StringComparison.OrdinalIgnoreCase))
                role = "Admin";
            else if (returnUrl.Contains("Teacher", StringComparison.OrdinalIgnoreCase))
                role = "Teacher";
            else if (returnUrl.Contains("Student", StringComparison.OrdinalIgnoreCase))
                role = "Student";
        }
        if (string.IsNullOrEmpty(role))
        {
            TempData["error"] = "Please select your role to continue.";
            return RedirectToAction("Index");
        }
        ViewData["Role"] = role;
        return View(new LoginVM { Role = role });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVM model)
    {
        ViewData["Role"] = model.Role;
        if (!ModelState.IsValid)
            return View(model);

        var client = _httpClientFactory.CreateClient();
        var loginData = new
        {
            Email = model.Email,
            Password = model.Password,
            Role = model.Role
        };
        var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://localhost:7097/api/Auth/login", content);

        if (!response.IsSuccessStatusCode)
        {
            TempData["error"] = "Invalid login credentials or email not confirmed.";
            return View(model);
        }

        var responseString = await response.Content.ReadAsStringAsync();
        Console.WriteLine("API Response: " + responseString);

        string token = null;
        string role = null;
        try
        {
            var jsonDoc = JsonDocument.Parse(responseString);
            token = jsonDoc.RootElement.GetProperty("token").GetString();
            role = jsonDoc.RootElement.GetProperty("role").GetString();
        }
        catch (Exception ex)
        {
            TempData["error"] = "Login response parsing error: " + ex.Message;
            return View(model);
        }

        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(role))
        {
            TempData["error"] = "Token veya rol bilgisi alınamadı.";
            return View(model);
        }

        Response.Cookies.Append("jwtToken", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(1)
        });

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            TempData["error"] = "Kullanıcı bulunamadı.";
            return View(model);
        }
        await _signInManager.SignInAsync(user, isPersistent: true);

        if (role == "Student")
        {
            return Redirect("/Student/Dashboard/Startquiz");
        }
        else
        {
            return RedirectToAction("Index", "Dashboard", new { area = role });
        }
    }
    [HttpGet]
    public IActionResult ForgotPassword(string role)
    {
        ViewData["Role"] = role ?? "Student";
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model, string role)
    {
        ViewData["Role"] = role ?? "Student";
        if (!ModelState.IsValid)
            return View(model);
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            TempData["success"] = "If the email is registered, a reset link has been sent.";
            return RedirectToAction("ForgotPassword", new { role });
        }
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetLink = Url.Action("ResetPassword", "Account", new { token, email = model.Email, role }, Request.Scheme);
        var emailBody = $@"
                <div style='font-family:Segoe UI, Tahoma, Geneva, Verdana, sans-serif; max-width:600px; margin:auto; padding:20px; color:#333;'>
                    <h2 style='color:#198754;'>QuizApp Password Reset Request</h2>
                    <p>Hello,</p>
                    <p>We received a request to reset your password for your QuizApp account associated with <strong>{model.Email}</strong>.</p>
                    <p>Click the button below to reset your password. This link will expire in 1 hour.</p>
                    <p style='text-align:center; margin:30px 0;'>
                        <a href='{resetLink}' style='background-color:#198754; color:#fff; padding:12px 25px; border-radius:6px; text-decoration:none; font-weight:bold; display:inline-block;'>Reset Password</a>
                    </p>
                    <p>If you did not request this, please ignore this email.</p>
                    <hr />
                    <p style='font-size:0.9rem; color:#666;'>Thank you,<br />QuizApp Team</p>
                </div>";

        await _emailSender.SendEmailAsync(model.Email, "QuizApp Password Reset", emailBody);

        TempData["success"] = "A reset link has been sent to your email.";
        return RedirectToAction("ForgotPassword", new { role });
    }
    [HttpGet]
    public IActionResult ResetPassword(string token, string email, string role)
    {
        if (token == null || email == null)
        {
            TempData["error"] = "Invalid token or email.";
            return RedirectToAction("ForgotPassword", new { role });
        }
        ViewData["Role"] = role ?? "Student";
        return View(new ResetPasswordViewModel
        {
            Token = token,
            Email = email,
            Role = role
        });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        ViewData["Role"] = model.Role ?? "Student";
        if (!ModelState.IsValid)
            return View(model);
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            TempData["success"] = "Password reset successful.";
            return RedirectToAction("Login", new { role = model.Role });
        }
        var resetPassResult = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
        if (!resetPassResult.Succeeded)
        {
            foreach (var error in resetPassResult.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }
        TempData["success"] = "Password has been reset successfully.";
        return RedirectToAction("Login", new { role = model.Role });
    }
    [HttpGet]
    public IActionResult Register(string role)
    {
        if (string.IsNullOrWhiteSpace(role) || !IsValidRole(role))
            return BadRequest("Invalid role.");
        ViewData["Role"] = role;
        return View(new RegisterViewModel { Role = role });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        ViewData["Role"] = model.Role;
        if (!IsValidRole(model.Role))
        {
            ModelState.AddModelError("Role", "Invalid role.");
            return View(model);
        }
        if (!ModelState.IsValid)
            return View(model);
        var client = _httpClientFactory.CreateClient();
        var registerData = new
        {
            model.FullName,
            model.Email,
            model.Password,
            model.BirthDate,
            model.ProfileImageUrl,
            model.Role
        };
        var content = new StringContent(JsonSerializer.Serialize(registerData), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://localhost:7097/api/Auth/register", content);
        if (!response.IsSuccessStatusCode)
        {
            TempData["error"] = "Registration failed. Please check your input.";
            return View(model);
        }
        TempData["success"] = "Registration successful! Please check your email to confirm.";
        return RedirectToAction("Login", new { role = model.Role });
    }
    private bool IsValidRole(string role) => role == "Admin" || role == "Teacher" || role == "Student";
}
