using Bibliomatic_MAUI_App.Models;
using Bibliomatic_MAUI_App.Views;
using Bibliomatic_MAUI_App.Services;
using Bibliomatic_MAUI_App.Helpers;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Maui.Alerts;

namespace Bibliomatic_MAUI_App.ViewModels
{
    [QueryProperty(nameof(Article), "Article")]
    [QueryProperty(nameof(IsNewArticle), "IsNewArticle")]
    public partial class ArticleDocumentViewModel : ObservableObject
    {
        LoadingService loadingService = new LoadingService();
        FiltersManager filtersManager = new FiltersManager();        

        private readonly IFileService fileService;
        private readonly IDataService<ArticleResponce> dataService;
        private readonly IRelatedDataService<ArticleResponce, ArticleLike> relatedDataLikeService;
        private readonly IRelatedDataService<ArticleResponce, ArticleDislike> relatedDataDislikeService;
        private readonly IRelatedDataService<ArticleResponce, ArticleComment> relatedDataCommentService;        

        [ObservableProperty]
        public ArticleResponce article;

        [ObservableProperty]
        public bool isNewArticle;

        [ObservableProperty]
        public int recordsOnPage = 5;

        [ObservableProperty]
        public bool isRefreshing;

        public ArticleDocumentViewModel(IDataService<ArticleResponce> dataService, IFileService fileService, 
                                        IRelatedDataService<ArticleResponce, ArticleLike> relatedDataLikeService,
                                        IRelatedDataService<ArticleResponce, ArticleDislike> relatedDataDislikeService, 
                                        IRelatedDataService<ArticleResponce, ArticleComment> relatedDataCommentService)
        {
            this.fileService = fileService;
            this.relatedDataLikeService = relatedDataLikeService;
            this.relatedDataDislikeService = relatedDataDislikeService;
            this.relatedDataCommentService = relatedDataCommentService;
            this.dataService = dataService;

            relatedDataLikeService.ControllerName = "articles";
            relatedDataDislikeService.ControllerName = "articles";
            relatedDataCommentService.ControllerName = "articles";
            dataService.ControllerName = "articles";           
        }

        [RelayCommand]
        public async Task RefreshArticleData()
        {
            IsRefreshing = false;

            await loadingService.PerformLoading("Refreshing article...", async () =>
            {
                var article = await dataService.GetData(Article.Id);
                FilesAttachmentsService.GetValidArticlesAttachments(article);

                article.AllArticlesCommentsCount = article.ArticleCommentsCount;
                article.AllArticleComments = article.ArticleComments;

                Article = article;
            });
        }

        [RelayCommand]
        public async Task LoadMoreArticleComments()
        {            
            var allRecordsCollection = Article.AllArticleComments;
            int skippedRecords = allRecordsCollection.Count;

            filtersManager.AddOrderFilter("CreatedAt", OrderBy.Descending);
            string filter = filtersManager.CreateFiltersUrl();            
            var articleCommentsResponse = await relatedDataCommentService.GetAllRelatedDataById(Article.Id, "comments", skippedRecords, RecordsOnPage, filter);

            foreach (var articleComment in articleCommentsResponse)
            {                
                allRecordsCollection.Add(articleComment);
            }
        }

        [RelayCommand]
        public async Task NavigateToArticleDetailsView()
        {            
            if(IsNewArticle && string.IsNullOrEmpty(Article.ArticleDocumentPath))
            {
                await Application.Current.MainPage.DisplayAlert("Document is not attached", "You cant preview document, because its not attached. Return to article creation page to set document and come back to preview it", "Got it");
                return;
            }

            await loadingService.PerformLoading("Loading article document...", async () =>
            {
                await Shell.Current.GoToAsync($"{nameof(ArticleDetailsView)}", true, new Dictionary<string, object>
                {
                     {"Article", Article},
                     {"IsNewArticle", IsNewArticle}
                });
            });
        }      

