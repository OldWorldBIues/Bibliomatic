namespace BibliomaticData.Models.SocialData
{
    public class CommentBase : BaseTrackedEntity
    {
        public Guid Id { get; set; }        
        public Guid UserId { get; set; }
        public string Author { get; set; }
        public string Comment { get; set; }
    }
}
