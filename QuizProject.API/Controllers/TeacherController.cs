using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizProject.Business.Interfaces;

namespace QuizProject.API.Controllers
{
    [Area("Teacher")]
    [ApiController]
    [Route("api/teacher/[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IQuestionService _questionService;
        private readonly IQuizCommentService _quizCommentService;
        private readonly IQuizService _quizService;
        private readonly ITestTypeService _testTypeService;
        private readonly IUserAnswerService _userAnswerService;
        private readonly IUserQuizResultService _userQuizResultService;

        public TeacherController(
            IQuizService quizService,
            IQuestionService questionService,
            IUserQuizResultService userQuizResultService,
            IUserAnswerService userAnswerService,
            IQuizCommentService quizCommentService,
            ICategoryService categoryService,
            ITestTypeService testTypeService)
        {
            // Quiz ve soru işlemleri
            _quizService = quizService;
            _questionService = questionService;

            // Kullanıcı sonuçları ve cevapları
            _userQuizResultService = userQuizResultService;
            _userAnswerService = userAnswerService;

            // Yorum işlemleri
            _quizCommentService = quizCommentService;

            // Kategori işlemleri
            _categoryService = categoryService;

            // Test tipi işlemleri
            _testTypeService = testTypeService;
        }

        // ===============================
        //          QUIZ İŞLEMLERİ
        // ===============================
        [HttpGet("quizzes")]
        public async Task<IActionResult> GetAllQuizzes() => Ok(await _quizService.GetAllAsync());//


        [HttpGet("categories/paged")]
        public async Task<IActionResult> GetCategoriesPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10) =>
    Ok(await _categoryService.GetCategoriesAsync(page, pageSize));
    }
}
