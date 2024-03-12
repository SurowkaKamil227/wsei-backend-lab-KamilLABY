using ApplicationCore.Interfaces.QuizUserService;
using ApplicationCore.Models;
using Infrastructure.MongoDB.Entities;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizzesController : ControllerBase
    {
        private readonly QuizUserServiceMongoDB mongoDB;
       public QuizzesController(QuizUserServiceMongoDB _mongoDB)
        {
           mongoDB = _mongoDB;
        }
        [HttpGet]
        public List<Quiz> Get()
        {
            return mongoDB.FindAllQuizzes();
        }

        // GET api/<QuizzesController>/5
        [HttpGet("{id}")]
        public Quiz Get(int id)
        {
            return mongoDB.FindQuizById(id);
        }

        [HttpPost("answer/{quizId}/{itemId}")]
        public QuizItemUserAnswer SaveUserAnswerForQuiz([FromBody] QuizItemAnswerDto quizItemUserAnswer,  [FromRoute] int quizId, [FromRoute] int itemId)
        {
           return  mongoDB.SaveUserAnswerForQuiz(quizId,itemId,quizItemUserAnswer.UserId,quizItemUserAnswer.Answer);
            
        }
        [HttpGet("userAnswers/{quizId}/{userId}")]
        public List<QuizItemUserAnswer> GetUserAnswersForQuiz(int quizId, int userId)
        {
            return mongoDB.GetUserAnswersForQuiz(quizId, userId);
        }


    }
}
