using Bibliomatic_MAUI_App.ViewModels;

namespace Bibliomatic_MAUI_App.Views;

public partial class QuestionDetailsView : ContentPage
{
	public QuestionDetailsView(QuestionDetailsViewModel questionDetailsViewModel)
	{
		InitializeComponent();
		this.BindingContext = questionDetailsViewModel;        
	}
   
    private void OnNavigating(object sender, WebNavigatingEventArgs e)
    {
        e.Cancel = true;
        Launcher.TryOpenAsync(e.Url);
    }

    private async void ScrollToNewAnswerButton_Clicked(object sender, EventArgs e)
    {
        await ContentScrollView.ScrollToAsync(AnswersCollectionView, ScrollToPosition.Start, true);
        var viewModel = (QuestionDetailsViewModel)BindingContext;                
        viewModel.ScrollToAnswer = false;
    }

    private void HideScrollButton_Clicked(object sender, EventArgs e)
    {
        var viewModel = (QuestionDetailsViewModel)BindingContext;
        viewModel.ScrollToAnswer = false;
    }
}