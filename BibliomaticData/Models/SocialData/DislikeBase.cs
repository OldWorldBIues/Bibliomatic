namespace BibliomaticData.Models.SocialData
{
    public class DislikeBase : BaseTrackedEntity
    {
        public Guid Id { get; set; }      
        public Guid UserId { get; set; }
    }
}
