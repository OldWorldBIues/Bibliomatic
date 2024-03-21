using Bibliomatic_MAUI_App.ViewModels;

namespace Bibliomatic_MAUI_App.Views;

public partial class CreatorTestInformationView : ContentPage
{
	public CreatorTestInformationView(CreatorTestInformationViewModel creatorTestInformationViewModel)
	{
		InitializeComponent();
		this.BindingContext = creatorTestInformationViewModel;
	}
}