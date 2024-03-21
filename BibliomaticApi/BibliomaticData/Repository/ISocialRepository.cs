using BibliomaticData.Models;

namespace BibliomaticData.Repository
{
    public interface ISocialRepository<TLike, TDislike, TComment> : ILikesRepository<TLike>, IDislikesRepository<TDislike>, ICommentsRepository<TComment> { }   

    public interface ILikesRepository<TLike>
    {      
        Task AddLike(TLike like);
        Task RemoveLike(TLike like);
    }

    public interface IDislikesRepository<TDislike>
    {
        Task AddDislike(TDislike dislike);
        Task RemoveDislike(TDislike dislike);
    }

    public interface ICommentsRepository<TComment>
    {
        Task<IEnumerable<TComment>> GetAllComments(Guid id);        
        Task AddComment(TComment comment);
        Task RemoveComment(TComment comment);
    }

    public interface IUserScoresRepository<TUserScore>
    {
        Task<IEnumerable<TUserScore>> GetAllUserScores(Guid id);
        Task<SummarizedUserScore> GetAllSummarizedUserScoresForUser(Guid id, Guid userId);
        Task AddUserScore(TUserScore userScore);
        Task RemoveUserScore(TUserScore userScore);
    }  

}
