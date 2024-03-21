using Bibliomatic_MAUI_App.Views;

namespace Bibliomatic_MAUI_App
{
    public partial class App : Application
    {
        public App(AuthenticationView authenticationView)
        {
            InitializeComponent();
            MainPage = new NavigationPage(authenticationView);
        }
    }
}