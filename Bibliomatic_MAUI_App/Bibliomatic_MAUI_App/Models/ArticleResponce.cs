using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;
using System.Collections.ObjectModel;
using Bibliomatic_MAUI_App.Models.SocialActivity;

namespace Bibliomatic_MAUI_App.Models
{
    [ObservableObject]
    public partial class ArticleResponce : TimeTrackedModel
    {        
        public Guid Id { get; set; }   
        public Guid UserId { get; set; }
        public ObservableCollection<ArticleComment>? ArticleComments { get; set; }
        public ObservableCollection<ArticleLike>? ArticleLikes { get; set; }
        public ObservableCollection<ArticleDislike>? ArticleDislikes { get; set; }
        public string Title { get; set; }        
        public string Author { get; set; }          
        public string Description { get; set; }           
        public string ArticleImageSource { get; set; }
        public string ArticleDocumentSource { get; set; }
        public int ArticleCommentsCount { get; set; }

        #region Additional properties
        [JsonIgnore]
        public ObservableCollection<ArticleComment> AllArticleComments { get; set; }

        [ObservableProperty]
        public int allArticlesCommentsCount;

        [JsonIgnore]
        public string ShortAuthorName { get => Author[..Author.LastIndexOf(' ')]; }

        [ObservableProperty]
        [JsonIgnore]
        public string articleComment;

        [ObservableProperty]
        [JsonIgnore]
        public string articleImagePath;

        [ObservableProperty]
        [JsonIgnore]
        public string articleDocumentPath;       
        #endregion
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
