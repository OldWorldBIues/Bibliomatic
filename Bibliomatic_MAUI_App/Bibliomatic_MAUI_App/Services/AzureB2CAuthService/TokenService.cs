using System.IdentityModel.Tokens.Jwt;
using Microsoft.Identity.Client;
using Bibliomatic_MAUI_App.Models;

namespace Bibliomatic_MAUI_App.Services.AzureB2CAuthService
{
    public class TokenService
    {
        public static void SetCurrentUserSetting(AuthenticationResult authenticationResult)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            string accessToken = authenticationResult.AccessToken;            
            var tokenData = jwtTokenHandler.ReadJwtToken(accessToken);            

            if (tokenData != null)
            {
                Guid id = Guid.Parse(tokenData.Claims.FirstOrDefault(c => c.Type.Equals("sub"))?.Value);
                string userEmail = tokenData.Claims.FirstOrDefault(c => c.Type.Equals("emails"))?.Value;
                string userFirstName = tokenData.Claims.FirstOrDefault(c => c.Type.Equals("extension_Firstname"))?.Value;
                string userLastName = tokenData.Claims.FirstOrDefault(c => c.Type.Equals("extension_Lastname"))?.Value;
                string userMiddleName = tokenData.Claims.FirstOrDefault(c => c.Type.Equals("extension_Middlename"))?.Value;

                new CurrentUser(id, userEmail, userFirstName, userLastName, userMiddleName);
            }
        }
                                                                                                    
        public static void SetAuthorizationHeader(AuthenticationResult authenticationResult)
        {
            string authorizationHeader = authenticationResult.CreateAuthorizationHeader();
            RequestService.HttpClient.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
        }

        public static void RemoveAuthorizationHeader()
        {
            RequestService.HttpClient.DefaultRequestHeaders.Remove("Authorization");
        }
    }
}
