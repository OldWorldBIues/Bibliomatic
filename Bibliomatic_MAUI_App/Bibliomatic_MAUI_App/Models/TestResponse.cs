using Bibliomatic_MAUI_App.Models.SocialActivity;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Bibliomatic_MAUI_App.Models
{
    [ObservableObject]
    public partial class TestResponse : TimeTrackedModel
    {        
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ObservableCollection<TestQuestionResponse>? TestQuestions { get; set; }
        public ObservableCollection<TestComment>? TestComments { get; set; }
        public ObservableCollection<TestLike>? TestLikes { get; set; }
        public ObservableCollection<TestDislike>? TestDislikes { get; set; }
        public ObservableCollection<UserScore>? UserScores { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TestCommentsCount { get; set; }
        public int TestUserScoresCount { get; set; }

        #region Additional properties   
        [JsonIgnore]
        public ObservableCollection<TestComment> AllTestComments { get; set; }

        [ObservableProperty]
        public int allTestCommentsCount;

        [JsonIgnore]
        public ObservableCollection<UserScore> AllTestUserScores { get; set; }

        [ObservableProperty]
        public int allTestUserScoresCount;

        [JsonIgnore]
        public string ShortAuthorName { get => Author[..Author.LastIndexOf(' ')]; }

        [ObservableProperty]
        [JsonIgnore]
        public string testComment;       
        #endregion
    }

    public class TestLike : LikeBase
    {       
        public Guid TestId { get; set; }       
    }

    public class TestDislike : DislikeBase
    {      
        public Guid TestId { get; set; }      
    }

    public class TestComment : CommentBase
    {       
        public Guid TestId { get; set; }       
    }
    
    public class UserScore
    {
        public Guid Id { get; set; }
        public Guid TestId { get; set; }
        public Guid UserId { get; set; }
        public double PointsForTest { get; set; }
        public string User { get; set; }
        public DateTimeOffset TestStartDate { get; set; }
        public DateTimeOffset TestEndDate { get; set; }

        [JsonIgnore]
        public string TestStartDateFormat { get => TestStartDate.ToLocalTime().ToString("dd.MM.yyyy HH:mm"); }
        [JsonIgnore]
        public string TestEndDateFormat { get => TestEndDate.ToLocalTime().ToString("dd.MM.yyyy HH:mm"); }
    }
}
