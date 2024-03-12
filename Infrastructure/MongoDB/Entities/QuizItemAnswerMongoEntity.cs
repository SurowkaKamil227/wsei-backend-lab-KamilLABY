using Infrastructure.EF.Entities;
using MongoDB.Bson.Serialization.Attributes;
namespace Infrastructure.MongoDB.Entities
{
   public class QuizItemAnswerMongoEntity : BaseMongoEntity
    {
        [BsonElement("userId")]
        public int UserId { get; set; }
        [BsonElement("quizItemId")]
        public int QuizItemId { get; set; }
        [BsonElement("quizId")]
        public int QuizId { get; set; }
        [BsonElement("userAnswer")]
        public string UserAnswer { get; set; }
        [BsonElement("quizItem")]
        public QuizItemMongoEntity QuizItem { get; set; }
    }
}
