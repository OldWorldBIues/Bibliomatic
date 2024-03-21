using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliomaticData.Models.AttachmentInfo
{
    public class QuestionHyperlinkInfo
    {
        public Guid Id { get; set; }
        public Guid? QuestionId { get; set; }
        public string Hyperlink { get; set; }
        public string HyperlinkDescription { get; set; }
    }

    public class AnswerHyperlinkInfo
    {
        public Guid Id { get; set; }
        public Guid? AnswerId { get; set; }
        public string Hyperlink { get; set; }
        public string HyperlinkDescription { get; set; }
    }
}
