namespace QuizProject.API.Dtos
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string CorrectOption { get; set; }
        public QuizShortDto Quiz { get; set; }
    }

    public class QuizShortDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public CategoryDto Category { get; set; }
        public TestTypeDto TestType { get; set; }
    }

    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }

    public class TestTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
