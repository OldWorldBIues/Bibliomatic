namespace BibliomaticData.Models.AttachmentInfo
{
    public class QuestionImageInfo
    {
        public Guid Id { get; set; }
        public Guid? QuestionId { get; set; }
        public string ImageFilename { get; set; }
    }

    public class AnswerImageInfo
    {
        public Guid Id { get; set; }
        public Guid? AnswerId { get; set; }
        public string ImageFilename { get; set; }
    }
}
