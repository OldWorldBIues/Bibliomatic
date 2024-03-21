using Bibliomatic_MAUI_App.CustomControls;
using Bibliomatic_MAUI_App.Services.AzureB2CAuthService;
using Bibliomatic_MAUI_App.ViewModels;
using Bibliomatic_MAUI_App.Views;
using Bibliomatic_MAUI_App.Models.AttachmentInfo;
using CommunityToolkit.Maui.Views;
using Microsoft.Identity.Client;

namespace Bibliomatic_MAUI_App.Services
{
    public class LoadingResult
    {
        public bool IsRetry { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class LoadingService
    {
        AuthService authService = new AuthService();
        LoadingPopup loadingPopup;
        LoadingErrorPopup loadingErrorPopup;

        private async Task<LoadingResult> HandleExceptions(Func<Task> loadingAction)
        {
            try 
            {               
                await loadingAction();
                return new LoadingResult { IsRetry = false, IsSuccess = true };                      
            }
            catch(Exception exception)
            {                
                bool isRetry;

                switch (exception)
                {
                    case UnauthorizedException:
                        isRetry = await TryToReauthorize(exception.Message);
                        break;

                    case DataBaseException:
                    case FileStorageException:
                    case DataNotFoundException:
                        loadingPopup.Close();
                        loadingErrorPopup = new LoadingErrorPopup(exception.Message, false);
                        isRetry = (bool)await Application.Current.MainPage.ShowPopupAsync(loadingErrorPopup);
                        break;

                    default:
                        loadingPopup.Close();
                        loadingErrorPopup = new LoadingErrorPopup();
                        isRetry = (bool)await Application.Current.MainPage.ShowPopupAsync(loadingErrorPopup);
                        break;
                }

                return new LoadingResult { IsRetry = isRetry, IsSuccess = false };
            }
        }

        private async Task<LoadingResult> HandleExceptionsWithFilesUpload(List<string> files, Func<List<string>, List<AttachmentDTOResponse>, Task> loadingAction)
        {
            var loadedFilesList = new List<AttachmentDTOResponse>();            

            try
            {
                await loadingAction(files, loadedFilesList);
                return new LoadingResult { IsRetry = false, IsSuccess = true };
            }
            catch (Exception exception)
            {
                bool isRetry;

                switch (exception)
                {
                    case UnauthorizedException:
                        isRetry = await TryToReauthorize(exception.Message);
                        break;

                    case FileStorageException:
                        loadingPopup.Close();
                        loadingErrorPopup = new LoadingErrorPopup(exception.Message, false);
                        loadedFilesList.AddRange((List<AttachmentDTOResponse>)exception.Data["LoadedFilesList"]);
                        files = (List<string>)exception.Data["FailedFilesList"];
                        isRetry = (bool)await Application.Current.MainPage.ShowPopupAsync(loadingErrorPopup);
                        break;

                    case DataBaseException:                    
                    case DataNotFoundException:
                        loadingPopup.Close();
                        loadingErrorPopup = new LoadingErrorPopup(exception.Message, false);                        
                        isRetry = (bool)await Application.Current.MainPage.ShowPopupAsync(loadingErrorPopup);
                        break;


                    default:
                        loadingPopup.Close();
                        loadingErrorPopup = new LoadingErrorPopup();
                        isRetry = (bool)await Application.Current.MainPage.ShowPopupAsync(loadingErrorPopup);
                        break;
                }

                return new LoadingResult { IsRetry = isRetry, IsSuccess = false };
            }
        }

        private async Task<bool> TryToReauthorize(string message)
        {
            try
            {
                TokenService.RemoveAuthorizationHeader();                
                var tokenResult = await authService.LoginSilently(CancellationToken.None);

                TokenService.SetCurrentUserSetting(tokenResult);
                TokenService.SetAuthorizationHeader(tokenResult);

                return true;
            }
            catch (MsalUiRequiredException)
            {
                loadingPopup.Close();
                loadingErrorPopup = new LoadingErrorPopup(message, true);
                bool isReauthorized = (bool)await Application.Current.MainPage.ShowPopupAsync(loadingErrorPopup);

                if (isReauthorized)
                {
                    await authService.SignOutAsync();

                    TokenService.RemoveAuthorizationHeader();
                    Application.Current.MainPage = new NavigationPage(new AuthenticationView(new AuthenticationViewModel(authService)));
                }

                return false;
            }
            catch(Exception)
            {
                loadingPopup.Close();
                loadingErrorPopup = new LoadingErrorPopup();
                return (bool)await Application.Current.MainPage.ShowPopupAsync(loadingErrorPopup);
            }
        }

        public async Task PerformLoading(string loadingHeader, Func<Task> loadingAction)
        {
            LoadingResult loadingResult = null;
            bool isRetry = true;

            while(isRetry)
            {
                loadingPopup = new LoadingPopup(loadingHeader);
                Application.Current.MainPage.ShowPopup(loadingPopup);

                loadingResult = await HandleExceptions(loadingAction);
                isRetry = loadingResult.IsRetry;
            }

            if(loadingResult.IsSuccess) loadingPopup.Close();
        }

        public async Task PerformLoading(string loadingHeader, string confirmationHeader, Func<Task> loadingAction)
        {
            LoadingResult loadingResult = null;
            bool isRetry = true;            

            while (isRetry)
            {
                loadingPopup = new LoadingPopup(loadingHeader);
                Application.Current.MainPage.ShowPopup(loadingPopup); 
                
                loadingResult = await HandleExceptions(loadingAction);
                isRetry = loadingResult.IsRetry;                
            }

            if (loadingResult.IsSuccess) loadingPopup.ChangeToCompleted(confirmationHeader);
        }

        public async Task PerformLoadingWithFilesUpload(string loadingHeader, string confirmationHeader, string routeToNavigate, Dictionary<string, object> navigationParameters, List<string> filesToUpload, Func<List<string>, List<AttachmentDTOResponse>, Task> loadingAction)
        {
            LoadingResult loadingResult = null;
            bool isRetry = true;

            while (isRetry)
            {
                loadingPopup = new LoadingPopup(loadingHeader, routeToNavigate, navigationParameters);
                Application.Current.MainPage.ShowPopup(loadingPopup);

                loadingResult = await HandleExceptionsWithFilesUpload(filesToUpload, loadingAction);
                isRetry = loadingResult.IsRetry;                
            }

            if (loadingResult.IsSuccess) loadingPopup.ChangeToCompleted(confirmationHeader); 
        }

        public async Task PerformLoadingWithFilesUpload(string loadingHeader, string confirmationHeader, List<string> filesToUpload, Func<List<string>, List<AttachmentDTOResponse>, Task> loadingAction)
        {
            LoadingResult loadingResult = null;
            bool isRetry = true;

            while (isRetry)
            {
                loadingPopup = new LoadingPopup(loadingHeader);
                Application.Current.MainPage.ShowPopup(loadingPopup);

                loadingResult = await HandleExceptionsWithFilesUpload(filesToUpload, loadingAction);
                isRetry = loadingResult.IsRetry;                
            }
            
            if (loadingResult.IsSuccess) loadingPopup.ChangeToCompleted(confirmationHeader);
        }
    }
}
