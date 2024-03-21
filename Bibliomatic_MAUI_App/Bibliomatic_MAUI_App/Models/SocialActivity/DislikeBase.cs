namespace Bibliomatic_MAUI_App.Models.SocialActivity
{
    public class DislikeBase : TimeTrackedModel
    {
        public Guid Id { get; set; }        
        public Guid UserId { get; set; }
    }
}
