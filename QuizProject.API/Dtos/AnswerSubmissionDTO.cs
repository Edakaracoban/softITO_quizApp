namespace QuizProject.API.Dtos
{
    public class AnswerSubmissionDTO
    {
        public string UserId { get; set; } = null!;
        public int QuizId { get; set; }
        public int QuestionId { get; set; }
        public string SelectedOption { get; set; } = null!;
    }
}
