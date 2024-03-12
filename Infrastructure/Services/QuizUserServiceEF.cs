using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Infrastructure.EF.Entities;
using Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class QuizUserServiceEF : IQuizUserService
    {
        private QuizDbContext _context;
        public QuizUserServiceEF(QuizDbContext context)
        {
            _context = context;
        }
        public Quiz CreateAndGetQuizRandom(int count)
        {
            throw new NotImplementedException();
        }
        public List<Quiz> FindAllQuizzes()
        {
            return _context
                .Quizzes
                .AsNoTracking()
                .Include(q => q.Items)
                .ThenInclude(i => i.IncorrectAnswers)
                .Select(QuizMappers.FromEntityToQuiz)
                .ToList();
        }

        public Quiz? FindQuizById(int id)
        {
            var entity = _context
                .Quizzes
                .AsNoTracking()
                .Include(q => q.Items)
                .ThenInclude(i => i.IncorrectAnswers)
                .FirstOrDefault(e => e.Id == id);
            return entity is null ? null : QuizMappers.FromEntityToQuiz(entity);
        }
        public List<QuizItemUserAnswer> GetUserAnswersForQuiz(int quizId, int userId)
        {
            throw new NotImplementedException();
        }

        public QuizItemUserAnswer SaveUserAnswerForQuiz(int quizId, int quizItemId, int userId, string answer)
        {
            var quizzEntity = _context.Quizzes.Find(quizId);  // pobierz encję quizu o quizId
        if (quizzEntity is null)
            {
                throw new Exception($"Quiz with id {quizId} not found");
            }
            var item = _context.QuizItems.Find(quizItemId); // pobierz encję elementu quizu o quizItemId 
        if (item is null)
            {
                throw new Exception($"Quiz item with id {quizId} not found");
            }
            QuizItemUserAnswerEntity entity = new QuizItemUserAnswerEntity()
            {
                QuizId = quizId,
                UserAnswer = answer,
                UserId = userId, 
                QuizItemId = quizItemId
            };
            var savedEntity = _context.Add(entity).Entity;
            _context.SaveChanges();
           
            return new QuizItemUserAnswer(quizItem :QuizMappers.FromEntityToQuizItem(item), userId: userId,quizId : quizId, answer: answer)
        {
              // uzupełnij aby zmapować obiekt entity na klasę QuizItemUserAnswer 
        };
        }
    }
}
