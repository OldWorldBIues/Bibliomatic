namespace Bibliomatic_MAUI_App.Models.SocialActivity
{
    public class LikeBase : TimeTrackedModel
    {
        public Guid Id { get; set; }        
        public Guid UserId { get; set; }
    }
}
