using BibliomaticData.Models.SocialData;

namespace BibliomaticData.Models
{   
    public class Article : BaseTrackedEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }       
        public string Title { get; set; }
        public string Author { get; set; }         
        public string Description { get; set; }
        public string ArticleImageSource { get; set; }
        public string ArticleDocumentSource { get; set; }
        public int ArticleCommentsCount { get; set; }
        public List<ArticleComment>? ArticleComments { get; set; }
        public List<ArticleLike>? ArticleLikes { get; set; }
        public List<ArticleDislike>? ArticleDislikes { get; set; }
    }

    public class ArticleLike : LikeBase
    {       
        public Guid ArticleId { get; set; }      
    }

    public class ArticleDislike : DislikeBase
    {       
        public Guid ArticleId { get; set; }        
    }

    public class ArticleComment : CommentBase
    {       
        public Guid ArticleId { get; set; }       
    }
}
