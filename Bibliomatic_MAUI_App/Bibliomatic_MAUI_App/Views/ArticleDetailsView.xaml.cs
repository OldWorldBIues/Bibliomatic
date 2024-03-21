using Bibliomatic_MAUI_App.ViewModels;

namespace Bibliomatic_MAUI_App.Views;

public partial class ArticleDetailsView : ContentPage
{
	public ArticleDetailsView(ArticleDetailsViewModel articleDetailsViewModel)
	{
		InitializeComponent();
		this.BindingContext = articleDetailsViewModel;          
	}    
}