using BibliomaticData.Models;
using BibliomaticData.Models.DTOs;

namespace BibliomaticData.Repository
{
    public interface IUserDataRepository
    {
        Task<UserDataDTO> GetUserDataById(Guid id);
        Task<UserData> GetTrackedUserDataById(Guid id);
        Task<Author> UpdateUserData(Author author);        
    }
}
