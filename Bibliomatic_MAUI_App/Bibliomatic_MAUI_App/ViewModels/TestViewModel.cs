using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Bibliomatic_MAUI_App.Models;
using Bibliomatic_MAUI_App.Views;
using Bibliomatic_MAUI_App.Services;

namespace Bibliomatic_MAUI_App.ViewModels
{    
    [QueryProperty(nameof(Test), "Test")]
    [QueryProperty(nameof(UserScore), "UserScore")]   
    public partial class TestViewModel : ObservableObject
    {
        LoadingService loadingService = new LoadingService();
        
        private readonly IRelatedDataService<TestResponse, UserScore> relatedDataUserScoresService;

        public TestViewModel(IRelatedDataService<TestResponse, UserScore> relatedDataUserScoresService)
        {
            this.relatedDataUserScoresService = relatedDataUserScoresService;
            relatedDataUserScoresService.ControllerName = "tests";
        }

        [ObservableProperty]
        public TestResponse test;        

        [ObservableProperty]
        public UserScore userScore;        

        private double GetTotalTestPoints()
        {
            double totalTestPoints = 0;
            var testQuestions = Test.TestQuestions;

            foreach(var testQuestion in testQuestions)
            {
                var questionType = testQuestion.TestQuestionType;
                var testAnswers = testQuestion.TestAnswers;

                int count = 0;

                switch (questionType)
                {
                    case TestQuestionType.OneAnswerQuestion:                        
                    case TestQuestionType.MultipleAnswerQuestion:
                        count = testAnswers.Count(answer => answer.IsCorrectAnswer);                        
                        break;

                    case TestQuestionType.PairsQuestion:
                    case TestQuestionType.SpacesQuestion:
                        count = testAnswers.Count;
                        break;

                    case TestQuestionType.OpenEndedQuestion:
                        count = 1;
                        break;                         
                }

                totalTestPoints += testQuestion.PointsPerAnswer * count;
            }

            return totalTestPoints;
        }
        
        private double GetUserTestPoints()
        {
            double userPoints = 0;
            var testQuestions = Test.TestQuestions;

            foreach (var testQuestion in testQuestions)
            {
                var questionType = testQuestion.TestQuestionType;

                var testAnswers = testQuestion.TestAnswers;
                var userAnswers = new List<TestAnswerResponse>();
                var multipleAnswerQuestionUserAnswers = new List<TestAnswerResponse>();

                switch (questionType)
                {                    
                    case TestQuestionType.OneAnswerQuestion:
                        userAnswers = testAnswers.Where(answer => answer.IsSelected & answer.IsCorrectAnswer).ToList();
                        testQuestion.QuestionCorrectType = GetOneAnswerType(testAnswers.ToList(), userAnswers);
                        break;

                    case TestQuestionType.MultipleAnswerQuestion:                        
                        userAnswers = testAnswers.Where(answer => answer.IsSelected & answer.IsCorrectAnswer).ToList();
                        multipleAnswerQuestionUserAnswers = testAnswers.Where(answer => answer.IsSelected).ToList();
                        testQuestion.QuestionCorrectType = GetMultipleAnswerType(testAnswers.ToList(), userAnswers);
                        break;

                    case TestQuestionType.PairsQuestion:
                        userAnswers = testAnswers.Where(answer => answer.Variant == answer.SelectedTestAnswer?.Variant && answer.Answer == answer.SelectedTestAnswer?.Answer).ToList();
                        testQuestion.QuestionCorrectType = GetPairsAnswerType(testAnswers.ToList(), userAnswers);
                        GetPairTestAnswersTypes(testAnswers);
                        break;

                    case TestQuestionType.OpenEndedQuestion:
                        userAnswers = testAnswers.Where(answer => answer.Answer == testQuestion.UserAnswer).ToList();
                        testQuestion.QuestionCorrectType = GetOpenEndedAnswerType(userAnswers);                        
                        break;                   
                }

                if (questionType == TestQuestionType.MultipleAnswerQuestion && multipleAnswerQuestionUserAnswers.Count > userAnswers.Count)
                {
                    userPoints += testQuestion.PointsPerAnswer * (userAnswers.Count / multipleAnswerQuestionUserAnswers.Count);
                }
                else
                {
                    userAnswers.ForEach(answer => userPoints += testQuestion.PointsPerAnswer);
                }                   
            }

            return userPoints;
        }

