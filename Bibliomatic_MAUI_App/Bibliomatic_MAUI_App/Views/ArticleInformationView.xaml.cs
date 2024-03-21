using Bibliomatic_MAUI_App.ViewModels;

namespace Bibliomatic_MAUI_App.Views;

public partial class ArticleInformationView : ContentPage
{
	public ArticleInformationView(ArticleInformationViewModel articleInformationViewModel)
	{
		InitializeComponent();
		this.BindingContext = articleInformationViewModel;
	}
}