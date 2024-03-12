
using ApplicationCore.Interfaces.Repository;
using ApplicationCore.Models;
using ApplicationCore.Specyfications;

namespace ApplicationCore.Interfaces.QuizUserService;

public class QuizUserService : IQuizUserService
{
    private IGenericRepository<Quiz, int> _quizRepository;
    private  IGenericRepository<QuizItem, int> _itemRepository;
    private IGenericRepository<QuizItemUserAnswer, string> _answerRepository;

    public QuizUserService(IGenericRepository<Quiz, int> quizRepository, IGenericRepository<QuizItemUserAnswer, string> answerRepository, IGenericRepository<QuizItem, int> itemRepository)
    {
        this._quizRepository = quizRepository;
        this._answerRepository = answerRepository;
        this._itemRepository = itemRepository;
    }

    public Quiz CreateAndGetQuizRandom(int count)
    {
        throw new NotImplementedException();
    }

    public List<Quiz> FindAllQuizzes()
    {
        return _quizRepository.FindAll();
    }

    public Quiz? FindQuizById(int id)
    {
        return _quizRepository.FindById(id);
    }

    public QuizItemUserAnswer SaveUserAnswerForQuiz(int quizId, int quizItemId, int userId, string answer)
    {
        var quiz = _quizRepository.FindById(quizId);
        var item = _itemRepository.FindById(quizItemId);
        if (quiz is null)
        {
            throw new Exception($"Quiz with id = {quizId} not found!");
        }

        if (item is null)
        {
            throw new Exception($"Quiz item with id = {quizItemId} not found!");
        }

        var userAnswer = new QuizItemUserAnswer(
        
            quizItem: item,
            quizId : quizId,
            userId : userId,
            answer :  answer
        );
        return _answerRepository.Add(userAnswer);
    }


    public List<QuizItemUserAnswer> GetUserAnswersForQuiz(int quizId, int userId)
    {
        // return answerRepository.FindAll()
        //     .Where(x => x.QuizId == quizId)
        //     .Where(x => x. UserId == userId)
        //     .ToList();
        return _answerRepository.FindBySpecification(new QuizItemsForQuizIdFilledByUser(quizId, userId)).ToList();
    }
}