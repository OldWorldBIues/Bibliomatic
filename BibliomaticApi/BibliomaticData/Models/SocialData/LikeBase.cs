namespace BibliomaticData.Models.SocialData
{
    public class LikeBase : BaseTrackedEntity
    {
        public Guid Id { get; set; }        
        public Guid UserId { get; set; }
    }
}
