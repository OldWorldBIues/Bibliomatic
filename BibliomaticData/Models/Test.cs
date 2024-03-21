using BibliomaticData.Models.SocialData;

namespace BibliomaticData.Models
{    
    public class Test : BaseTrackedEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }       
        public string Author { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TestCommentsCount { get; set; }
        public int TestUserScoresCount { get; set; }
        public List<TestComment>? TestComments { get; set; }
        public List<TestLike>? TestLikes { get; set; }
        public List<TestDislike>? TestDislikes { get; set; }
        public List<UserScore>? UserScores { get; set; }
        public ICollection<TestQuestion>? TestQuestions { get; set; }       
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
    }

    public class SummarizedUserScore
    {
        public int Count { get; set; }
        public ICollection<UserScore> UserScores { get; set; }
    }
}
