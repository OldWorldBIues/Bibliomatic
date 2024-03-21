using Bibliomatic_MAUI_App.Models.AttachmentInfo;
using Bibliomatic_MAUI_App.Models.SocialActivity;
using System.Text.Json.Serialization;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bibliomatic_MAUI_App.Models
{
    [ObservableObject]
    public partial class AnswerResponce : TimeTrackedModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? BaseQuestionId { get; set; }
        public BaseQuestionResponce? BaseQuestion { get; set; }
        public ObservableCollection<AnswerImageInfo>? Images { get; set; }
        public ObservableCollection<AnswerHyperlinkInfo>? Hyperlinks { get; set; }
        public ObservableCollection<AnswerFormulaInfo>? Formulas { get; set; }
        public ObservableCollection<AnswerComment>? AnswerComments { get; set; }
        public ObservableCollection<AnswerLike>? AnswerLikes { get; set; }
        public ObservableCollection<AnswerDislike>? AnswerDislikes { get; set; }
        public string AnswerHtmlDocument { get; set; }
        public string Author { get; set; }
        public string AnswerBody { get; set; }
        public bool IsAnswer { get; set; }
        public int AnswerCommentsCount { get; set; }

        #region Additional properties    
        [JsonIgnore]
        public string AnswerHtmlDocumentPath { get; set; }

        [JsonIgnore]
        public ObservableCollection<AnswerComment> AllAnswerComments { get; set; }

        [ObservableProperty]
        public int allAnswerCommentsCount;

        [JsonIgnore]
        public string ShortAuthorName { get => Author[..Author.LastIndexOf(' ')]; }

        [ObservableProperty]
        public bool markedAsAnswer;

        [ObservableProperty]
        [JsonIgnore]        
        public string answerComment;

        [ObservableProperty]
        [JsonIgnore]
        public HtmlWebViewSource answerHtmlFormat;

        [JsonIgnore]       
        public bool CanEditAnswer { get => UserId == CurrentUser.Id; }       

        [JsonIgnore]
        public List<AnswerImageInfo> AllAttachedImages { get; set; }

        [JsonIgnore]
        public List<AnswerFormulaInfo> AllAttachedFormulas { get; set; }        
        #endregion       
    }

    public class AnswerLike : LikeBase
    {       
        public Guid AnswerId { get; set; }       
    }

    public class AnswerDislike : DislikeBase
    {       
        public Guid AnswerId { get; set; }       
    }

    public class AnswerComment : CommentBase
    {       
        public Guid AnswerId { get; set; }        
    }
}
