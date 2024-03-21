using Microsoft.Identity.Client;

namespace Bibliomatic_MAUI_App.Services.AzureB2CAuthService
{
    public interface IAuthService
    {
        Task<AuthenticationResult> LoginSilently(CancellationToken cancellationToken);
        Task<AuthenticationResult> LoginAsync(CancellationToken cancellationToken);
        Task<AuthenticationResult> EditProfileAsync(CancellationToken cancellationToken);
        Task<AuthenticationResult> LoginInteractively(CancellationToken cancellationToken);
        Task SignOutAsync();
    }
}
