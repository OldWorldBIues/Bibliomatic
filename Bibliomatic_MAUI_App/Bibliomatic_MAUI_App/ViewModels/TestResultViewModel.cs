using Bibliomatic_MAUI_App.Models;
using Bibliomatic_MAUI_App.Services;
using Bibliomatic_MAUI_App.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bibliomatic_MAUI_App.ViewModels
{
    [QueryProperty(nameof(Test), "Test")]
    [QueryProperty(nameof(TotalTestPoints), "TotalTestPoints")]
    [QueryProperty(nameof(UserTestPoints), "UserTestPoints")]    
    public partial class TestResultViewModel : ObservableObject
    {
        LoadingService loadingService = new LoadingService();

        [ObservableProperty]
        public TestResponse test;

        [ObservableProperty]
        public double totalTestPoints;

        [ObservableProperty]
        public double userTestPoints;   

        [RelayCommand]
        public async Task NavigateToTestFeedbackPage()
        {
            await loadingService.PerformLoading("Loading test information...", async () =>
            {
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

                bool currentUserIsCreator = CurrentUser.Id == Test.UserId;

                await Shell.Current.Navigation.PopToRootAsync(false);
                await Shell.Current.GoToAsync($"{nameof(TestInformationView)}", false, new Dictionary<string, object>
                {
                    {"Test", Test},
                    {"CurrentUserIsCreator", currentUserIsCreator}
                });
            });            
        }

        [RelayCommand]
        public async Task NavigateToAllTestsPage()
        {
            await loadingService.PerformLoading("Loading tests...", async () =>
            {
                await Shell.Current.Navigation.PopToRootAsync(false);
                await Shell.Current.GoToAsync($"{nameof(TestsView)}");
            });            
        }
    }
}
