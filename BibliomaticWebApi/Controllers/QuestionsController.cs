using BibliomaticData.Models;
using BibliomaticData.Models.DTOs;
using BibliomaticData.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Identity.Web.Resource;

namespace BibliomaticWebApi.Controllers
{
    [ApiController]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
    [Route("api/questions")]
    public class QuestionsController : Controller
    {         
        private readonly IRepository<BaseQuestion, BaseQuestionDTO> questionRepository;
        private readonly IRelatedRepository<Answer> relatedAnswerRepository;
        private readonly ISocialRepository<QuestionLike, QuestionDislike, QuestionComment> questionsSocialRepository;
        private readonly ISocialRepository<AnswerLike, AnswerDislike, AnswerComment> answersSocialRepository;

        public QuestionsController(IRepository<BaseQuestion, BaseQuestionDTO> questionRepository, IRelatedRepository<Answer> relatedAnswerRepository,
                                   ISocialRepository<QuestionLike, QuestionDislike, QuestionComment> questionsSocialRepository,
                                   ISocialRepository<AnswerLike, AnswerDislike, AnswerComment> answersSocialRepository)
        {           
            this.questionRepository = questionRepository;
            this.relatedAnswerRepository = relatedAnswerRepository;
            this.questionsSocialRepository = questionsSocialRepository;
            this.answersSocialRepository = answersSocialRepository;
        }

