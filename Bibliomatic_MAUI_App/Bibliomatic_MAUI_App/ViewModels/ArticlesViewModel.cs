using Bibliomatic_MAUI_App.Models;
using Bibliomatic_MAUI_App.Services;
using Bibliomatic_MAUI_App.Helpers;
using Bibliomatic_MAUI_App.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Bibliomatic_MAUI_App.ViewModels
{  
    public partial class ArticlesViewModel : ObservableObject
    {       
        LoadingService loadingService = new LoadingService();       
        FiltersManager filtersManager = new FiltersManager();

        private int currentPageNumber = 1;
        private readonly int currentPageSize = 20;

        public ObservableCollection<ArticleResponce> ArticleList { get; set; }
        private List<ArticleResponce> allArticlesList;
        
        private readonly IDataService<ArticleResponce> articleService;
        private readonly IFileService fileService;

        [ObservableProperty]
        public int remainingItemsThreshold = 3;

        [ObservableProperty]
        public bool isBusy;

        [ObservableProperty]
        public bool isLoading;

        [ObservableProperty]
        public bool isRefreshing;

        [ObservableProperty]
        public bool isFiltering;

        [ObservableProperty]
        public bool failedLayoutVisible;

        [ObservableProperty]
        public bool noContentLayoutVisible;

        [ObservableProperty]
        public string authorFilter;

        [ObservableProperty]
        public string titleFilter;

        [ObservableProperty]
        public bool showNewArticlesFirstFilter = false;

        [ObservableProperty]
        public bool showOldArticlesFirstFilter = true;

        public ArticlesViewModel(IDataService<ArticleResponce> articleService, IFileService fileService)
        {
            this.fileService = fileService;
            this.articleService = articleService;
            articleService.ControllerName = "articles";

            ArticleList = new ObservableCollection<ArticleResponce>();
            LoadArticleData();           
        }

        private async Task FillArticlesCollectionData()
        {
            try
            {
                FailedLayoutVisible = false;
                NoContentLayoutVisible = false;

                string dataFilter = filtersManager.CreateFiltersUrl();
                allArticlesList = await articleService.GetAllData(currentPageNumber, currentPageSize, dataFilter);

                if (allArticlesList?.Count > 0)
                {
                    foreach (var article in allArticlesList)
                    {
                        ArticleList.Add(article);
                    }

                    if (allArticlesList.Count < currentPageSize)
                        RemainingItemsThreshold = -1;

                    currentPageNumber++;
                }
                else
                {
                    if (currentPageNumber == 1)
                    {
                        NoContentLayoutVisible = true;
                    }
                }
            }
            catch(Exception)
            {
                FailedLayoutVisible = true;
            }
        }
        
        private void LoadArticleData()
        {         
            IsBusy = true;

            Task.Run(async() =>
            {
                filtersManager.ClearCurrentFilters();
                filtersManager.AddOrderFilter("UpdatedAt", OrderBy.Ascending);
                await FillArticlesCollectionData();

                IsBusy = false;
            });           
        }
        

        [RelayCommand]
        public async Task LoadMoreArticleData()
        {
            if (IsLoading || IsBusy || IsRefreshing || IsFiltering) return;

            IsLoading = true;
            await FillArticlesCollectionData();
            IsLoading = false;
        }

        [RelayCommand]
        public async Task RefreshArticlesData()
        {            
            ArticleList.Clear();            
            currentPageNumber = 1;            

            IsRefreshing = true;
            await FillArticlesCollectionData();
            IsRefreshing = false;
        }

        [RelayCommand]
        public async Task NavigateToArticlePreview(ArticleResponce articleResponseDto)
        {
            await loadingService.PerformLoading("Loading article...", async () =>
            {                
                var article = await articleService.GetData(articleResponseDto.Id);
                article.AllArticlesCommentsCount = article.ArticleCommentsCount;
                article.AllArticleComments = article.ArticleComments;

                FilesAttachmentsService.GetValidArticlesAttachments(article);               
                await Shell.Current.Navigation.PopToRootAsync(false);
                await Shell.Current.GoToAsync($"{nameof(ArticleDocumentView)}", true, new Dictionary<string, object>
                {
                     {"Article", article},
                     {"IsNewArticle", false}
                });
            });
        }

        [RelayCommand]
        public async Task FilterData()
        {        
            ArticleList.Clear();
            currentPageNumber = 1;

            IsFiltering = true;
            filtersManager.ClearCurrentFilters();

            var orderBy = ShowNewArticlesFirstFilter ? OrderBy.Descending : OrderBy.Ascending;

            filtersManager.AddStringDataFilter("Author", AuthorFilter, true);
            filtersManager.AddStringDataFilter("Title", TitleFilter, true);
            filtersManager.AddOrderFilter("UpdatedAt", orderBy);
            
            await FillArticlesCollectionData();
            IsFiltering = false;
        }        

        [RelayCommand]
        public void ClearFilters()
        {
            AuthorFilter = string.Empty;
            TitleFilter = string.Empty;

            ShowNewArticlesFirstFilter = false;
            ShowOldArticlesFirstFilter = true;
        }
    }
}
