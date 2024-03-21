using Bibliomatic_MAUI_App.ViewModels;

namespace Bibliomatic_MAUI_App.Views;

public partial class QuestionView : ContentPage
{  
	
	public QuestionView(QuestionViewModel questionViewModel)
	{
		InitializeComponent();
		this.BindingContext = questionViewModel;		
	}   
}