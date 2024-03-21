using Bibliomatic_MAUI_App.Models;
using Bibliomatic_MAUI_App.Services;
using Bibliomatic_MAUI_App.Services.AzureB2CAuthService;
using Bibliomatic_MAUI_App.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bibliomatic_MAUI_App.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        LoadingService loadingService = new LoadingService();

        private readonly IUserDataService userDataRequestService;
        private readonly IDataService<ArticleResponce> articlesDataRequestService;
        private readonly IDataService<TestResponse> testsDataRequestService;
        private readonly IDataService<BaseQuestionResponce> baseQuestionsDataRequestService;
        private readonly IAuthService authService;
        private readonly IFileService fileService;

        [ObservableProperty]
        public Guid userId;

        [ObservableProperty]
        public Author author;

        [ObservableProperty]
        public UserData userData;

        [ObservableProperty]
        public string userName;

        [ObservableProperty]
        public string userEmail;
        
        [ObservableProperty]
        public string avatarText;

        [ObservableProperty]
        public bool failedLayoutVisible;

        [ObservableProperty]
        public bool noContentLayoutVisible;

        [ObservableProperty]
        public bool isRefreshing;

        [ObservableProperty]
        public bool isBusy;        

        [ObservableProperty]
        public bool canEditProfile = false;

        [ObservableProperty]
        public bool articlesButtonEnabled = false;

        [ObservableProperty]
        public bool testsButtonEnabled = false;

        [ObservableProperty]
        public bool questionsButtonEnabled = false;        

        public ProfileViewModel(IUserDataService userDataRequestService, IAuthService authService, IFileService fileService, IDataService<ArticleResponce> articlesDataRequestService, 
                                IDataService<TestResponse> testsDataRequestService, IDataService<BaseQuestionResponce> baseQuestionsDataRequestService)
        {
            this.userDataRequestService = userDataRequestService;
            this.articlesDataRequestService = articlesDataRequestService;
            this.testsDataRequestService = testsDataRequestService;
            this.baseQuestionsDataRequestService = baseQuestionsDataRequestService;
            this.authService = authService;
            this.fileService = fileService;

            userDataRequestService.ControllerName = "userData";
            articlesDataRequestService.ControllerName = "articles";
            testsDataRequestService.ControllerName = "tests";
            baseQuestionsDataRequestService.ControllerName = "questions";

            GetUserData();       
        }       

        [RelayCommand]
        public void SetArticlesCollectionAsItemSource()
        {
            NoContentLayoutVisible = false;
            ArticlesButtonEnabled = false;
            TestsButtonEnabled = true;
            QuestionsButtonEnabled = true;

            if(!UserData.Articles.Any())
            {
                NoContentLayoutVisible = true;
            }
        }

        [RelayCommand]
        public void SetTestsCollectionAsItemSource()
        {
            NoContentLayoutVisible = false;
            ArticlesButtonEnabled = true;
            TestsButtonEnabled = false;
            QuestionsButtonEnabled = true;

            if (!UserData.Tests.Any())
            {
                NoContentLayoutVisible = true;
            }
        }

        [RelayCommand]
        public void SetQuestionsCollectionAsItemSource()
        {
            NoContentLayoutVisible = false;
            ArticlesButtonEnabled = true;
            TestsButtonEnabled = true;
            QuestionsButtonEnabled = false;

            if (!UserData.Questions.Any())
            {
                NoContentLayoutVisible = true;
            }
        }

        private async Task FillUserDataCollection()
        {
            try
            {
                FailedLayoutVisible = false;
                UserData = await userDataRequestService.GetUserDataById(UserId);
            }
            catch (Exception)
            {
                IsBusy = false;
                FailedLayoutVisible = true;
            }
        }

        [RelayCommand]
        public async Task RefreshUserData()
        {            
            IsRefreshing = true;

            UserData.Clear();
            await FillUserDataCollection();

            IsRefreshing = false;

            if (!ArticlesButtonEnabled) SetArticlesCollectionAsItemSource();
            else if (!TestsButtonEnabled) SetTestsCollectionAsItemSource();
            else SetQuestionsCollectionAsItemSource();           
        }
        
        private void GetUserData()
        {
            IsBusy = true;

            Task.Run(async () =>
            {
                UserId = CurrentUser.Id;
                UserName = CurrentUser.FullName;
                UserEmail = CurrentUser.Email;
                AvatarText = UserName[0].ToString();
                CanEditProfile = true;
                Author = new Author
                {
                    Id = UserId,
                    FirstName = CurrentUser.FirstName,
                    LastName = CurrentUser.LastName,
                    MiddleName = CurrentUser.MiddleName
                };

                await FillUserDataCollection();
                SetArticlesCollectionAsItemSource();

                IsBusy = false;
            });
           
        }

        [RelayCommand]
        public async Task Logout()
        {
            await authService.SignOutAsync();

            TokenService.RemoveAuthorizationHeader();
            Application.Current.MainPage = new NavigationPage(new AuthenticationView(new AuthenticationViewModel(authService)));
        }
        
        public async Task SaveEditChanges()
        {
            await loadingService.PerformLoading("Updating profile..", async () =>
            {
                Author = await userDataRequestService.UpdateUser(Author.Id, Author);

                CurrentUser.FirstName = Author.FirstName;
                CurrentUser.LastName = Author.LastName;
                CurrentUser.MiddleName = Author.MiddleName;
               
                UserName = CurrentUser.FullName;
                AvatarText = UserName[0].ToString();

                await authService.SignOutAsync();
            });

            string successMessage = $"Profile was successfully changed";
            await Toast.Make(successMessage, ToastDuration.Long).Show();
        }
        
        [RelayCommand]
        public async Task NavigateToArticlePreview(ArticleResponce articleResponseDto)
        {
            await loadingService.PerformLoading("Loading article...", async () =>
            {
                var article = await articlesDataRequestService.GetData(articleResponseDto.Id);
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
        public async Task NavigateToArticleEditor(ArticleResponce articleResponseDto)
        {
            await loadingService.PerformLoading("Loading article editor...", async () =>
            {
                var article = await articlesDataRequestService.GetData(articleResponseDto.Id);
                FilesAttachmentsService.GetValidArticlesAttachments(article);

                await Shell.Current.GoToAsync($"{nameof(ArticleInformationView)}", true, new Dictionary<string, object>
                {
                    {"Article" , article},
                    {"IsNewArticle", false}
                });
            });            
        }

        [RelayCommand]
        public async Task DeleteArticle(ArticleResponce article)
        {
            bool deleteAction = await Application.Current.MainPage.DisplayAlert("Delete article", "Are you sure you want to delete this article? All data article will be lost", "Yes", "Cancel");

            if (!deleteAction) return;

            await loadingService.PerformLoading("Deleting article...", async () =>
            {
                var articleFiles = await articlesDataRequestService.GetFiles(article.Id);
                await fileService.DeleteFiles(articleFiles);

                await articlesDataRequestService.DeleteData(article.Id);
                UserData.Articles.Remove(article);
            });

            await Toast.Make("Article was successfully deleted").Show();
            SetArticlesCollectionAsItemSource();
        }
        

        [RelayCommand]
        public async Task NavigateToTestInformationView(TestResponse selectedTestDto)
        {
            await loadingService.PerformLoading("Loading test information...", async () =>
            {
                bool currentUserIsCreator = CurrentUser.Id == selectedTestDto.UserId;
                string route = currentUserIsCreator ? nameof(CreatorTestInformationView) : nameof(TestInformationView);
                TestResponse test;

                if (!currentUserIsCreator)
                {
                    string additionalQueryParameters = $"?userId={CurrentUser.Id}";
                    test = await testsDataRequestService.GetData(selectedTestDto.Id, additionalQueryParameters);
                }
                else
                {
                    test = await testsDataRequestService.GetData(selectedTestDto.Id);
                }

                test.AllTestCommentsCount = test.TestCommentsCount;
                test.AllTestUserScoresCount = test.TestUserScoresCount;

                test.AllTestComments = test.TestComments;
                test.AllTestUserScores = test.UserScores;

                FilesAttachmentsService.GetValidTestAttachments(test);
                await Shell.Current.GoToAsync(route, true, new Dictionary<string, object>
                {
                    {"Test", test}
                });
            });
        }

        [RelayCommand]
        public async Task NavigateToTestEditor(TestResponse testDto)
        {
            await loadingService.PerformLoading("Loading test editor...", async () =>
            {
                var test = await testsDataRequestService.GetData(testDto.Id);
                FilesAttachmentsService.GetValidTestAttachments(test);

                foreach (var testQuestion in test.TestQuestions)
                {
                    testQuestion.QuestionImageFilenameBeforeEdit = testQuestion.TestQuestionImageFilename;
                    testQuestion.QuestionImageFileTypeBeforeEdit = testQuestion.TestQuestionImageFileType;
                    testQuestion.QuestionFormulaImageFilenameBeforeEdit = testQuestion.TestQuestionFormulaImageFilename;
                    testQuestion.QuestionFormulaImageFileTypeBeforeEdit = testQuestion.TestQuestionFormulaImageFileType;

                    foreach (var testAnswer in testQuestion.TestAnswers)
                    {
                        testAnswer.AnswerImageFilenameBeforeEdit = testAnswer.TestAnswerImageFilename;
                        testAnswer.AnswerImageFileTypeBeforeEdit = testAnswer.TestAnswerImageFileType;
                        testAnswer.AnswerFormulaImageFilenameBeforeEdit = testAnswer.TestAnswerFormulaImageFilename;
                        testAnswer.AnswerFormulaImageFileTypeBeforeEdit = testAnswer.TestAnswerFormulaImageFileType;

                        testAnswer.VariantImageFilenameBeforeEdit = testAnswer.TestVariantImageFilename;
                        testAnswer.VariantImageFileTypeBeforeEdit = testAnswer.TestVariantImageFileType;
                        testAnswer.VariantFormulaImageFilenameBeforeEdit = testAnswer.TestVariantFormulaImageFilename;
                        testAnswer.VariantFormulaImageFileTypeBeforeEdit = testAnswer.TestVariantFormulaImageFileType;
                    }
                }

                await Shell.Current.GoToAsync($"{nameof(TestEditorView)}", true, new Dictionary<string, object>
                {
                    {"IsNewTest", false},
                    {"Test", test}
                });
            });            
        }

        [RelayCommand]
        public async Task DeleteTest(TestResponse test)
        {
            bool deleteAction = await Application.Current.MainPage.DisplayAlert("Delete test", "Are you sure you want to delete this test? All data test will be lost", "Yes", "Cancel");

            if (!deleteAction) return;

            await loadingService.PerformLoading("Deleting test...", async () =>
            {
                var testFiles = await testsDataRequestService.GetFiles(test.Id);
                await fileService.DeleteFiles(testFiles);

                await testsDataRequestService.DeleteData(test.Id);
                UserData.Tests.Remove(test);
            });

            await Toast.Make("Test was successfully deleted").Show();
            SetTestsCollectionAsItemSource();
        }
         

        [RelayCommand]
        public async Task NavigateToQuestionDetailsView(BaseQuestionResponce baseQuestionDto)
        {
            await loadingService.PerformLoading("Loading question...", async () =>
            {
                var baseQuestion = await baseQuestionsDataRequestService.GetData(baseQuestionDto.Id);
                baseQuestion.Question.AllQuestionCommentsCount = baseQuestion.Question.QuestionCommentsCount;
                baseQuestion.Question.AllQuestionComments = baseQuestion.Question.QuestionComments;

                foreach (var answer in baseQuestion.Answers)
                {
                    answer.AllAnswerCommentsCount = answer.AnswerCommentsCount;
                    answer.AllAnswerComments = answer.AnswerComments;
                }

                await FilesAttachmentsService.GetValidBaseQuestionAttachments(baseQuestion);
                await Shell.Current.GoToAsync($"{nameof(QuestionDetailsView)}", true, new Dictionary<string, object>
                {
                    {"BaseQuestion", baseQuestion},
                    {"Answer", null},
                    {"ScrollToAnswer", false}
                });
            });
        }

        [RelayCommand]
        public async Task NavigateToQuestionEditor(BaseQuestionResponce baseQuestionDto)
        {
            await loadingService.PerformLoading("Loading question editor...", async () =>
            {
                var baseQuestion = await baseQuestionsDataRequestService.GetData(baseQuestionDto.Id);
                FilesAttachmentsService.GetValidQuestionAttachments(baseQuestion.Question);
                
                baseQuestion.Question.AllAttachedImages = baseQuestion.Question.Images.ToList();
                baseQuestion.Question.AllAttachedFormulas = baseQuestion.Question.Formulas.ToList();

                await Shell.Current.GoToAsync($"{nameof(QuestionView)}", true, new Dictionary<string, object>
                {
                     {"IsNewQuestion", false},
                     {"BaseQuestion", baseQuestion},
                     {"Question", baseQuestion.Question}
                });
            });           
        }

        [RelayCommand]
        public async Task DeleteBaseQuestion(BaseQuestionResponce baseQuestion)
        {
            bool deleteAction = await Application.Current.MainPage.DisplayAlert("Delete question", "Are you sure you want to delete this question? All data test will be lost", "Yes", "Cancel");

            if (!deleteAction) return;

            await loadingService.PerformLoading("Deleting question...", async () =>
            {
                var questionFiles = await baseQuestionsDataRequestService.GetFiles(baseQuestion.Id);
                await fileService.DeleteFiles(questionFiles);

                await baseQuestionsDataRequestService.DeleteData(baseQuestion.Id);
                UserData.Questions.Remove(baseQuestion);
            });

            await Toast.Make("Question was successfully deleted").Show();
            SetQuestionsCollectionAsItemSource();
        }      
    }
}
