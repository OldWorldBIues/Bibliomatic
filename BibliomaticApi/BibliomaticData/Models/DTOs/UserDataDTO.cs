namespace BibliomaticData.Models.DTOs
{
    public class UserDataDTO
    {
        public Guid Id { get; set; }
        public ICollection<BaseQuestionDTO>? Questions { get; set; }
        public ICollection<ArticleDTO>? Articles { get; set; }
        public ICollection<TestDTO>? Tests { get; set; }       
    }
}
