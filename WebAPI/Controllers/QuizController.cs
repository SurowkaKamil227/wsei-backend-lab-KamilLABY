using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using WebAPI.Dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private IQuizUserService _service;

        public QuizController(IQuizUserService service) 
        {
            _service = service;
        }

        // GET: api/<QuizController>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<QuizDto>> FindById(int id)
        {
            QuizDto quizDto = new QuizDto();
            var quiz = _service.FindQuizById(id);

            if (quiz != null)
            {
                quizDto.Id = quiz.Id;
                quizDto.Title = quiz.Title;
                quizDto.Items = quiz.Items;
                return Ok(quizDto);
            }
            else
                return NotFound();
        }
        // GET api/<QuizController>/5
        [HttpGet]
        public IEnumerable<QuizDto> FindAll()
        {
            var quizzes = _service.FindAllQuizzes()
                .Select(q => new QuizDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Items = q.Items
                });
            return quizzes;

        }
        [HttpPost]
        [Route("{quizId}/items/{itemId}/answers")]
        public ActionResult SaveAnswer([FromBody] QuizItemAnswerDto dto, int quizId, int itemId)
        {
            try
            {
                //var answer = _service.SaveUserAnswerForQuiz(quizId, itemId, dto.UserId, dto.UserAnswer);
                return Created("", _service.SaveUserAnswerForQuiz(quizId, itemId, dto.UserId, dto.Answer));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet, Produces("application/json")]
        [Route("{quizId}/feedbacks")]
        public FeedbackQuizDto GetFeedback(int quizId)
        {
            int userId = 1;
            var answers = _service.GetUserAnswersForQuiz(quizId, userId);
            //TODO: zdefiniuj mapper listy odpowiedzi na obiekt FeedbackQuizDto 
            return new FeedbackQuizDto()
            {
                QuizId = quizId,
                UserId = 1,
                QuizItemsAnswers = answers.Select(i => new FeedbackQuizItemDto()
                {
                    Question = i.QuizItem.Question,
                    Answer = i.Answer,
                    IsCorrect = i.IsCorrect(),
                    QuizItemId = i.QuizItem.Id
                }).ToList()
            };
        }
    }





}
