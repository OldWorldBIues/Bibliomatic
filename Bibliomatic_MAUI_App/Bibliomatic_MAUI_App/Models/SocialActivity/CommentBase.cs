using System.Text.Json.Serialization;


namespace Bibliomatic_MAUI_App.Models.SocialActivity
{
    public class CommentBase : TimeTrackedModel
    {
        public Guid Id { get; set; }        
        public Guid UserId { get; set; }
        public string Comment { get; set; }
        public string Author { get; set; }       

        [JsonIgnore]
        public bool CanEditComment { get => CurrentUser.Id == UserId; }
    }   
}
