using Bibliomatic_MAUI_App.Models;
using Bibliomatic_MAUI_App.Helpers;
using Bibliomatic_MAUI_App.Services;
using Bibliomatic_MAUI_App.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Alerts;

namespace Bibliomatic_MAUI_App.ViewModels
{
    [QueryProperty(nameof(Test), "Test")]
    public partial class CreatorTestInformationViewModel : ObservableObject
    {
        LoadingService loadingService = new LoadingService();
        FiltersManager filtersManager = new FiltersManager();

        private readonly IDataService<TestResponse> dataService;
        private readonly IRelatedDataService<TestResponse, TestLike> relatedDataLikeService;
        private readonly IRelatedDataService<TestResponse, TestDislike> relatedDataDislikeService;
        private readonly IRelatedDataService<TestResponse, TestComment> relatedDataCommentService;
        private readonly IRelatedDataService<TestResponse, UserScore> relatedDataUserScoresService;

        [ObservableProperty]
        public TestResponse test;       

        [ObservableProperty]
        public bool isRefreshing;

        [ObservableProperty]
        public int recordsOnPage = 5;

        public CreatorTestInformationViewModel(IDataService<TestResponse> dataService,
                                               IRelatedDataService<TestResponse, TestLike> relatedDataLikeService,
                                               IRelatedDataService<TestResponse, TestDislike> relatedDataDislikeService,
                                               IRelatedDataService<TestResponse, TestComment> relatedDataCommentService,
                                               IRelatedDataService<TestResponse, UserScore> relatedDataUserScoresService)
        {
            this.dataService = dataService;
            this.relatedDataLikeService = relatedDataLikeService;
            this.relatedDataDislikeService = relatedDataDislikeService;
            this.relatedDataCommentService = relatedDataCommentService;
            this.relatedDataUserScoresService = relatedDataUserScoresService;

            dataService.ControllerName = "tests";
            relatedDataLikeService.ControllerName = "tests";
            relatedDataDislikeService.ControllerName = "tests";
            relatedDataCommentService.ControllerName = "tests";
            relatedDataUserScoresService.ControllerName = "tests";
        }

        [RelayCommand]
        public async Task RefreshTestData()
        {
            IsRefreshing = false;

            await loadingService.PerformLoading("Refreshing test information", async () =>
            {
                var test = await dataService.GetData(Test.Id);
                FilesAttachmentsService.GetValidTestAttachments(test);

                test.AllTestCommentsCount = test.TestCommentsCount;
                test.AllTestUserScoresCount = test.TestUserScoresCount;

                test.AllTestComments = test.TestComments;
                test.AllTestUserScores = test.UserScores;

                Test = test;
            });
        }

        [RelayCommand]
        public async Task LoadMoreTestComments()
        {
            var allRecordsCollection = Test.AllTestComments;
            int skippedRecords = allRecordsCollection.Count;

            filtersManager.ClearCurrentFilters();
            filtersManager.AddOrderFilter("CreatedAt", OrderBy.Descending);
            string filter = filtersManager.CreateFiltersUrl();
            var testCommentsResponse = await relatedDataCommentService.GetAllRelatedDataById(Test.Id, "comments", skippedRecords, RecordsOnPage, filter);

            foreach (var testComment in testCommentsResponse)
            {
                allRecordsCollection.Add(testComment);
            }
        }

        [RelayCommand]
        public async Task LoadMoreUserScores()
        {
            var allRecordsCollection = Test.AllTestUserScores;
            int skippedRecords = allRecordsCollection.Count;

            filtersManager.ClearCurrentFilters();
            filtersManager.AddOrderFilter("TestEndDate", OrderBy.Descending);
            string filter = filtersManager.CreateFiltersUrl();

            var previousAttemptsResponse = await relatedDataUserScoresService.GetAllRelatedDataById(Test.Id, "userScores", skippedRecords, RecordsOnPage, filter);

            foreach (var previousAttempt in previousAttemptsResponse)
            {
                allRecordsCollection.Add(previousAttempt);
            }
        }

