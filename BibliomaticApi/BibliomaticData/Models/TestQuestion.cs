namespace BibliomaticData.Models
{
    public class TestQuestion
    {
        public Guid Id { get; set; }
        public string Question { get; set; }
        public Guid TestId { get; set; }
        public double PointsPerAnswer { get; set; }
        public string? TestQuestionFormulaFilename { get; set; }
        public string? TestQuestionFilename { get; set; }
        public TestQuestionType TestQuestionType { get; set; }
        public Test? Test { get; set; }
        public ICollection<TestAnswer>? TestAnswers { get; set; }
    }

    public enum TestQuestionType
    {
        OneAnswerQuestion,
        MultipleAnswerQuestion,
        PairsQuestion,
        OpenEndedQuestion
    }
}