        private CorrectType GetOneAnswerType(List<TestAnswerResponse> testAnswers, List<TestAnswerResponse> userAnswers)
        {
            int selectedAnswers = testAnswers.Count(ta => ta.IsSelected);
            int correctAnswers = testAnswers.Count(ta => ta.IsCorrectAnswer);

            if(userAnswers.Count > 0 || (selectedAnswers == correctAnswers & correctAnswers == 0))
            {
                return CorrectType.Correct;
            }

            return CorrectType.Incorrect;
        }

        private CorrectType GetMultipleAnswerType(List<TestAnswerResponse> testAnswers, List<TestAnswerResponse> userAnswers)
        {
            int allCount = testAnswers.Where(ta => ta.IsCorrectAnswer).Count();
            int correctCount = userAnswers.Count;
            int selectedCount = testAnswers.Count(ta => ta.IsSelected); 

            if(allCount == correctCount & correctCount == selectedCount & allCount == selectedCount)
            {
                return CorrectType.Correct;
            }
            else if(allCount >= 0 & correctCount < 1)
            {
                return CorrectType.Incorrect;
            }

            return CorrectType.PartiallyCorrect;
        }

        private CorrectType GetPairsAnswerType(List<TestAnswerResponse> testAnswers, List<TestAnswerResponse> userAnswers)
        {
            int allCount = testAnswers.Count;
            int correctCount = userAnswers.Count;

            if (allCount == correctCount)
            {
                return CorrectType.Correct;
            }
            else if (correctCount > 0)
            {
                return CorrectType.PartiallyCorrect;
            }

            return CorrectType.Incorrect;
        }

        private void GetPairTestAnswersTypes(IEnumerable<TestAnswerResponse> testAnswers)
        {
            foreach(var testAnswer in testAnswers)
            {
                testAnswer.AnswerCorrectType = CorrectType.Incorrect;

                if (testAnswer.Variant == testAnswer.SelectedTestAnswer?.Variant && testAnswer.Answer == testAnswer.SelectedTestAnswer?.Answer)
                {
                    testAnswer.AnswerCorrectType = CorrectType.Correct;
                }                
            }
        }

        private CorrectType GetOpenEndedAnswerType(List<TestAnswerResponse> userAnswers)
        {
            if(userAnswers.Count > 0)
            {
                return CorrectType.Correct;
            }

            return CorrectType.Incorrect;
        }
       
        [RelayCommand]
        public async Task ReturnBack()
        {
            bool deleteAction = await Application.Current.MainPage.DisplayAlert("Return back", "Are you sure you want to go back? All your answers will be discarded", "Yes", "Cancel");

            if (!deleteAction) return;

            foreach (var testQuestion in Test.TestQuestions)
            {
                testQuestion.UserAnswer = null;
                testQuestion.SelectedPickerTestAnswer = null;                

                foreach (var testAnswer in testQuestion.TestAnswers)
                {
                    testAnswer.IsSelected = false;                    
                    testAnswer.SelectedTestAnswer = null;
                    testAnswer.SelectedPickerSpaceTestVariant = null;
                }
            }

            await Shell.Current.Navigation.PopAsync(true);            
        }

        

        [RelayCommand]
        public async Task NavigateToTestResultView()
        {
            await loadingService.PerformLoading("Loading test result...", async () =>
            {
                double totalPointsForCurrentTest = GetTotalTestPoints();
                double userPointsForCurrentTest = GetUserTestPoints();

                UserScore.PointsForTest = userPointsForCurrentTest;
                UserScore.TestEndDate = DateTimeOffset.Now;

                await relatedDataUserScoresService.CreateRelatedData(Test.Id, UserScore, "scores");

                Test.AllTestUserScores.Insert(0, UserScore);
                Test.TestUserScoresCount++;
                Test.AllTestUserScoresCount++;

                await Shell.Current.GoToAsync($"{nameof(TestResultView)}", true, new Dictionary<string, object>
                {
                    {"Test", Test },
                    {"TotalTestPoints", totalPointsForCurrentTest },
                    {"UserTestPoints", userPointsForCurrentTest }
                });
            });            
        }
    }
}
