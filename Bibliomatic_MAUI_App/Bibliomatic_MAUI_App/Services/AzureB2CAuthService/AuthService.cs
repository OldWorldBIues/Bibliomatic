using Microsoft.Identity.Client;
using System.Diagnostics;

namespace Bibliomatic_MAUI_App.Services.AzureB2CAuthService
{
    
    public class AuthService : IAuthService
    {
        private readonly IPublicClientApplication authenticationClient;
        private readonly IPublicClientApplication editProfileClient;
        public AuthService()
        {
            authenticationClient = PublicClientApplicationBuilder.Create(Constants.ClientId)
                .WithB2CAuthority(Constants.AuthoritySignIn)                
                .WithRedirectUri($"msal{Constants.ClientId}://auth")
                .Build();

            editProfileClient = PublicClientApplicationBuilder.Create(Constants.ClientId)
                .WithB2CAuthority(Constants.AuthorityEdit)
                .WithRedirectUri($"msal{Constants.ClientId}://auth")
                .Build();
        }

        public async Task<AuthenticationResult> LoginAsync(CancellationToken cancellationToken)
        {            
            try
            {
                return await LoginSilently(cancellationToken);
            }
            catch (MsalUiRequiredException rex)
            {
                Debug.WriteLine(rex);
                return await LoginInteractively(cancellationToken);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<AuthenticationResult> EditProfileAsync(CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<IAccount> accounts = await authenticationClient.GetAccountsAsync();

                var result = await editProfileClient
                   .AcquireTokenInteractive(Constants.Scopes)                   
                   .WithPrompt(Prompt.NoPrompt)                   
                   .WithAccount(GetAccountByEnvironment(accounts, Constants.AccountEnvironment))
#if ANDROID
                    .WithParentActivityOrWindow(Microsoft.Maui.ApplicationModel.Platform.CurrentActivity)
#endif
                   .ExecuteAsync(cancellationToken);
                return result;
            }           
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<AuthenticationResult> LoginSilently(CancellationToken cancellationToken)
        {
            IEnumerable<IAccount> accounts = await authenticationClient.GetAccountsAsync();

            var result = await authenticationClient
            .AcquireTokenSilent(Constants.Scopes, GetAccountByEnvironment(accounts, Constants.AccountEnvironment))
            .WithForceRefresh(true)
            .ExecuteAsync(cancellationToken);

            return result;
        }
        public async Task<AuthenticationResult> LoginInteractively(CancellationToken cancellationToken)
        {
            try
            {
                var result = await authenticationClient
                    .AcquireTokenInteractive(Constants.Scopes)
                    .WithPrompt(Prompt.ForceLogin)
#if ANDROID
                    .WithParentActivityOrWindow(Microsoft.Maui.ApplicationModel.Platform.CurrentActivity)
#endif
                    .ExecuteAsync(cancellationToken);
                return result;
            }
            catch (MsalClientException msalEx)
            {
                if (msalEx.ErrorCode == "authentication_canceled")
                {
                    return null;
                }
                Debug.Write(msalEx);
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }
        private IAccount GetAccountByEnvironment(IEnumerable<IAccount> accounts, string environment)
        {
            foreach (var account in accounts)
            {
                if (account.Environment.ToLower() == environment.ToLower()) return account;
            }

            return null;
        }

        public async Task SignOutAsync()
        {
            IEnumerable<IAccount> accounts = await authenticationClient.GetAccountsAsync();
            while (accounts.Any())
            {
                await authenticationClient.RemoveAsync(accounts.FirstOrDefault());
                accounts = await authenticationClient.GetAccountsAsync();
            }
        }
    }
}
