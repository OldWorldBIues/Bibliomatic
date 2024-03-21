using Bibliomatic_MAUI_App.ViewModels;

namespace Bibliomatic_MAUI_App.Views;

public partial class QuestionsView : ContentPage
{
	public QuestionsView(QuestionsViewModel questionsViewModel)
	{
		InitializeComponent();
		this.BindingContext = questionsViewModel;
	}
}