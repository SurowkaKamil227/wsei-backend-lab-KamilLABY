using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MongoDB.Entities
{
    public class QuizUserServiceMongoDB : IQuizUserService
    {
        private readonly IMongoCollection<QuizMongoEntity> _quizzes;
        private readonly IMongoCollection<QuizItemAnswerMongoEntity> _answers;
        private readonly MongoClient _client;

        public QuizUserServiceMongoDB(IOptions<MongoDBSettings> settings)
        {
            _client = new MongoClient(settings.Value.ConnectionUri);
            IMongoDatabase database = _client.GetDatabase(settings.Value.DatabaseName);
            _quizzes = database.GetCollection<QuizMongoEntity>(settings.Value.QuizCollection);
         _answers = database.GetCollection<QuizItemAnswerMongoEntity>(settings.Value.AnswerCollection);
        }

        public List<Quiz> FindAllQuizzes()
        {
            var quizMongoEntities = _quizzes.Find(Builders<QuizMongoEntity>.Filter.Empty).ToList();
            return _quizzes
                .Find(Builders<QuizMongoEntity>.Filter.Empty)
                .Project(
                    q =>
                        new Quiz(
                            q.QuizId,
                            q.Items.Select(i => new QuizItem(
                                    i.ItemId,
                                    i.Question,
                                    i.IncorrectAnswers,
                                    i.CorrectAnswer
                                )
                            ).ToList(),
                            q.Title
                        )
                ).ToList();
        }

        public Quiz? FindQuizById(int id)
        {
            return _quizzes
                .Find(Builders<QuizMongoEntity>.Filter.Eq(q => q.QuizId, id))
                .Project(q =>
                    new Quiz(
                        q.QuizId,
                        q.Items.Select(i => new QuizItem(
                                i.ItemId,
                                i.Question,
                                i.IncorrectAnswers,
                                i.CorrectAnswer
                            )
                        ).ToList(),
                        q.Title
                    )
                ).FirstOrDefault();
        }

        public QuizItemUserAnswer SaveUserAnswerForQuiz(int quizId, int quizItemId, int userId, string answer)
        {
            var findQuizItem = _quizzes.Find(x => x.QuizId == quizItemId).FirstOrDefault();
            var quizItemUserAnswerMongo = (new QuizItemAnswerMongoEntity()
            {
                QuizId = quizId,
                QuizItemId = quizItemId,
                UserId = userId,
                UserAnswer = answer
              

            });
            _answers.InsertOne( quizItemUserAnswerMongo );
            var quizItem = new QuizItemUserAnswer(
               new QuizItem(
                      quizItemUserAnswerMongo.QuizItem.ItemId,
                        quizItemUserAnswerMongo.QuizItem.Question,
                           quizItemUserAnswerMongo.QuizItem.IncorrectAnswers,
                          quizItemUserAnswerMongo.QuizItem.CorrectAnswer),
                quizItemUserAnswerMongo.UserId,
                quizItemUserAnswerMongo.QuizId,
                quizItemUserAnswerMongo.UserAnswer
            );
            return quizItem;
            
            
        }

        public List<QuizItemUserAnswer> GetUserAnswersForQuiz(int quizId, int userId)
        {


            var quizAnswerMongoEntities = _answers.Find(Builders<QuizItemAnswerMongoEntity>.Filter.Empty).ToList();
            return (List<QuizItemUserAnswer>)_answers
                .Find(Builders<QuizItemAnswerMongoEntity>.Filter.Empty);
              
                    
                    


                
        }

        public Quiz CreateAndGetQuizRandom(int count)
        {
            throw new NotImplementedException();
        }

    }
}
