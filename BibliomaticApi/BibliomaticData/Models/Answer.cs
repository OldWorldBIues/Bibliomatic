using BibliomaticData.Models.AttachmentInfo;
using BibliomaticData.Models.SocialData;

namespace BibliomaticData.Models
{
    public class Answer : BaseTrackedEntity
    {
        public Guid Id { get; set; }        
        public Guid UserId { get; set; }
        public Guid? BaseQuestionId { get; set; }
        public BaseQuestion? BaseQuestion { get; set; }
        public string AnswerHtmlDocument { get; set; }
        public string AnswerBody { get; set; }
        public string Author { get; set; }
        public bool IsAnswer { get; set; }      
        public int AnswerCommentsCount { get; set; }       
        public ICollection<AnswerImageInfo>? Images { get; set; }
        public ICollection<AnswerHyperlinkInfo>? Hyperlinks { get; set; }
        public ICollection<AnswerFormulaInfo>? Formulas { get; set; }
        public List<AnswerComment>? AnswerComments { get; set; }
        public List<AnswerLike>? AnswerLikes { get; set; }
        public List<AnswerDislike>? AnswerDislikes { get; set; }
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
