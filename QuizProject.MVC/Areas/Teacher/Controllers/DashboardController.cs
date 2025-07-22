using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizProject.Business.Interfaces;
using QuizProject.Data.Models;
using System.Net.Http;

namespace QuizProject.MVC.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    public class DashboardController : Controller
    {
        private readonly IUserQuizResultService _userQuizResultService;
        private readonly HttpClient _httpClient;

        public DashboardController(IUserQuizResultService userQuizResultService, IHttpClientFactory httpClientFactory)
        {
            _userQuizResultService = userQuizResultService;
            _httpClient = httpClientFactory.CreateClient("API");
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CategoryDetails(int id)
        {
            var category = await _httpClient.GetFromJsonAsync<Category>($"api/student/student/categories/{id}/with-quizzes");
            if (category == null) return NotFound();

            return View(category);
        }

    }
}
