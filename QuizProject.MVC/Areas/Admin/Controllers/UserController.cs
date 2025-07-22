using Microsoft.AspNetCore.Mvc;
using QuizProject.MVC.ViewModels;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
namespace QuizProject.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {

        private readonly ILogger<UserController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(ILogger<UserController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult CreateUser() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(AdminCreateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                string base64Image = null;

                if (model.ProfileImage != null && model.ProfileImage.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await model.ProfileImage.CopyToAsync(ms);
                    var imageBytes = ms.ToArray();
                    base64Image = Convert.ToBase64String(imageBytes);
                }

                var client = CreateAuthorizedClient();

                var createDto = new
                {
                    model.Email,
                    model.FullName,
                    model.UserName,
                    model.PhoneNumber,
                    model.BirthDate,
                    model.Role,
                    model.Password,
                    ProfileImageBase64 = base64Image,
                    model.EmailConfirmed
                };

                var content = new StringContent(JsonSerializer.Serialize(createDto), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("api/auth/createuser", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorJson = await response.Content.ReadAsStringAsync();
                    string errorMsg;

                    try
                    {
                        var errorObj = JsonSerializer.Deserialize<ErrorResponse>(errorJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        errorMsg = errorObj?.Errors != null ? string.Join("; ", errorObj.Errors) : errorJson;
                    }
                    catch
                    {
                        errorMsg = errorJson;
                    }

                    TempData["error"] = $"User creation failed: {errorMsg}";
                    return View(model);
                }

                TempData["success"] = "User created successfully.";
                return RedirectToAction("GetAllUsers");
            }
            catch (UnauthorizedAccessException uaEx)
            {
                _logger.LogWarning(uaEx, "Unauthorized access in CreateUser.");
                return Forbid();
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Http request failed in CreateUser.");
                TempData["error"] = "API request failed.";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in CreateUser.");
                TempData["error"] = "An error occurred.";
                return View(model);
            }
        }


        private HttpClient CreateAuthorizedClient()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://your-api-base-url.com/");

            // Token alma ve header ekleme kısmı (örnek):
            var token = HttpContext.Session.GetString("AccessToken");
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return client;
        }

        private class ErrorResponse
        {
            public List<string> Errors { get; set; }
        }
    }
}