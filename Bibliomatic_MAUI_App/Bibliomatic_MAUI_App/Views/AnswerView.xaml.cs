using Bibliomatic_MAUI_App.ViewModels;

namespace Bibliomatic_MAUI_App.Views;

public partial class AnswerView : ContentPage
{
	public AnswerView(AnswerViewModel answerViewModel)
	{
		InitializeComponent();
		this.BindingContext = answerViewModel;		
	}
}