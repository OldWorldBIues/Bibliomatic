using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;
using System.Collections.ObjectModel;
using Bibliomatic_MAUI_App.Models.AttachmentInfo;
using Bibliomatic_MAUI_App.Models.SocialActivity;

namespace Bibliomatic_MAUI_App.Models
{
    [ObservableObject]
    public partial class QuestionResponce : TimeTrackedModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? BaseQuestionId { get; set; }
        public BaseQuestionResponce? BaseQuestion { get; set; }        
        public ObservableCollection<QuestionImageInfo>? Images { get; set; }
        public ObservableCollection<QuestionHyperlinkInfo>? Hyperlinks { get; set; }
        public ObservableCollection<QuestionFormulaInfo>? Formulas { get; set; }
        public ObservableCollection<QuestionComment>? QuestionComments { get; set; }
        public ObservableCollection<QuestionLike>? QuestionLikes { get; set; }
        public ObservableCollection<QuestionDislike>? QuestionDislikes { get; set; }
        public string QuestionHtmlDocument { get; set; }
        public string Author { get; set; }
        public string QuestionBody { get; set; }
        public int QuestionCommentsCount { get; set; }

        #region Additional properties
        [JsonIgnore]
        public string QuestionHtmlDocumentPath { get; set; }

        [JsonIgnore]
        public ObservableCollection<QuestionComment> AllQuestionComments { get; set; }

        [ObservableProperty]
        public int allQuestionCommentsCount;

        [JsonIgnore]
        public string ShortAuthorName { get => Author[..Author.LastIndexOf(' ')]; }

        [ObservableProperty]
        [JsonIgnore]
        public string questionComment;

        [JsonIgnore]
        public bool CanEditQuestion { get => UserId == CurrentUser.Id; }

        [JsonIgnore]
        public HtmlWebViewSource QuestionHtmlFormat { get; set; }

        [JsonIgnore]
        public List<QuestionImageInfo> AllAttachedImages { get; set; }
       
        [JsonIgnore]
        public List<QuestionFormulaInfo> AllAttachedFormulas { get; set; }
        #endregion
    }

    public class QuestionLike : LikeBase
    {        
        public Guid QuestionId { get; set; }       
    }

    public class QuestionDislike : DislikeBase
    {      
        public Guid QuestionId { get; set; }      
    }

    public class QuestionComment : CommentBase
    {      
        public Guid QuestionId { get; set; }      
    }
}
