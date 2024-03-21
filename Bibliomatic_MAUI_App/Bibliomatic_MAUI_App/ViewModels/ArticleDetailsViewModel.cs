using Bibliomatic_MAUI_App.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Syncfusion.Maui.PdfViewer;

namespace Bibliomatic_MAUI_App.ViewModels
{    
    public partial class ArticleDetailsViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        public Stream pdfDocumentStream;

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var article = (ArticleResponce)query["Article"];
            bool isNewArticle = (bool)query["IsNewArticle"];

            var stream = await GetArticleDocumentContent(article.ArticleDocumentPath, isNewArticle);
            PdfDocumentStream = stream;
        }

        private async Task<Stream> GetArticleDocumentContent(string path, bool isNewArticle)
        {
            if(isNewArticle)
            {
                return File.OpenRead(path);
            }
            else
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(path);

                return await response.Content.ReadAsStreamAsync();
            }
        }

        [RelayCommand]
        public async Task NavigateBackToArticle(SfPdfViewer sfPdfViewer)
        {
            await sfPdfViewer.UnloadDocumentAsync();
            await Shell.Current.GoToAsync("..");
        }
    }
}
