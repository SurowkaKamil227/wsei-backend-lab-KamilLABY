using ApplicationCore.Models;
using System.Runtime.CompilerServices;

namespace WebAPI.Dto
{
    public class QuizItemDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public List<string> Options { get; set; }

        public static QuizItemDto Of(QuizItem quiz) => new QuizItemDto
        {
            Id = quiz.Id,
            Question = quiz.Question,
            Options = quiz.IncorrectAnswers.Append(quiz.CorrectAnswer).ToList()
        };
        
    }
}
