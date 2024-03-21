using Bibliomatic_MAUI_App.ViewModels;

namespace Bibliomatic_MAUI_App.Views;
public partial class ArticleView : ContentPage
{  
	public ArticleView(ArticlesViewModel articlesViewModel)
	{
		InitializeComponent();
		this.BindingContext = articlesViewModel;
	}   
}