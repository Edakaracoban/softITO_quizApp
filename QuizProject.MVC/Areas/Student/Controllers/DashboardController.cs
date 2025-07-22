using Microsoft.AspNetCore.Mvc;
using QuizProject.Business.Interfaces;
using QuizProject.Data.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuizProject.Business.Services;
using QuizProject.API.Dtos;
using System.Security.Claims;
using QuizProject.MVC.ViewModels;
using System.Net.Http.Headers;
using System.Text.Json;

[Area("Student")]
[Route("Student/[controller]/[action]")]
public class DashboardController : Controller
{
    private readonly IUserQuizResultService _userQuizResultService;
    private readonly HttpClient _httpClient;
    private readonly ITestTypeService _testTypeService;
    private readonly ICategoryService _categoryService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<DashboardController> _logger;
    public DashboardController(
      IUserQuizResultService userQuizResultService,
      IHttpClientFactory httpClientFactory,
      ITestTypeService testTypeService,
                  ILogger<DashboardController> logger,
      ICategoryService categoryService)
    {
        _userQuizResultService = userQuizResultService;
        _httpClientFactory = httpClientFactory; // BU SATIR
        _httpClient = httpClientFactory.CreateClient("API");
        _testTypeService = testTypeService;
        _categoryService = categoryService;
        _logger = logger;
    }
    public async Task<IActionResult> Index(int categoryId, int testTypeId)
    {
        var category = await _categoryService.GetByIdAsync(categoryId);
        var testType = await _testTypeService.GetByIdAsync(testTypeId);

        ViewBag.CategoryId = categoryId;
        ViewBag.TestTypeId = testTypeId;
        ViewBag.CategoryName = category?.Name ?? "Unknown Category";
        ViewBag.TestTypeName = testType?.Name ?? "Unknown Test Type";
        ViewBag.TestTypeImage = string.IsNullOrEmpty(testType?.ImageUrl)
            ? "/img/default-testtype.png"
            : testType.ImageUrl;
        var quiz = await _httpClient.GetFromJsonAsync<Quiz>(
  $"api/student/student/quizzes/byCategoryAndTestType?categoryId={categoryId}&testTypeId={testTypeId}");



        if (quiz == null)
        {
            TempData["Error"] = "Bu kategori ve test tipine ait aktif bir quiz bulunamadı.";
            return RedirectToAction("Startquiz");
        }

        ViewBag.QuizId = quiz.Id;
        return View();
    }
    public async Task<IActionResult> Startquiz()/////////////////////////////////// başla//ilk sayfa//kategori seç //sayfa çalışıyor
    {
        var categories = await _httpClient.GetFromJsonAsync<List<Category>>("api/student/student/categories");
        return View(categories);
    }
    [HttpGet("/Student/TestType")]
    public async Task<IActionResult> GetTestTypesByCategoryIdAsync(int categoryId)//ikinci sayfa//testtpe seç  //sayfa çalışıyor
    {
        var testTypes = await _testTypeService.GetTestTypesByCategoryIdAsync(categoryId);
        var category = await _categoryService.GetByIdAsync(categoryId);
        ViewBag.CategoryId = categoryId;
        ViewBag.CategoryName = category?.Name ?? "Unknown";
        return View(testTypes);
    }
    [HttpGet]
    public async Task<IActionResult> Start(int quizId)
    {
        var quiz = await _httpClient.GetFromJsonAsync<Quiz>(
            $"api/student/student/quizzes/{quizId}");

        if (quiz == null || quiz.Questions == null || !quiz.Questions.Any())
            return RedirectToAction("Index");

        // Soru ID listesini TempData'ya koyuyoruz (sıralı ilerlemek için)
        TempData["QuizId"] = quiz.Id;
        TempData["QuestionIds"] = quiz.Questions.Select(q => q.Id).ToList();

        return RedirectToAction("Question", new { index = 0 }); // İlk soruya git
    }
    public async Task<IActionResult> Question(int index)
    {
        var questionIds = TempData["QuestionIds"] as List<int>;
        TempData.Keep("QuestionIds");
        TempData.Keep("QuizId");

        if (questionIds == null || index < 0 || index >= questionIds.Count)
            return RedirectToAction("Index");

        int questionId = questionIds[index];

        var question = await _httpClient.GetFromJsonAsync<Question>(
            $"api/student/student/quizzes/{questionId}/question");

        ViewBag.Index = index;
        ViewBag.TotalQuestions = questionIds.Count;
        return View("Question", question);
    }
    [HttpPost]
    public async Task<IActionResult> SubmitAnswer(int index, int questionId, string selectedOption, string direction)
    {
        TempData.Keep("QuestionIds");
        TempData.Keep("QuizId");

        var questionIds = TempData["QuestionIds"] as List<int>;
        if (questionIds == null)
            return RedirectToAction("Startquiz");

        var quizId = TempData["QuizId"] as int?;
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Sadece ileri giderken cevabı kaydet
        if (direction == "next" && !string.IsNullOrEmpty(selectedOption))
        {
            var answerDto = new UserAnswerDto
            {
                UserId = userId,
                QuizId = quizId ?? 0,
                QuestionId = questionId,
                SelectedOption = selectedOption
            };

            await _httpClient.PostAsJsonAsync("api/student/student/answers", answerDto);
        }

        int nextIndex = direction == "prev" ? index - 1 : index + 1;

        if (nextIndex < 0)
            nextIndex = 0;

        if (nextIndex >= questionIds.Count)
            return RedirectToAction("Finish");

        return RedirectToAction("Question", new { index = nextIndex });
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

   
}


