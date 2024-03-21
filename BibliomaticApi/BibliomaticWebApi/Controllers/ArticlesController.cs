using BibliomaticData.Models;
using BibliomaticData.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web.Resource;
using BibliomaticData.Models.DTOs;
using Microsoft.AspNetCore.OData.Query;

namespace BibliomaticWebApi.Controllers
{
    [ApiController]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
    [Route("api/articles")]
    public class ArticlesController : Controller
    {        
        private readonly IRepository<Article, ArticleDTO> articleRepository;
        private readonly ISocialRepository<ArticleLike, ArticleDislike, ArticleComment> socialRepository;

        public ArticlesController(IRepository<Article, ArticleDTO> articleRepository, ISocialRepository<ArticleLike, ArticleDislike, ArticleComment> socialRepository)
        {           
            this.articleRepository = articleRepository;
            this.socialRepository = socialRepository;
        }

        [HttpGet]
        [EnableQuery(PageSize = 20)]
        public async Task<ActionResult> GetAllArticles()
        {
            var result = await articleRepository.GetAll();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}/comments")]
        [EnableQuery(PageSize = 5)]
        public async Task<ActionResult> GetArticleComments(Guid id)
        {
            var result = await socialRepository.GetAllComments(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetArticle(Guid id)
        {
            var result = await articleRepository.GetSummarizedById(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}/files")]
        public async Task<ActionResult> GetQuestionFiles(Guid id)
        {
            var result = await articleRepository.GetAllFiles(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult> CreateArticle(Article article)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            await articleRepository.Create(article);
            return CreatedAtAction(nameof(GetArticle), new { id = article.Id }, article);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateArticle(Guid id, Article article)
        {
            if (!ModelState.IsValid || id != article.Id)
                return BadRequest();

            var result = await articleRepository.GetById(id);

            if (result == null)
                return NotFound();

            await articleRepository.Update(article);

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteArticle(Guid id)
        {
            var article = await articleRepository.GetById(id);

            if (article == null)
                return NotFound();

            await articleRepository.Delete(article);
            return NoContent();
        }        

        [HttpPost]
        [Route("{id}/likes")]
        public async Task<ActionResult> AddArticleLike(Guid id, ArticleLike like)
        {
            if (!ModelState.IsValid && id != like.ArticleId)
                return BadRequest();           
            
            var article = await articleRepository.GetById(id);            

            if (article == null)
                return NotFound();

            await socialRepository.AddLike(like);
            return Ok(like);
        }

        [HttpDelete]
        [Route("{id}/likes/{likeId}")]
        public async Task<ActionResult> RemoveArticleLike(Guid id, Guid likeId)
        {
            var article = await articleRepository.GetById(id);
            var like = article.ArticleLikes.FirstOrDefault(al => al.Id == likeId);

            if (like == null)
                return NotFound();

            await socialRepository.RemoveLike(like);
            return NoContent();
        }

        [HttpPost]
        [Route("{id}/dislikes")]
        public async Task<ActionResult> AddArticleDislike(Guid id, ArticleDislike dislike)
        {
            if (!ModelState.IsValid && id != dislike.ArticleId)
                return BadRequest();

            var article = await articleRepository.GetById(id);

            if (article == null)
                return NotFound();

            await socialRepository.AddDislike(dislike);
            return Ok(dislike);
        }

        [HttpDelete]
        [Route("{id}/dislikes/{dislikeId}")]
        public async Task<ActionResult> RemoveArticleDislike(Guid id, Guid dislikeId)
        {
            var article = await articleRepository.GetById(id);
            var dislike = article.ArticleDislikes.FirstOrDefault(adl => adl.Id == dislikeId);

            if (dislike == null)
                return NotFound();

            await socialRepository.RemoveDislike(dislike);
            return NoContent();
        }

        [HttpPost]
        [Route("{id}/comments")]
        public async Task<ActionResult> AddArticleComment(Guid id, ArticleComment comment)
        {
            if (!ModelState.IsValid && id != comment.ArticleId)
                return BadRequest();

            var article = await articleRepository.GetById(id);

            if (article == null)
                return NotFound();

            await socialRepository.AddComment(comment);
            return Ok(comment);
        }

        [HttpDelete]
        [Route("{id}/comments/{commentId}")]
        public async Task<ActionResult> RemoveArticleComment(Guid id, Guid commentId)
        {
            var article = await articleRepository.GetById(id);
            var comment = article.ArticleComments.FirstOrDefault(ac => ac.Id == commentId);

            if (comment == null)
                return NotFound();

            await socialRepository.RemoveComment(comment);
            return NoContent();
        }
    }
}
