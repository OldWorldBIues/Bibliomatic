using Bibliomatic_MAUI_App.Models;

namespace Bibliomatic_MAUI_App.Services
{
    public interface IUserDataService
    {
        string ControllerName { get; set; }      
        Task<UserData> GetUserDataById(Guid id);
        Task<Author> UpdateUser(Guid id, Author author);
    }
}
