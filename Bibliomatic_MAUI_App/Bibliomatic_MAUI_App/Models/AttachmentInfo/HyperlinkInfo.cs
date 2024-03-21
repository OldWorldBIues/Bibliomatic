using System.Text.Json.Serialization;

namespace Bibliomatic_MAUI_App.Models.AttachmentInfo
{
    public class QuestionHyperlinkInfo
    {
        public Guid Id { get; set; }
        public Guid? QuestionId { get; set; }
        public string Hyperlink { get; set; }
        public string HyperlinkDescription { get; set; }

        [JsonIgnore]
        public int IndexOfHyperlink { get; set; }
    }

    public class AnswerHyperlinkInfo
    {        
        public Guid? AnswerId { get; set; }
        public string Hyperlink { get; set; }
        public string HyperlinkDescription { get; set; }

        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public int IndexOfHyperlink { get; set; }
    }
}
