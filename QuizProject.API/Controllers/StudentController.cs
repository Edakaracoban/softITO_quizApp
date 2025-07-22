using Microsoft.AspNetCore.Mvc;
using QuizProject.API.Dtos;
using QuizProject.Business.Interfaces;
using QuizProject.Data.Models;

namespace QuizProject.API.Controllers
{
    [Area("Student")]
    [ApiController]
    [Route("api/student/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IQuestionService _questionService;
        private readonly IQuizCommentService _quizCommentService;
        private readonly IQuizService _quizService;
        private readonly ITestTypeService _testTypeService;
        private readonly IUserAnswerService _userAnswerService;
        private readonly IUserQuizResultService _userQuizResultService;
        public StudentController(
            IQuizService quizService,
            IQuestionService questionService,
            IUserQuizResultService userQuizResultService,
            IUserAnswerService userAnswerService,
            IQuizCommentService quizCommentService,
            ICategoryService categoryService,
            ITestTypeService testTypeService)
        {
            _quizService = quizService;
            _questionService = questionService;
            _userQuizResultService = userQuizResultService;
            _userAnswerService = userAnswerService;
            _quizCommentService = quizCommentService;
            _categoryService = categoryService;
            _testTypeService = testTypeService;
        }
        [HttpGet("quizzes/active")]
        public async Task<IActionResult> GetActiveQuizzes() => Ok(await _quizService.GetActiveQuizzesAsync());//
        [HttpGet("quizzes/{id}")]
        public async Task<IActionResult> GetQuizById(int id)//
        {
            var quiz = await _quizService.GetQuizWithQuestionsAsync(id);
            return quiz == null ? NotFound() : Ok(quiz);
        }
        [HttpGet("quizzes/{quizId}/questions")]
        public async Task<IActionResult> GetQuestionsByQuiz(int quizId) =>//aslında burada quizler geliyor 
            Ok(await _questionService.GetQuestionsByQuizIdAsync(quizId));
        [HttpGet("quizzes/{quizId}/questions/{currentQuestionId}/next")]
        public async Task<IActionResult> GetNextQuestion(int quizId, int currentQuestionId)//
        {
            var question = await _questionService.GetNextQuestionAsync(quizId, currentQuestionId);
            return question == null ? NotFound() : Ok(question);
        }
        [HttpGet("quizzes/{quizId}/questions/{currentQuestionId}/previous")]
        public async Task<IActionResult> GetPreviousQuestion(int quizId, int currentQuestionId)//
        {
            var question = await _questionService.GetPreviousQuestionAsync(quizId, currentQuestionId);
            return question == null ? NotFound() : Ok(question);
        }
    
        [HttpGet("categories/popular")]
        public async Task<IActionResult> GetPopularCategories([FromQuery] int top = 5) =>
            Ok(await _categoryService.GetPopularCategoriesAsync(top));
        [HttpGet("categories/{categoryId}/quizzes")]
        public async Task<IActionResult> GetQuizzesByCategory(int categoryId) =>
            Ok(await _quizService.GetQuizzesByCategoryAsync(categoryId));
        [HttpGet("categories/{categoryId}/quiz-count")]
        public async Task<IActionResult> GetQuizCountByCategory(int categoryId) =>
            Ok(await _categoryService.GetQuizCountByCategoryAsync(categoryId));
        [HttpGet("categories/search")]
        public async Task<IActionResult> SearchCategories([FromQuery] string term) =>
            Ok(await _categoryService.SearchCategoriesAsync(term));
        [HttpGet("categories/{id}/with-quizzes")]
        public async Task<IActionResult> GetCategoryWithQuizzes(int id)
        {
            var category = await _categoryService.GetCategoryWithQuizzesAsync(id);
            return category == null ? NotFound() : Ok(category);
        }
    
        [HttpGet("test-types")]
        public async Task<IActionResult> GetAllTestTypes() => Ok(await _testTypeService.GetAllAsync());
        [HttpGet("test-types/with-quizzes")]
        public async Task<IActionResult> GetTestTypesWithQuizzes() =>
            Ok(await _testTypeService.GetTestTypesWithQuizzesAsync());
        [HttpGet("test-types/by-name")]
        public async Task<IActionResult> GetTestTypesByName(string? term)
        {
            var result = await _testTypeService.SearchTestTypesAsync(term); 
            return Ok(result);
        }
        [HttpGet("quizzes/by-testtype/{testTypeId}")]
        public async Task<IActionResult> GetQuizzesByTestType(int testTypeId)
        {
            var quizzes = await _quizService.GetQuizzesByTestTypeAsync(testTypeId);
            return Ok(quizzes);
        }
        [HttpGet("results/{userId}")]
        public async Task<IActionResult> GetUserResults(string userId) =>
            Ok(await _userQuizResultService.GetResultsByUserIdAsync(userId));
        [HttpGet("results/{userId}/quiz/{quizId}")]
        public async Task<IActionResult> GetResultByUserAndQuiz(string userId, int quizId)
        {
            var result = await _userQuizResultService.GetResultByUserAndQuizAsync(userId, quizId);
            return result == null ? NotFound() : Ok(result);
        }
        [HttpGet("answers/{userId}/quiz/{quizId}")]
        public async Task<IActionResult> GetAnswersByUserAndQuiz(string userId, int quizId) =>
            Ok(await _userAnswerService.GetAnswersByUserAndQuizResultAsync(userId, quizId));
        [HttpGet("comments/quiz/{quizId}")]
        public async Task<IActionResult> GetCommentsByQuiz(int quizId) =>
            Ok(await _quizCommentService.GetCommentsByQuizIdAsync(quizId));
        [HttpPost("comments")]
        public async Task<IActionResult> AddComment([FromBody] QuizComment comment)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _quizCommentService.CreateAsync(comment);
            return Ok();
        }
        [HttpPut("comments/{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] QuizComment comment)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != comment.Id) return BadRequest("Comment ID mismatch");
            var existing = await _quizCommentService.GetByIdAsync(id);
            if (existing == null) return NotFound();
            await _quizCommentService.UpdateAsync(comment);
            return NoContent();
        }
        [HttpDelete("comments/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _quizCommentService.GetByIdAsync(id);
            if (comment == null) return NotFound();

            await _quizCommentService.DeleteAsync(comment);
            return NoContent();
        }
        [HttpGet("comments/user/{userId}")]
        public async Task<IActionResult> GetCommentsByUser(string userId) =>
            Ok(await _quizCommentService.GetCommentsByUserIdAsync(userId));
        [HttpPost("answers")]
        public async Task<IActionResult> SubmitAnswer([FromBody] UserAnswerDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userQuizResult = await _userQuizResultService.GetResultByUserAndQuizAsync(dto.UserId, dto.QuizId);
            if (userQuizResult == null) return BadRequest("UserQuizResult not found.");
            var userAnswer = new UserAnswer
            {
                UserQuizResultId = userQuizResult.Id,
                QuestionId = dto.QuestionId,
                SelectedOption = dto.SelectedOption,
                IsCorrect = dto.IsCorrect,
                AnsweredAt = DateTime.Now
            };
            await _userAnswerService.AddUserAnswerAsync(userAnswer);
            return Ok();
        }




        [HttpGet("quizzes/byCategoryAndTestType")]
        public async Task<IActionResult> GetQuizByCategoryAndTestType(int categoryId, int testTypeId)
        {
            var quizzes = await _quizService.GetQuizzesByCategoryAndTestTypeAsync(categoryId, testTypeId);
            var quiz = quizzes.FirstOrDefault();
            if (quiz == null)
                return NotFound();

            return Ok(quiz);
        }
        [HttpGet("categories/{categoryId}/test-types")]
        public async Task<IActionResult> GetTestTypesByCategory(int categoryId)
        {
            var testTypes = await _testTypeService.GetTestTypesByCategoryAsync(categoryId);
            return Ok(testTypes);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories() => Ok(await _categoryService.GetAllAsync());
    }
}
