namespace BibliomaticData.Models
{ 
    public class BaseQuestion : BaseTrackedEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Author { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public bool IsSolved { get; set; }
        public int AnswersCount { get; set; }
        public Question? Question { get; set; }          
        public ICollection<Answer>? Answers { get; set; }
    }
}
