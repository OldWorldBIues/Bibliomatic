namespace BibliomaticData.Models.DTOs
{
    public class TestDTO : BaseTrackedEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }              
    }
}