        [HttpGet]
        [EnableQuery(PageSize = 20)]
        public async Task<ActionResult> GetAllQuestions()
        {
            var result = await questionRepository.GetAll();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}/answers")]
        [EnableQuery(PageSize = 5)]
        public async Task<ActionResult> GetAllAnswers(Guid id)
        {
            var result = await relatedAnswerRepository.GetAllRelated(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}/comments")]
        [EnableQuery(PageSize = 5)]
        public async Task<ActionResult> GetQuestionComments(Guid id)
        {
            var result = await questionsSocialRepository.GetAllComments(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}/answers/{answerId}/comments")]
        [EnableQuery(PageSize = 5)]
        public async Task<ActionResult> GetAnswerComments(Guid id, Guid answerId)
        {
            var answer = await relatedAnswerRepository.GetRelatedById(id, answerId);            
            var result = await answersSocialRepository.GetAllComments(answer.Id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetQuestion(Guid id)
        {
            var result = await questionRepository.GetSummarizedById(id);                  

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}/files")]
        public async Task<ActionResult> GetQuestionFiles(Guid id)
        {
            var result = await questionRepository.GetAllFiles(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        [HttpGet]
        [Route("{id}/answers/{answerId}")]
        public async Task<ActionResult> GetAnswer(Guid id, Guid answerId)
        {
            var result = await relatedAnswerRepository.GetRelatedById(id, answerId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateQuestion(BaseQuestion question)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await questionRepository.Create(question);
            return CreatedAtAction(nameof(GetQuestion), new { id = question.Id }, question);
        }

        [HttpPost]
        [Route("{id}/answers")]
        public async Task<ActionResult> CreateQuestionAnswer(Guid id, Answer answer)
        {
            if (!ModelState.IsValid || id != answer.BaseQuestionId)
                return BadRequest();

            await relatedAnswerRepository.CreateRelated(answer);
            return CreatedAtAction(nameof(GetAnswer), new { id = id, answerId = answer.Id }, answer);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateQuestion(Guid id, BaseQuestion question)
        {
            if (!ModelState.IsValid || id != question.Id)
                return BadRequest();            

            await questionRepository.Update(question);

            return NoContent();
        }

        [HttpPut]
        [Route("{id}/answers/{answerId}")]
        public async Task<ActionResult> UpdateQuestionAnswer(Guid id, Answer answer, Guid answerId)
        {
            if (!ModelState.IsValid || id != answer.BaseQuestionId || answerId != answer.Id)
                return BadRequest();

            await relatedAnswerRepository.UpdateRelated(answer);

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteQuestion(Guid id)
        {
            var question = await questionRepository.GetById(id);

            if (question == null)
                return NotFound();

            await questionRepository.Delete(question);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}/answers/{answerId}")]
        public async Task<ActionResult> DeleteQuestionAnswer(Guid id, Guid answerId)
        {
            var question = await questionRepository.GetById(id);
            var answer = question.Answers.FirstOrDefault(a => a.Id == answerId);

            if (answer == null)
                return NotFound();

            await relatedAnswerRepository.DeleteRelated(answer);
            return NoContent();
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<ActionResult> PatchBaseQuestion(Guid id, [FromBody] JsonPatchDocument<BaseQuestion> baseQuestion)
        {
            var result = await questionRepository.GetTrackedById(id);

            if (result == null)
                return NotFound();

            baseQuestion.ApplyTo(result);
            await questionRepository.SaveChanges();

            return NoContent();
        }

        [HttpPatch]
        [Route("{id}/question/{questionId}")]
        public async Task<ActionResult> PatchQuestion(Guid id, [FromBody] JsonPatchDocument<Question> question, Guid questionId)
        {
            var questionResult = await questionRepository.GetTrackedById(id);
            var result = questionResult.Question;

            if (result == null || result.Id != questionId)
                return NotFound();

            question.ApplyTo(result);
            await questionRepository.SaveChanges();

            return NoContent();
        }

        [HttpPatch]
        [Route("{id}/answers/{answerId}")]
        public async Task<ActionResult> PatchQuestionAnswer(Guid id, [FromBody] JsonPatchDocument<Answer> answer, Guid answerId)
        {
            var result = await relatedAnswerRepository.GetRelatedById(id, answerId);

            if (result == null)
                return NotFound();

            answer.ApplyTo(result);
            await relatedAnswerRepository.SaveChanges();

            return NoContent();
        }

        [HttpPost]
        [Route("{id}/likes")]
        public async Task<ActionResult> AddQuestionLike(Guid id, QuestionLike like)
        {
            if (!ModelState.IsValid && id != like.QuestionId)
                return BadRequest();

            var question = await questionRepository.GetById(id);

            if (question == null)
                return NotFound();

            await questionsSocialRepository.AddLike(like);
            return Ok(like);
        }

        [HttpDelete]
        [Route("{id}/likes/{likeId}")]
        public async Task<ActionResult> RemoveQuestionLike(Guid id, Guid likeId)
        {
            var question = await questionRepository.GetById(id);
            var like = question.Question.QuestionLikes.FirstOrDefault(ql => ql.Id == likeId);

            if (like == null)
                return NotFound();

            await questionsSocialRepository.RemoveLike(like);
            return NoContent();
        }

        [HttpPost]
        [Route("{id}/dislikes")]
        public async Task<ActionResult> AddQuestionDislike(Guid id, QuestionDislike dislike)
        {
            if (!ModelState.IsValid && id != dislike.QuestionId)
                return BadRequest();

            var question = await questionRepository.GetById(id);

            if (question == null)
                return NotFound();

            await questionsSocialRepository.AddDislike(dislike);
            return Ok(dislike);
        }

        [HttpDelete]
        [Route("{id}/dislikes/{dislikeId}")]
        public async Task<ActionResult> RemoveQuestionDislike(Guid id, Guid dislikeId)
        {
            var question = await questionRepository.GetById(id);
            var dislike = question.Question.QuestionDislikes.FirstOrDefault(qdl => qdl.Id == dislikeId);

            if (dislike == null)
                return NotFound();

            await questionsSocialRepository.RemoveDislike(dislike);
            return NoContent();
        }

        [HttpPost]
        [Route("{id}/comments")]
        public async Task<ActionResult> AddQuestionComment(Guid id, QuestionComment comment)
        {
            if (!ModelState.IsValid && id != comment.QuestionId)
                return BadRequest();

            var question = await questionRepository.GetById(id);

            if (question == null)
                return NotFound();

            await questionsSocialRepository.AddComment(comment);
            return Ok(comment);
        }

        [HttpDelete]
        [Route("{id}/comments/{commentId}")]
        public async Task<ActionResult> RemoveQuestionComment(Guid id, Guid commentId)
        {
            var question = await questionRepository.GetById(id);
            var comment = question.Question.QuestionComments.FirstOrDefault(qc => qc.Id == commentId);

            if (comment == null)
                return NotFound();

            await questionsSocialRepository.RemoveComment(comment);
            return NoContent();
        }

        [HttpPost]
        [Route("{id}/answers/{answerId}/likes")]
        public async Task<ActionResult> AddAnswerLike(Guid id, Guid answerId, AnswerLike like)
        {
            if (!ModelState.IsValid && answerId != like.AnswerId)
                return BadRequest();

            var question = await questionRepository.GetById(id);

            if (question == null)
                return NotFound();
           
            await answersSocialRepository.AddLike(like);
            return Ok(like);
        }

        [HttpDelete]
        [Route("{id}/answers/{answerId}/likes/{likeId}")]
        public async Task<ActionResult> RemoveAnswerLike(Guid id, Guid answerId, Guid likeId)
        {
            var answer = await relatedAnswerRepository.GetRelatedById(id, answerId);            
            var like = answer.AnswerLikes.FirstOrDefault(al => al.Id == likeId);

            if (like == null)
                return NotFound();

            await answersSocialRepository.RemoveLike(like);
            return NoContent();
        }

        [HttpPost]
        [Route("{id}/answers/{answerId}/dislikes")]
        public async Task<ActionResult> AddAnswerDisike(Guid id, Guid answerId, AnswerDislike dislike)
        {
            if (!ModelState.IsValid && answerId != dislike.AnswerId)
                return BadRequest();

            var question = await questionRepository.GetById(id);

            if (question == null)
                return NotFound();
            
            await answersSocialRepository.AddDislike(dislike);
            return Ok(dislike);
        }

        [HttpDelete]
        [Route("{id}/answers/{answerId}/dislikes/{dislikeId}")]
        public async Task<ActionResult> RemoveAnswerDislike(Guid id, Guid answerId, Guid dislikeId)
        {            
            var answer = await relatedAnswerRepository.GetRelatedById(id, answerId);
            var dislike = answer.AnswerDislikes.FirstOrDefault(adl => adl.Id == dislikeId);

            if (dislike == null)
                return NotFound();

            await answersSocialRepository.RemoveDislike(dislike);
            return NoContent();
        }

        [HttpPost]
        [Route("{id}/answers/{answerId}/comments")]
        public async Task<ActionResult> AddAnswerComment(Guid id, Guid answerId, AnswerComment comment)
        {
            if (!ModelState.IsValid && answerId != comment.AnswerId)
                return BadRequest();

            var question = await questionRepository.GetById(id);

            if (question == null)
                return NotFound();
            
            await answersSocialRepository.AddComment(comment);
            return Ok(comment);
        }

        [HttpDelete]
        [Route("{id}/answers/{answerId}/comments/{commentId}")]
        public async Task<ActionResult> RemoveAnswerComment(Guid id, Guid answerId, Guid commentId)
        {            
            var answer = await relatedAnswerRepository.GetRelatedById(id, answerId);
            var comment = answer.AnswerComments.FirstOrDefault(ac => ac.Id == commentId);

            if (comment == null)
                return NotFound();

            await answersSocialRepository.RemoveComment(comment);
            return NoContent();
        }
    }
}
