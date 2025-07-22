namespace QuizProject.API.Dtos
{
    public class SubmitAnswerDto
    {
        public string UserId { get; set; } = null!;
        public int QuizId { get; set; }
        public List<AnswerItemDto> Answers { get; set; } = new();
    }

    public class AnswerItemDto
    {
        public int QuestionId { get; set; }
        public int SelectedOptionId { get; set; }
    }
}
