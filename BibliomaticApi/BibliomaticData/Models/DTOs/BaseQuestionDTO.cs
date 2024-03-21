namespace BibliomaticData.Models.DTOs
{
    public class BaseQuestionDTO : BaseTrackedEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Author { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public bool IsSolved { get; set; }       
    }
}
