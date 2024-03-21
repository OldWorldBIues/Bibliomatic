using Bibliomatic_MAUI_App.ViewModels;

namespace Bibliomatic_MAUI_App.Views;

public partial class CreationView : ContentPage
{
	public CreationView(CreationViewModel creationViewModel)
	{
		InitializeComponent();
		this.BindingContext = creationViewModel;
	}
}