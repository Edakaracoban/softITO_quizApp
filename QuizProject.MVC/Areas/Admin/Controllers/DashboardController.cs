using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuizProject.API.Dtos;
using QuizProject.Data.Models;
using QuizProject.Data.Repositories.Abstract;
using QuizProject.MVC.ViewModels;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace QuizProject.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DashboardController> _logger;
        private const string ApiBaseUrl = "https://localhost:7097/api/Auth/";
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(
            IHttpClientFactory httpClientFactory,
            IUnitOfWork unitOfWork,
            ILogger<DashboardController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _httpClientFactory = httpClientFactory;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userManager = userManager;
        }

        private string GetTokenFromCookie() => Request.Cookies["jwtToken"];

        private HttpClient CreateAuthorizedClient()
        {
            var token = GetTokenFromCookie();
            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("JWT token not found in cookies.");

            var client = _httpClientFactory.CreateClient("API");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        private IActionResult HandleUnauthorized(string message = "Unauthorized access.")
        {
            TempData["error"] = message;
            return RedirectToAction("Login", "Account", new { area = "", role = "Admin" });
        }

        private UserViewModel MapUserFromJson(string jsonString)
        {
            try
            {
                return JsonSerializer.Deserialize<UserViewModel>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "User JSON deserialization failed.");
                return null;
            }
        }

        public async Task<IActionResult> UserDetails(string id)
        {
            try
            {
                var client = CreateAuthorizedClient();
                var response = await client.GetAsync(id);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("UserDetails API call failed with status: {StatusCode}", response.StatusCode);
                    return NotFound();
                }

                var json = await response.Content.ReadAsStringAsync();
                var user = MapUserFromJson(json);
                if (user == null)
                    return NotFound();

                return View(user);
            }
            catch (UnauthorizedAccessException uaEx)
            {
                _logger.LogWarning(uaEx, "Unauthorized access in UserDetails.");
                return HandleUnauthorized();
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Http request failed in UserDetails.");
                return View("Error", "API request failed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in UserDetails.");
                return View("Error", "An error occurred.");
            }
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _unitOfWork.Categories.GetAllAsync();
                var quizzes = await _unitOfWork.Quizzes.GetAllAsync();
                var questions = await _unitOfWork.Questions.GetAllAsync();
                var results = await _unitOfWork.UserQuizResults.GetAllAsync();

                int totalUsers = 0;
                try
                {
                    totalUsers = await _userManager.Users.CountAsync(); // ASENKRON
                }
                catch (Exception apiEx)
                {
                    _logger.LogWarning(apiEx, "Could not retrieve user count from UserManager. Using default value 0.");
                }

                var popularCategoriesEntities = await _unitOfWork.Categories.GetPopularCategoriesAsync(5);
                var popularCategories = popularCategoriesEntities.Select(c => new CategorySummaryViewModel
                {
                    CategoryId = c.Id,
                    CategoryName = c.Name,
                    QuizCount = c.Quizzes.Count,
                    TotalQuizAttempts = c.Quizzes.Sum(q => q.UserQuizResults.Count),
                    TotalQuestions = c.Quizzes.SelectMany(q => q.Questions).Count(),
                    RecentQuestions = c.Quizzes.SelectMany(q => q.Questions)
                                               .OrderByDescending(q => q.Id)
                                               .Take(3)
                                               .ToList()
                }).ToList();

                var recentQuestions = questions.OrderByDescending(q => q.Id)
                                               .Take(10)
                                               .Select(q => new QuestionViewModel
                                               {
                                                   Id = q.Id,
                                                   QuestionText = q.Text,
                                                   QuizName = q.Quiz?.Title ?? "Unknown Quiz"
                                               }).ToList();

                var allQuestions = questions.Select(q => new QuestionViewModel
                {
                    Id = q.Id,
                    QuestionText = q.Text,
                    QuizName = q.Quiz?.Title ?? "Unknown Quiz"
                }).ToList();

                var testTypes = (await _unitOfWork.TestTypes.GetTestTypesWithQuizzesAsync())
                    .Select(tt => new TestTypeWithQuizzesViewModel
                    {
                        Id = tt.Id,
                        Name = tt.Name,
                        Quizzes = tt.Quizzes.Select(q => new QuizViewModel
                        {
                            Id = q.Id,
                            Title = q.Title
                        }).ToList()
                    }).ToList();

                var dashboardVM = new DashboardViewModel
                {
                    TotalCategories = categories.Count,
                    TotalQuizzes = quizzes.Count,
                    TotalQuestions = questions.Count,
                    TotalUsers = totalUsers,
                    TotalUserQuizResults = results.Count,
                    PopularCategories = popularCategories,
                    RecentQuestions = recentQuestions,
                    AllQuestions = allQuestions,
                    TestTypesWithQuizzes = testTypes
                };

                return View(dashboardVM);
            }
            catch (UnauthorizedAccessException uaEx)
            {
                _logger.LogWarning(uaEx, "Unauthorized access in Index.");
                return HandleUnauthorized();
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HTTP request failed in Index.");
                return View("Error", "API request failed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in Index.");
                return View("Error", "An unexpected error occurred.");
            }
        }
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var client = CreateAuthorizedClient();
                var response = await client.GetAsync("api/auth/all");

                if (!response.IsSuccessStatusCode)
                {
                    TempData["error"] = "Failed to fetch users.";
                    return HandleUnauthorized();
                }

                var jsonList = JsonDocument.Parse(await response.Content.ReadAsStringAsync())
                                           .RootElement.EnumerateArray();

                var users = jsonList.Select(jsonElement =>
                    JsonSerializer.Deserialize<UserViewModel>(jsonElement.GetRawText(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }))
                    .ToList();

                return View(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllUsers.");
                return View("Error", "An error occurred.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser([FromForm] string id)
        {
            if (string.IsNullOrEmpty(id))
                return Json(new { success = false, message = "User ID is required." });

            using var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:7097/");
            var response = await client.DeleteAsync($"api/Auth/{id}");

            if (response.IsSuccessStatusCode)
                return Json(new { success = true, message = "User deleted successfully." });

            var error = await response.Content.ReadAsStringAsync();
            if (error.Contains("related data"))
                return Json(new
                {
                    success = false,
                    message = "This user cannot be deleted because they have related data (e.g., quiz results, enrollments)."
                });

            return Json(new { success = false, message = "Failed to delete user: " + error });
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            try
            {
                var client = CreateAuthorizedClient();
                var response = await client.GetAsync(id);

                if (!response.IsSuccessStatusCode)
                    return NotFound();

                var json = await response.Content.ReadAsStringAsync();
                var vm = MapUserFromJson(json);
                return vm == null ? NotFound() : View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in EditUser.");
                return View("Error", "An error occurred.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            try
            {
                var client = CreateAuthorizedClient();
                var content = new StringContent(JsonSerializer.Serialize(new
                {
                    model.FullName,
                    model.BirthDate,
                    model.ProfileImageUrl
                }), Encoding.UTF8, "application/json");

                var response = await client.PutAsync(model.Id, content);
                if (!response.IsSuccessStatusCode)
                {
                    TempData["error"] = "User update failed.";
                    return View(model);
                }

                TempData["success"] = "User updated successfully.";
                return RedirectToAction("GetAllUsers");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in EditUser POST.");
                TempData["error"] = "An error occurred.";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserProfile(string id, UpdateUserProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            try
            {
                var client = CreateAuthorizedClient();
                var content = new StringContent(JsonSerializer.Serialize(new
                {
                    model.FullName,
                    model.BirthDate,
                    model.ProfileImageBase64
                }), Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{id}/profile", content);
                if (!response.IsSuccessStatusCode)
                {
                    TempData["error"] = "User profile update failed.";
                    return View(model);
                }

                TempData["success"] = "User profile updated successfully.";
                return RedirectToAction("GetAllUsers");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateUserProfile.");
                TempData["error"] = "An error occurred.";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserLockStatus(string id, UpdateUserLockStatusViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var client = CreateAuthorizedClient();
                var content = new StringContent(JsonSerializer.Serialize(new { model.IsLocked }), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"{id}/lockstatus", content);

                TempData[response.IsSuccessStatusCode ? "success" : "error"] =
                    response.IsSuccessStatusCode
                        ? (model.IsLocked ? "User locked (passive)." : "User unlocked (active).")
                        : "User lock status update failed.";

                return RedirectToAction("GetAllUsers");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateUserLockStatus.");
                TempData["error"] = "An error occurred.";
                return RedirectToAction("GetAllUsers");
            }
        }

        [HttpGet]
        public async Task<IActionResult> LockedUsers()
        {
            try
            {
                var client = CreateAuthorizedClient();
                var response = await client.GetAsync("all");

                if (!response.IsSuccessStatusCode)
                {
                    TempData["error"] = "Failed to fetch locked users.";
                    return RedirectToAction("GetAllUsers");
                }

                var jsonList = JsonDocument.Parse(await response.Content.ReadAsStringAsync())
                                           .RootElement.EnumerateArray();

                var lockedUsers = jsonList
                    .Where(item => item.TryGetProperty("isLocked", out var lockedProp) && lockedProp.GetBoolean())
                    .Select(item => JsonSerializer.Deserialize<UserViewModel>(item.GetRawText(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }))
                    .ToList();

                return View(lockedUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in LockedUsers.");
                TempData["error"] = "An error occurred.";
                return RedirectToAction("GetAllUsers");
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new AdminCreateUserDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminCreateUserDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var client = CreateAuthorizedClient(); // JWT Token ile HttpClient
                model.EmailConfirmed = true; // Admin tarafından eklenen kullanıcı e-posta onaylı olacak

                var json = JsonSerializer.Serialize(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{ApiBaseUrl}createuser", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "User successfully created.";
                    return RedirectToAction(nameof(GetAllUsers));
                }

                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Failed to create user: {error}");
                return View(model);
            }
            catch (UnauthorizedAccessException uaEx)
            {
                _logger.LogWarning(uaEx, "Unauthorized access in Create.");
                return HandleUnauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Create User.");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the user.");
                return View(model);
            }
        }
    }
}