        [RelayCommand]
        public async Task ChangeLikes()
        {
            try
            {
                var userLike = Article.ArticleLikes.FirstOrDefault(al => al.UserId == CurrentUser.Id);

                if (userLike == null)
                {
                    userLike = new ArticleLike() { Id = Guid.NewGuid(), ArticleId = Article.Id, UserId = CurrentUser.Id };
                    await relatedDataLikeService.CreateRelatedData(Article.Id, userLike, "likes");                    
                    var userDislike = Article.ArticleDislikes.FirstOrDefault(al => al.UserId == CurrentUser.Id);

                    if (userDislike != null) await ChangeDislikesCommand.ExecuteAsync(null);
                    
                    Article.ArticleLikes.Add(userLike);
                }
                else
                {
                    await relatedDataLikeService.DeleteRelatedData(Article.Id, userLike.Id, "likes");
                    Article.ArticleLikes.Remove(userLike);
                }
            }
            catch(Exception)
            {
                await Toast.Make("An error occurred while adding like. Please try again or later").Show();
            }
        }

        [RelayCommand]
        public async Task ChangeDislikes()
        {
            try
            {
                var userDislike = Article.ArticleDislikes.FirstOrDefault(al => al.UserId == CurrentUser.Id);

                if (userDislike == null)
                {
                    userDislike = new ArticleDislike() { Id = Guid.NewGuid(), ArticleId = Article.Id, UserId = CurrentUser.Id };           
                    await relatedDataDislikeService.CreateRelatedData(Article.Id, userDislike, "dislikes");
                    var userLike = Article.ArticleLikes.FirstOrDefault(al => al.UserId == CurrentUser.Id);

                    if (userLike != null) await ChangeLikesCommand.ExecuteAsync(null);                    

                    Article.ArticleDislikes.Add(userDislike);
                }
                else
                {
                    await relatedDataDislikeService.DeleteRelatedData(Article.Id, userDislike.Id, "dislikes");
                    Article.ArticleDislikes.Remove(userDislike);
                }
            }
            catch(Exception)
            {
                await Toast.Make("An error occurred while adding dislike. Please try again or later").Show();
            }
        }

        [RelayCommand]
        public void StartAddingComment(StackLayout commentStackLayout) => commentStackLayout.IsVisible = true;

        [RelayCommand]
        public void CancelAddingComment(StackLayout commentStackLayout) => commentStackLayout.IsVisible = false;

        [RelayCommand]
        public async Task AddCommentToArticle(StackLayout commentStackLayout)
        {
            try
            {
                if (string.IsNullOrEmpty(Article.ArticleComment)) return;

                var comment = new ArticleComment() 
                { 
                    Id = Guid.NewGuid(),
                    ArticleId = Article.Id, 
                    UserId = CurrentUser.Id, 
                    Author = CurrentUser.FullName,
                    Comment = Article.ArticleComment, 
                    CreatedAt = DateTimeOffset.Now 
                };
                
                await relatedDataCommentService.CreateRelatedData(Article.Id, comment, "comments");
                CancelAddingCommentCommand.Execute(commentStackLayout);

                Article.ArticleComment = string.Empty;
                Article.AllArticleComments.Insert(0, comment);

                Article.ArticleCommentsCount++;
                Article.AllArticlesCommentsCount++;

                await Toast.Make("Comment was successfully added").Show();
            }
            catch(Exception)
            {
                await Toast.Make("An error occurred while adding comment. Please try again or later").Show();
            }
        }

        [RelayCommand]
        public async Task RemoveCommentFromArticle(ArticleComment comment)
        {
            try
            {
                bool deleteAction = await Application.Current.MainPage.DisplayAlert("Delete comment", "Are you sure you want to delete this comment?", "Yes", "Cancel");

                if (!deleteAction) return;

                await relatedDataCommentService.DeleteRelatedData(Article.Id, comment.Id, "comments");
                Article.AllArticleComments.Remove(comment);

                Article.ArticleCommentsCount--;
                Article.AllArticlesCommentsCount--;

                await Toast.Make("Comment was successfully deleted").Show();
            }
            catch (Exception)
            {
                await Toast.Make("An error occurred while deleting comment. Please try again or later").Show();
            }
        }
    }
}
