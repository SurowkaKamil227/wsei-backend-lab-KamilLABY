using ApplicationCore.Interfaces.Repository;
using ApplicationCore.Models;

namespace ApplicationCore.Interfaces;

public interface IQuizUserService
{
    Quiz CreateAndGetQuizRandom(int count);

    Quiz? FindQuizById(int id);
    List<Quiz> FindAllQuizzes();
    QuizItemUserAnswer SaveUserAnswerForQuiz(int quizId, int userId, int quizItemId, string answer);

    List<QuizItemUserAnswer> GetUserAnswersForQuiz(int quizId, int userId);

    int CountCorrectAnswersForQuizFilledByUser(int quizId, int userId)
    {
        return GetUserAnswersForQuiz(quizId, userId)
            .Count(e => e.IsCorrect());
    }

}