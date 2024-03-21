using Bibliomatic_MAUI_App.ViewModels;

namespace Bibliomatic_MAUI_App.Views;

public partial class AuthenticationView : ContentPage
{
	public AuthenticationView(AuthenticationViewModel authenticationViewModel)
	{
		InitializeComponent();
		this.BindingContext = authenticationViewModel;
	}
}