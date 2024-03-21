using Bibliomatic_MAUI_App.ViewModels;

namespace Bibliomatic_MAUI_App.Views;

public partial class TestResultView : ContentPage
{
	public TestResultView(TestResultViewModel testResultViewModel)
	{
		InitializeComponent();
		this.BindingContext = testResultViewModel;
	}
}