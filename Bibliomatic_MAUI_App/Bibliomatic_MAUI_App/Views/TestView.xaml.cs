using Bibliomatic_MAUI_App.ViewModels;

namespace Bibliomatic_MAUI_App.Views;

public partial class TestView : ContentPage
{
	public TestView(TestViewModel testViewModel)
	{
		InitializeComponent();
		this.BindingContext = testViewModel;
	}
}