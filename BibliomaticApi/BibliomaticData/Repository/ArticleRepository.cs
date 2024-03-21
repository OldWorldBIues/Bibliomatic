using BibliomaticData.AppContext;
using BibliomaticData.Models;
using BibliomaticData.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Article = BibliomaticData.Models.Article;


namespace BibliomaticData.Repository
{
    public class ArticleRepository : IRepository<Article, ArticleDTO>, ISocialRepository<ArticleLike, ArticleDislike, ArticleComment>
    {
        private readonly BibliomaticAppContext appContext;        

        public ArticleRepository(BibliomaticAppContext appContext)
        {
            this.appContext = appContext;
        }

        public async Task<IEnumerable<ArticleDTO>> GetAll()
        {
            var articles = await appContext.Articles.Select(a => new ArticleDTO
            {
                Id = a.Id,
                Title = a.Title,
                UserId = a.UserId,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt
            }).ToListAsync();

            foreach(var article in articles)
            {
                article.Author = await GraphApi.GetAuthor(article.UserId);
            }

            return articles;
        }

        public async Task<IEnumerable<string>> GetAllFiles(Guid id)
        {
            var articleFiles = new List<string>();

            var article = await appContext.Articles.IgnoreAutoIncludes()
                                                   .FirstOrDefaultAsync(a => a.Id == id);

            if (article == null) return null;

            if (!string.IsNullOrEmpty(article?.ArticleDocumentSource))
            {
                articleFiles.Add(article.ArticleDocumentSource);
                articleFiles.Add(article.ArticleImageSource);
            }

            return articleFiles;
        }

        public async Task<Article> GetById(Guid id)
        {
            return await appContext.Articles.Include(a => a.ArticleComments)
                                            .AsNoTracking()                
                                            .FirstOrDefaultAsync(a => a.Id == id);
        }

        private async Task SetArticleAuthor(Article article)
        {
            article.Author = await GraphApi.GetAuthor(article.UserId);
        }

        private async Task SetArticleCommentsAuthor(IEnumerable<ArticleComment> articleComments)
        {
            foreach (var articleComment in articleComments)
            {
                articleComment.Author = await GraphApi.GetAuthor(articleComment.UserId);
            }
        }

        public async Task<Article> GetSummarizedById(Guid id)
        {
            var article = await appContext.Articles.AsNoTracking()                                                                                              
                                                   .FirstOrDefaultAsync(a => a.Id == id);

            if (article == null) return null;
            
            var articlesComments = await appContext.ArticleComments.Where(ac => ac.ArticleId == id).OrderByDescending(ac => ac.CreatedAt).Skip(0).Take(5).ToListAsync();
            int articleCommentsCount = await appContext.ArticleComments.Where(ac => ac.ArticleId == id).CountAsync();

            await SetArticleAuthor(article);
            await SetArticleCommentsAuthor(articlesComments);

            article.ArticleComments = articlesComments;
            article.ArticleCommentsCount = articleCommentsCount;

            return article;
        }
        public async Task<Article> GetTrackedById(Guid id)
        {
            return await appContext.Articles.Include(a => a.ArticleComments).FirstOrDefaultAsync(a => a.Id == id);
        }
       
        public async Task Create(Article article)
        {
            await appContext.Articles.AddAsync(article);
            await appContext.SaveChangesAsync();
        }

        public async Task Delete(Article article)
        {
            appContext.Articles.Remove(article);
            await appContext.SaveChangesAsync();
        }

        public async Task Update(Article article)
        {
            appContext.Articles.Update(article);            
            await appContext.SaveChangesAsync();
        }

        public async Task SaveChanges()
        {
            await appContext.SaveChangesAsync();
        }

        public async Task AddLike(ArticleLike like)
        {
            appContext.ArticleLikes.Add(like);
            await appContext.SaveChangesAsync();
        }

        public async Task RemoveLike(ArticleLike like)
        {
            appContext.ArticleLikes.Remove(like);
            await appContext.SaveChangesAsync();
        }

        public async Task AddDislike(ArticleDislike dislike)
        {
            appContext.ArticleDislikes.Add(dislike);
            await appContext.SaveChangesAsync();
        }

        public async Task RemoveDislike(ArticleDislike dislike)
        {
            appContext.ArticleDislikes.Remove(dislike);
            await appContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ArticleComment>> GetAllComments(Guid id)
        {
            var articleComments = await appContext.ArticleComments.Where(ac => ac.ArticleId == id).ToListAsync();
            await SetArticleCommentsAuthor(articleComments);   
            return articleComments;
        }

        public async Task AddComment(ArticleComment comment)
        {
            appContext.ArticleComments.Add(comment);
            await appContext.SaveChangesAsync();
        }

        public async Task RemoveComment(ArticleComment comment)
        {
            appContext.ArticleComments.Remove(comment);
            await appContext.SaveChangesAsync();
        }           
    }
}
