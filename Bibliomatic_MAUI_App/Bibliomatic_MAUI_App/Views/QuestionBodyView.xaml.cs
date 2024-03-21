using Bibliomatic_MAUI_App.ViewModels;

namespace Bibliomatic_MAUI_App.Views;

public partial class QuestionBodyView : ContentPage
{
	public QuestionBodyView(QuestionBodyViewModel questionBodyViewModel)
	{
		InitializeComponent();
		this.BindingContext = questionBodyViewModel;		
	}

	private void OnNavigating(object sender, WebNavigatingEventArgs e)
	{
		e.Cancel = true;
		Launcher.TryOpenAsync(e.Url);		
	}
}