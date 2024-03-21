using BibliomaticData.Models.AttachmentInfo;
using BibliomaticData.Models.SocialData;

namespace BibliomaticData.Models
{
    public class Question : BaseTrackedEntity
    {
        public Guid Id { get; set; }
        public Guid? BaseQuestionId { get; set; }
        public Guid UserId { get; set; }
        public BaseQuestion? BaseQuestion { get; set; }
        public string QuestionHtmlDocument { get; set; }
        public string QuestionBody { get; set; }
        public string Author { get; set; }       
        public int QuestionCommentsCount { get; set; }
        public ICollection<QuestionImageInfo>? Images { get; set; }
        public ICollection<QuestionHyperlinkInfo>? Hyperlinks { get; set; }
        public ICollection<QuestionFormulaInfo>? Formulas { get; set; }
        public List<QuestionComment>? QuestionComments { get; set; }
        public List<QuestionLike>? QuestionLikes { get; set; }
        public List<QuestionDislike>? QuestionDislikes { get; set; }
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
