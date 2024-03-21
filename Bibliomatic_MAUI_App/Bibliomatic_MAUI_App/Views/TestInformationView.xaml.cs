using Bibliomatic_MAUI_App.ViewModels;

namespace Bibliomatic_MAUI_App.Views;

public partial class TestInformationView : ContentPage
{
	public TestInformationView(TestInformationViewModel testInformationViewModel)
	{
		InitializeComponent();
		this.BindingContext = testInformationViewModel;
	}
}