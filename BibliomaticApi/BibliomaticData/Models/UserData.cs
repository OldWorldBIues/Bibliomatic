namespace BibliomaticData.Models
{
    public class UserData
    {
        public Guid Id { get; set; }
        public ICollection<BaseQuestion>? Questions { get; set; }
        public ICollection<Article>? Articles { get; set; }
        public ICollection<Test>? Tests { get; set; }       
    }
}
