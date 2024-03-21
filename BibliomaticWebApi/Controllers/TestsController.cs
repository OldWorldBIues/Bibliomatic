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
    [Route("api/tests")]
    public class TestsController : Controller
    {        
        private readonly ITestRepository testRepository;
        private readonly ISocialRepository<TestLike, TestDislike, TestComment> socialRepository;
        private readonly IUserScoresRepository<UserScore> userScoresRepository;

        public TestsController(ITestRepository testRepository, IUserScoresRepository<UserScore> userScoresRepository, ISocialRepository<TestLike, TestDislike, TestComment> socialRepository)
        {
            this.testRepository = testRepository;
            this.socialRepository = socialRepository;
            this.userScoresRepository = userScoresRepository;            
        }

        [HttpGet]
        [EnableQuery(PageSize = 20)]
        public async Task<ActionResult> GetAllTests([FromQuery] Guid userId, [FromQuery] bool passed)
        {
            IEnumerable<TestDTO> result;

            if(userId == Guid.Empty)
            {
                result = await testRepository.GetAll();
            }
            else
            {
                result = await testRepository.GetAllByTestResult(userId, passed);
            }
           
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}/comments")]
        [EnableQuery(PageSize = 5)]
        public async Task<ActionResult> GetTestComments(Guid id)
        {
            var result = await socialRepository.GetAllComments(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}/userScores")]
        [EnableQuery(PageSize = 5)]
        public async Task<ActionResult> GetTestUserScores(Guid id)
        {            
            var result = await userScoresRepository.GetAllUserScores(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetTest(Guid id, [FromQuery]Guid userId)
        {
            Test result;

            if(userId == Guid.Empty)
            {
                result = await testRepository.GetSummarizedById(id);
            }
            else
            {
                result = await testRepository.GetSummarizedById(id, userId);
            }
           
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}/files")]
        public async Task<ActionResult> GetQuestionFiles(Guid id)
        {
            var result = await testRepository.GetAllFiles(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTest(Test test)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            await testRepository.Create(test);
            return CreatedAtAction(nameof(GetTest), new { id = test.Id }, test);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateTest(Guid id, Test test)
        {
            if (!ModelState.IsValid || id != test.Id)
                return BadRequest();
            
            await testRepository.Update(test);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteTest(Guid id)
        {
            var test = await testRepository.GetById(id);

            if (test == null)
                return NotFound();

            await testRepository.Delete(test);
            return NoContent();
        }

        [HttpPost]
        [Route("{id}/likes")]
        public async Task<ActionResult> AddTestLike(Guid id, TestLike like)
        {
            if (!ModelState.IsValid && id != like.TestId)
                return BadRequest();

            var test = await testRepository.GetById(id);

            if (test == null)
                return NotFound();

            await socialRepository.AddLike(like);
            return Ok(like);
        }


        [HttpDelete]
        [Route("{id}/likes/{likeId}")]
        public async Task<ActionResult> RemoveTestLike(Guid id, Guid likeId)
        {
            var test = await testRepository.GetById(id);
            var like = test.TestLikes.FirstOrDefault(tl => tl.Id == likeId);

            if (like == null)
                return NotFound();

            await socialRepository.RemoveLike(like);
            return NoContent();
        }

        [HttpPost]
        [Route("{id}/dislikes")]
        public async Task<ActionResult> AddTestDislike(Guid id, TestDislike dislike)
        {
            if (!ModelState.IsValid && id != dislike.TestId)
                return BadRequest();

            var test = await testRepository.GetById(id);

            if (test == null)
                return NotFound();

            await socialRepository.AddDislike(dislike);
            return Ok(dislike);
        }

        [HttpDelete]
        [Route("{id}/dislikes/{dislikeId}")]
        public async Task<ActionResult> RemoveTestDislike(Guid id, Guid dislikeId)
        {
            var test = await testRepository.GetById(id);
            var dislike = test.TestDislikes.FirstOrDefault(tdl => tdl.Id == dislikeId);

            if (dislike == null)
                return NotFound();

            await socialRepository.RemoveDislike(dislike);
            return NoContent();
        }

        [HttpPost]
        [Route("{id}/comments")]
        public async Task<ActionResult> AddTestComment(Guid id, TestComment comment)
        {
            if (!ModelState.IsValid && id != comment.TestId)
                return BadRequest();

            var article = await testRepository.GetById(id);

            if (article == null)
                return NotFound();

            await socialRepository.AddComment(comment);
            return Ok(comment);
        }


        [HttpDelete]
        [Route("{id}/comments/{commentId}")]
        public async Task<ActionResult> RemoveTestComment(Guid id, Guid commentId)
        {
            var test = await testRepository.GetById(id);
            var comment = test.TestComments.FirstOrDefault(tc => tc.Id == commentId);

            if (comment == null)
                return NotFound();

            await socialRepository.RemoveComment(comment);
            return NoContent();
        }

        [HttpPost]
        [Route("{id}/scores")]
        public async Task<ActionResult> AddTestUserScore(Guid id, UserScore userScore)
        {
            if (!ModelState.IsValid && id != userScore.TestId)
                return BadRequest();

            var test = await testRepository.GetById(id);

            if (test == null)
                return NotFound();

            await userScoresRepository.AddUserScore(userScore);
            return Ok(userScore);
        }


        [HttpDelete]
        [Route("{id}/scores/{userScoreId}")]
        public async Task<ActionResult> RemoveTestUserScore(Guid id, Guid userScoreId)
        {
            var test = await testRepository.GetById(id);
            var userScore = test.UserScores.FirstOrDefault(us => us.Id == userScoreId);

            if (userScore == null)
                return NotFound();

            await userScoresRepository.RemoveUserScore(userScore);
            return NoContent();
        }
    }
}