        [RelayCommand]
        public async Task ChangeLikes()
        {
            var userLike = Test.TestLikes.FirstOrDefault(al => al.UserId == CurrentUser.Id);

            try
            {
                if (userLike == null)
                {
                    userLike = new TestLike() { Id = Guid.NewGuid(), TestId = Test.Id, UserId = CurrentUser.Id };
                    await relatedDataLikeService.CreateRelatedData(Test.Id, userLike, "likes");
                    var userDislike = Test.TestDislikes.FirstOrDefault(al => al.UserId == CurrentUser.Id);

                    if (userDislike != null) await ChangeDislikesCommand.ExecuteAsync(null);

                    Test.TestLikes.Add(userLike);
                }
                else
                {
                    await relatedDataLikeService.DeleteRelatedData(Test.Id, userLike.Id, "likes");
                    Test.TestLikes.Remove(userLike);
                }
            }
            catch (Exception)
            {
                await Toast.Make("An error occurred while adding like. Please try again or later").Show();
            }
        }

        [RelayCommand]
        public async Task ChangeDislikes()
        {
            try
            {
                var userDislike = Test.TestDislikes.FirstOrDefault(al => al.UserId == CurrentUser.Id);

                if (userDislike == null)
                {
                    userDislike = new TestDislike() { Id = Guid.NewGuid(), TestId = Test.Id, UserId = CurrentUser.Id };
                    await relatedDataDislikeService.CreateRelatedData(Test.Id, userDislike, "dislikes");
                    var userLike = Test.TestLikes.FirstOrDefault(al => al.UserId == CurrentUser.Id);

                    if (userLike != null) await ChangeLikesCommand.ExecuteAsync(null);

                    Test.TestDislikes.Add(userDislike);
                }
                else
                {
                    await relatedDataDislikeService.DeleteRelatedData(Test.Id, userDislike.Id, "dislikes");
                    Test.TestDislikes.Remove(userDislike);
                }
            }
            catch (Exception)
            {
                await Toast.Make("An error occurred while adding dislike. Please try again or later").Show();
            }
        }

        [RelayCommand]
        public void StartAddingComment(StackLayout commentStackLayout) => commentStackLayout.IsVisible = true;

        [RelayCommand]
        public void CancelAddingComment(StackLayout commentStackLayout) => commentStackLayout.IsVisible = false;

        [RelayCommand]
        public async Task AddCommentToTest(StackLayout commentStackLayout)
        {
            try
            {
                if (string.IsNullOrEmpty(Test.TestComment)) return;

                var comment = new TestComment()
                {
                    Id = Guid.NewGuid(),
                    TestId = Test.Id,
                    UserId = CurrentUser.Id,
                    Author = CurrentUser.FullName,
                    Comment = Test.TestComment,
                    CreatedAt = DateTimeOffset.Now
                };

                await relatedDataCommentService.CreateRelatedData(Test.Id, comment, "comments");
                CancelAddingCommentCommand.Execute(commentStackLayout);

                Test.TestComment = string.Empty;
                Test.AllTestComments.Insert(0, comment);

                Test.TestCommentsCount++;
                Test.AllTestCommentsCount++;

                await Toast.Make("Comment was successfully added").Show();
            }
            catch (Exception)
            {
                await Toast.Make("An error occurred while adding comment. Please try again or later").Show();
            }
        }

        [RelayCommand]
        public async Task RemoveCommentFromTest(TestComment comment)
        {
            try
            {
                bool deleteAction = await Application.Current.MainPage.DisplayAlert("Delete comment", "Are you sure you want to delete this comment?", "Yes", "Cancel");

                if (!deleteAction) return;

                await relatedDataCommentService.DeleteRelatedData(Test.Id, comment.Id, "comments");
                Test.AllTestComments.Remove(comment);

                Test.TestCommentsCount--;
                Test.AllTestCommentsCount--;

                await Toast.Make("Comment was successfully deleted").Show();
            }
            catch (Exception)
            {
                await Toast.Make("An error occurred while deleting comment. Please try again or later").Show();
            }
        }

        [RelayCommand]
        public async Task NavigateToTestView()
        {
            await loadingService.PerformLoading("Loading test...", async () =>
            {                
                await Shell.Current.GoToAsync($"{nameof(CreatorTestView)}", true, new Dictionary<string, object>
                {                    
                    {"Test", Test}
                });
            });
        }
    }
}
