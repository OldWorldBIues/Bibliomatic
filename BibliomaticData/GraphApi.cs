using BibliomaticData.Models;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace BibliomaticData
{
    public class GraphApi
    {
        public static GraphServiceClient GraphServiceClient { get; set; }

        public static async Task<string> GetAuthor(Guid id)
        {
            var user = await GraphServiceClient.Users[$"{id}"].GetAsync(x =>
            {
                x.QueryParameters.Select = new[]
                {
                    "extension_067eebb53aed4152aa46b7076813abdd_Middlename",
                    "extension_067eebb53aed4152aa46b7076813abdd_Lastname",
                    "extension_067eebb53aed4152aa46b7076813abdd_Firstname"
                };
            });

            user.AdditionalData.TryGetValue("extension_067eebb53aed4152aa46b7076813abdd_Firstname", out object firstName);
            user.AdditionalData.TryGetValue("extension_067eebb53aed4152aa46b7076813abdd_Lastname", out object lastName);
            user.AdditionalData.TryGetValue("extension_067eebb53aed4152aa46b7076813abdd_Middlename", out object middleName);

            return $"{lastName} {firstName} {middleName}";
        }

        public static async Task<Author> ChangeUser(Author author)
        {
            var userData = await GraphServiceClient.Users[$"{author.Id}"].GetAsync();

            if(userData == null) return null;

            var newUserData = new User
            {                
                AdditionalData = new Dictionary<string, object>()
                {
                    { "extension_067eebb53aed4152aa46b7076813abdd_Middlename", author.MiddleName },
                    { "extension_067eebb53aed4152aa46b7076813abdd_Lastname", author.LastName },
                    { "extension_067eebb53aed4152aa46b7076813abdd_Firstname", author.FirstName }
                }
            };
            
            await GraphServiceClient.Users[$"{author.Id}"].PatchAsync(newUserData);
            await GraphServiceClient.Users[$"{author.Id}"].RevokeSignInSessions.PostAsRevokeSignInSessionsPostResponseAsync();

            return author;
        }
    }
}
