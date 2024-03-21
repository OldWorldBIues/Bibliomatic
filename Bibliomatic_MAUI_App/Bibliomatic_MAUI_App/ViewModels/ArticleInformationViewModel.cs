using Bibliomatic_MAUI_App.Models;
using Bibliomatic_MAUI_App.Models.AttachmentInfo;
using Bibliomatic_MAUI_App.Views;
using Bibliomatic_MAUI_App.Services;
using Bibliomatic_MAUI_App.CustomControls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Views;
using Syncfusion.Maui.PdfToImageConverter;
using Bibliomatic_MAUI_App.Helpers;

namespace Bibliomatic_MAUI_App.ViewModels
{
    [QueryProperty(nameof(Article), "Article")]
    [QueryProperty(nameof(IsNewArticle), "IsNewArticle")]
    public partial class ArticleInformationViewModel : ObservableObject, IQueryAttributable
    {        
        LoadingService loadingService = new LoadingService();

        private readonly ILocalFileKeeperService localFileKeeperService;
        private readonly IFileService fileService;
        private readonly IDataService<ArticleResponce> dataService;
        private PdfToImageConverter pdfToImageConverter;

        [ObservableProperty]
        public ArticleResponce article;

        [ObservableProperty]
        public string publishError;

        [ObservableProperty]
        public bool publishErrorVisible;

        private bool IsNewArticle { get; set; }       
        public string ArticleDocumentSourceBeforeEdit { get; set; }        
        public string ArticleImageSourceBeforeEdit { get; set; }       

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Article = (ArticleResponce)query["Article"];
            IsNewArticle = (bool)query["IsNewArticle"];

            localFileKeeperService.BaseDirectory = $"{Article.Id}";
            ArticleDocumentSourceBeforeEdit = Article.ArticleDocumentSource;
            ArticleImageSourceBeforeEdit = Article.ArticleImageSource;           
        }

        public ArticleInformationViewModel(ILocalFileKeeperService localFileKeeperService, IFileService fileService, IDataService<ArticleResponce> dataService)
        {            
            this.localFileKeeperService = localFileKeeperService;
            this.fileService = fileService;

            this.dataService = dataService;
            dataService.ControllerName = "articles";
        }

        [RelayCommand]
        public void RemoveAttachedDocument()
        {
            Article.ArticleImageSource = "attach_element.png";
            Article.ArticleImagePath = "attach_element.png";

            Article.ArticleDocumentSource = string.Empty;
            Article.ArticleDocumentPath = string.Empty;
        }

        [RelayCommand]
        public async Task ReturnBack()
        {
            bool deleteAction = await Application.Current.MainPage.DisplayAlert("Return back", "Are you sure you want to go back? All unsaved data will be lost", "Yes", "Cancel");

            if (!deleteAction) return;

            localFileKeeperService.DeleteAllFilesInBaseDirectory();
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task AttachFile()
        {
            bool permissionsGranted = await PermissionsChecker.CheckForRequiredPermissions();
            if (!permissionsGranted) return;

            var fileResult = await FilePicker.PickAsync();

            if (fileResult == null) return;
            if (!fileResult.FileName.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
            {
                await Application.Current.MainPage.DisplayAlert("Wrong file extension", "You can only attach files with PDF extension to article. Please select another file to continue", "Got it");
                return;
            }

            var pdfStream = File.OpenRead(fileResult.FullPath);
            var imageStream = await GetImageFromPdf(pdfStream);
            
            Article.ArticleDocumentPath = await localFileKeeperService.SaveFileToLocalStorage(fileResult);
            Article.ArticleDocumentSource = Path.GetRelativePath(localFileKeeperService.LocalAppDataPath, Article.ArticleDocumentPath);

            Article.ArticleImagePath = await localFileKeeperService.SaveFileToLocalStorage(imageStream);
            Article.ArticleImageSource = Path.GetRelativePath(localFileKeeperService.LocalAppDataPath, Article.ArticleImagePath);
        }

        [RelayCommand]
        public async Task NavigateToArticlePreview()
        {
            await loadingService.PerformLoading("Loading article preview...", async () =>
            {
                await Shell.Current.GoToAsync($"{nameof(ArticleDocumentView)}", true, new Dictionary<string, object>
                {
                    {"Article", Article},
                    {"IsNewArticle", IsNewArticle}
                });
            });            
        }

        private async Task<Stream> GetImageFromPdf(Stream pdfStream)
        {
            pdfToImageConverter = new PdfToImageConverter(pdfStream);
            return await pdfToImageConverter.ConvertAsync(0, new Size(350, 350));            
        }

        [RelayCommand]
        public async Task CheckForUnfilledData()
        {
            string errorMessage = string.Empty;

            if (string.IsNullOrEmpty(Article.Title))
            {
                errorMessage += "You need to fill article title";
            }

            if (string.IsNullOrEmpty(Article.Description))
            {
                errorMessage += string.IsNullOrEmpty(errorMessage) ? "You need to fill article description" : ", article description";
            }

            if (string.IsNullOrEmpty(Article.ArticleDocumentPath))
            {
                errorMessage += string.IsNullOrEmpty(errorMessage) ? "You need to attach article document" : " and attach article document";
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                errorMessage += " before publish";                
                PublishError = errorMessage;
                PublishErrorVisible = true;                
            }
            else
            {
                PublishErrorVisible = false;
                await PublishArticle();
            }
        }

        [RelayCommand]
        public async Task PublishArticle()
        {
            var files = new List<string>();

            if (Article.ArticleDocumentSource != ArticleDocumentSourceBeforeEdit)
            {
                files.Add(Article.ArticleDocumentPath);
                files.Add(Article.ArticleImagePath);
            }
           
            await loadingService.PerformLoadingWithFilesUpload("Publishing article...", "Back to main", files, async (filesToUpload, loadedFiles) =>
            {
                var newFiles = new List<AttachmentDTOResponse>();

                if (filesToUpload.Count > 0)
                {
                    newFiles = await fileService.UploadFiles(filesToUpload, localFileKeeperService.LocalAppDataPath);                   
                }

                FilesAttachmentsService.AttachArticleFiles(Article, newFiles.Union(loadedFiles).ToList());
                localFileKeeperService.DeleteAllFilesInBaseDirectory();                

                if (IsNewArticle)
                {
                    await dataService.CreateData(Article);                    
                }
                else
                {
                    if (Article.ArticleDocumentSource != ArticleDocumentSourceBeforeEdit)
                    {
                        await fileService.DeleteFiles(ArticleDocumentSourceBeforeEdit, ArticleImageSourceBeforeEdit);
                    }

                    await dataService.UpdateData(Article.Id, Article);
                }
            });
        }
    }
}
