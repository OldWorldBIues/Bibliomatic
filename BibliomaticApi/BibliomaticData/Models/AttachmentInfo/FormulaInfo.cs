namespace BibliomaticData.Models.AttachmentInfo
{
    public class QuestionFormulaInfo
    {
        public Guid Id { get; set; }
        public Guid? QuestionId { get; set; }
        public string FormulaLatex { get; set; }
        public string FormulaFilename { get; set; }
    }

    public class AnswerFormulaInfo
    {
        public Guid Id { get; set; }
        public Guid? AnswerId { get; set; }
        public string FormulaLatex { get; set; }
        public string FormulaFilename { get; set; }
    }
}
