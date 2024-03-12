using ApplicationCore.Models;

namespace WebAPI.Dto
{
    public class QuizDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<QuizItem> Items { get; set; }

        public static QuizDto of(Quiz quiz) => new QuizDto
        {
            Id = quiz.Id,
            Title = quiz.Title

        };
    }
}
