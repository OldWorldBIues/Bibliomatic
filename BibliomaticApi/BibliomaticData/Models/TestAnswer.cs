namespace BibliomaticData.Models
{
    public class TestAnswer
    {
        public Guid Id { get; set; }
        public string Answer { get; set; }
        public string? TestAnswerFilename { get; set; }
        public string? TestAnswerFormulaFilename { get; set; }
        public string Variant { get; set; }
        public string? TestVariantFilename { get; set; }
        public string? TestVariantFormulaFilename { get; set; }
        public string Message { get; set; }        
        public bool IsCorrectAnswer { get; set; }
        public Guid TestQuestionId { get; set; }
        public TestQuestion? TestQuestion { get; set; }
    }
}
