using Bibliomatic_MAUI_App.Helpers;
using Bibliomatic_MAUI_App.Models;
using Bibliomatic_MAUI_App.Services;
using Bibliomatic_MAUI_App.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bibliomatic_MAUI_App.ViewModels
{
    [QueryProperty(nameof(Test), "Test")]    
    public partial class TestInformationViewModel : ObservableObject
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
        
        public TestInformationViewModel(IDataService<TestResponse> dataService, 
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

            await loadingService.PerformLoading("Refreshing test information...", async () =>
            {
                string additionalQueryParameters = $"?userId={CurrentUser.Id}";
                var test = await dataService.GetData(Test.Id, additionalQueryParameters);

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
        public async Task LoadMoreUserScoresForCurrentUser()
        {
            var allRecordsCollection = Test.AllTestUserScores;
            int skippedRecords = allRecordsCollection.Count;

            filtersManager.ClearCurrentFilters();
            filtersManager.AddDefaultDataFilter(nameof(Test.UserId), CurrentUser.Id, true);
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
            catch(Exception)
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
            catch(Exception)
            {
                await Toast.Make("An error occurred while deleting comment. Please try again or later").Show();
            }
        }

        private void ShuffleList<T>(IList<T> list) where T : class
        {
            var random = new Random();            
            
            for(int i = list.Count - 1; i > 1; i--)
            {
                int randomListIndex = random.Next(i + 1);

                var listElement = list[randomListIndex];
                list[randomListIndex] = list[i];
                list[i] = listElement;
            }           
        }

        private void ShuffleTestElementsAndUpdateIndexes(TestResponse test)
        {
            int indexOfTestQuestion = 1;
            ShuffleList(test.TestQuestions);            

            foreach(var testQuestion in test.TestQuestions)
            {
                int indexOfTestAnswer = 1;
                testQuestion.QuestionNumber = indexOfTestQuestion++;
                ShuffleList(testQuestion.TestAnswers);    
                
                foreach (var testAnswer in testQuestion.TestAnswers)
                {
                    testAnswer.AnswerNumber = indexOfTestAnswer++;
                }
            }
        }

        [RelayCommand]
        public async Task NavigateToTestView()
        {
            await loadingService.PerformLoading("Loading test...", async () =>
            {
                var userScore = new UserScore()
                {
                    Id = Guid.NewGuid(),
                    TestId = Test.Id,
                    User = $"{CurrentUser.LastName} {CurrentUser.FirstName}",
                    UserId = CurrentUser.Id,
                    PointsForTest = 0,
                    TestStartDate = DateTimeOffset.Now,
                    TestEndDate = DateTimeOffset.Now,
                };

                ShuffleTestElementsAndUpdateIndexes(Test);

                await Shell.Current.GoToAsync($"{nameof(TestView)}", true, new Dictionary<string, object>
                {                    
                    {"Test", Test},
                    {"UserScore", userScore}                    
                });
            });            
        }
    }
}
