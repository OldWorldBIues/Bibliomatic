namespace Bibliomatic_MAUI_App.Services.AzureB2CAuthService
{
    public class Constants
    {        
        public static string ClientId;
        public static string TenantName;        
        public static string[] Scopes;

        public static string SignInPolicy;
        public static string EditProfilePolicy;

        public static string BaseApiRoute;

        public static string TenantId { get => $"{TenantName}.onmicrosoft.com"; }
        public static string AuthorityBase { get => $"https://{TenantName}.b2clogin.com/tfp/{TenantId}/"; }
        public static string AuthoritySignIn { get => $"{AuthorityBase}{SignInPolicy}"; }
        public static string AuthorityEdit  { get => $"{AuthorityBase}{EditProfilePolicy}";}
        public static string AccountEnvironment { get => $"{TenantName}.b2clogin.com"; }
    }
}
