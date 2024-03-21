using Bibliomatic_MAUI_App.ViewModels;

namespace Bibliomatic_MAUI_App.Views;

public partial class TestsView : ContentPage
{
	public TestsView(TestsViewModel testsViewModel)
	{
		InitializeComponent();
		this.BindingContext = testsViewModel;
	}
}