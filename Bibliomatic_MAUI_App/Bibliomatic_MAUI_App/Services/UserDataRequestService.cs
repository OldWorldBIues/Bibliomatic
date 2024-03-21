using Bibliomatic_MAUI_App.Models;
using System.Text.Json;

namespace Bibliomatic_MAUI_App.Services
{
    public class UserDataRequestService : IUserDataService
    {
        public string ControllerName { get; set; }

        public async Task<UserData> GetUserDataById(Guid id)
        {
            var dbException = new DataBaseException("Failed to load user data from server");

            var response = await RequestService.HttpClient.GetAsync($"{ControllerName}/{id}");
            response.ValidateStatusCode(dbException);

            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserData>(content, RequestService.JsonSerializerOptions);
        }

        public async Task<Author> UpdateUser(Guid id, Author author)
        {
            var dbException = new DataBaseException("Failed to update user on server");

            string json = JsonSerializer.Serialize(author, RequestService.JsonSerializerOptions);
            StringContent jsonContent = new StringContent(json, RequestService.DefaultEncoding, RequestService.JsonPatchMediaType);

            var response = await RequestService.HttpClient.PatchAsync($"{ControllerName}/{id}", jsonContent);
            response.ValidateStatusCode(dbException);

            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Author>(content, RequestService.JsonSerializerOptions);
        }
    }
}
