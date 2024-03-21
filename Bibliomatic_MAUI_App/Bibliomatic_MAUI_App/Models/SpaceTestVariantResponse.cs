namespace Bibliomatic_MAUI_App.Models
{
    public class SpaceTestVariantResponse
    {
        public Guid Id { get; set; }
        public string Variant { get; set; }
        public bool IsCorrectSpace { get; set; }
        public Guid TestAnswerId { get; set; }
        public TestAnswerResponse TestAnswer { get; set; }        
    }
}
