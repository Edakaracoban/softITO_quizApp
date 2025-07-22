namespace QuizProject.API.Dtos
{
    public class UserAnswerDto
    {
        public string UserId { get; set; }
        public int QuizId { get; set; }
        public int QuestionId { get; set; }
        public string SelectedOption { get; set; }
        public bool IsCorrect { get; set; }
    }
}
