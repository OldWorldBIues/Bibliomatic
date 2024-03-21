using Bibliomatic_MAUI_App.ViewModels;

namespace Bibliomatic_MAUI_App.Views;

public partial class CreatorTestView : ContentPage
{
	public CreatorTestView(CreatorTestViewModel creatorTestViewModel)
	{
		InitializeComponent();
		this.BindingContext = creatorTestViewModel;
	}
}