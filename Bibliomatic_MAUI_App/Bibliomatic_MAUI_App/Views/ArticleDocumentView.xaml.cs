using Bibliomatic_MAUI_App.ViewModels;

namespace Bibliomatic_MAUI_App.Views;

public partial class ArticleDocumentView : ContentPage
{
	public ArticleDocumentView(ArticleDocumentViewModel articleDocumentViewModel)
	{
		InitializeComponent();
		this.BindingContext = articleDocumentViewModel;
	}    
}