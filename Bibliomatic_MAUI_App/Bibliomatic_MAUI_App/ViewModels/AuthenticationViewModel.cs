using Bibliomatic_MAUI_App.Services.AzureB2CAuthService;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.Input;
using Bibliomatic_MAUI_App.Models;
using Bibliomatic_MAUI_App.Helpers;

namespace Bibliomatic_MAUI_App.ViewModels
{
    public partial class AuthenticationViewModel
    {
        private readonly IAuthService authenticationService;

        public AuthenticationViewModel(IAuthService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [RelayCommand]
        public async Task AuthenticateUser()
        {
            try
            {
                await PermissionsChecker.RequestRequiredPermissions();
                var tokenResult = await authenticationService.LoginAsync(CancellationToken.None);

                TokenService.SetCurrentUserSetting(tokenResult);
                TokenService.SetAuthorizationHeader(tokenResult);
                
                string successMessage = $"Welcome {CurrentUser.FullName}";
                await Toast.Make(successMessage).Show();

                Application.Current.MainPage = new AppShell();
            }
            catch(Exception)
            {
                string errorMessage = "Something went wrong. Try again or later";
                await Toast.Make(errorMessage).Show();
            }
        }
    }
}
