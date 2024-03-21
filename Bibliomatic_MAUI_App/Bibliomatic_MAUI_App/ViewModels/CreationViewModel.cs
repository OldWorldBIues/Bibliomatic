using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Views;
using Bibliomatic_MAUI_App.Views;
using Bibliomatic_MAUI_App.Models;
using Bibliomatic_MAUI_App.Models.AttachmentInfo;
using Bibliomatic_MAUI_App.CustomControls;
using System.Collections.ObjectModel;
using Bibliomatic_MAUI_App.Services;

namespace Bibliomatic_MAUI_App.ViewModels
{
    public partial class CreationViewModel : ObservableObject
    {
        LoadingService loadingService = new LoadingService();

        [RelayCommand]
        public async Task NavigateToArticleInformationView()
        {
            await loadingService.PerformLoading("Loading article editor...", async () =>
            {
                var article = new ArticleResponce
                {
                    Id = Guid.NewGuid(),
                    Title = string.Empty,
                    Description = string.Empty,
                    UserId = CurrentUser.Id,
                    Author = CurrentUser.FullName,
                    ArticleImageSource = "attach_element.png",
                    ArticleImagePath = "attach_element.png",
                    ArticleDocumentSource = string.Empty,
                    ArticleDocumentPath = string.Empty,
                    CreatedAt = DateTimeOffset.Now,
                    UpdatedAt = DateTimeOffset.Now
                };

                await Shell.Current.GoToAsync($"{nameof(ArticleInformationView)}", true, new Dictionary<string, object>
                {
                    {"Article" , article},
                    {"IsNewArticle", true}
                });
            });            
        }

        [RelayCommand]
        public async Task NavigateToTestInformationView()
        {
            await loadingService.PerformLoading("Loading test editor...", async () =>
            {
                var test = new TestResponse
                {
                    Id = Guid.NewGuid(),
                    Name = string.Empty,
                    UserId = CurrentUser.Id,
                    Author = CurrentUser.FullName,
                    Description = string.Empty,
                    TestQuestions = new ObservableCollection<TestQuestionResponse>(),
                    CreatedAt = DateTimeOffset.Now,
                    UpdatedAt = DateTimeOffset.Now
                };

                await Shell.Current.GoToAsync($"{nameof(TestEditorView)}", true, new Dictionary<string, object>
                {
                    {"Test", test},
                    {"IsNewTest", true}
                });
            });               
        }

        [RelayCommand]
        public async Task NavigateToQuestionInformationView()
        {
            await loadingService.PerformLoading("Loading question editor...", async () =>
            {
                var questionBase = new BaseQuestionResponce
                {
                    Id = Guid.NewGuid(),
                    Header = string.Empty,
                    UserId = CurrentUser.Id,
                    Author = CurrentUser.FullName,
                    Description = string.Empty,
                    Answers = new ObservableCollection<AnswerResponce>(),
                    IsSolved = false,
                    CreatedAt = DateTimeOffset.Now,
                    UpdatedAt = DateTimeOffset.Now
                };

                var question = new QuestionResponce
                {
                    Id = Guid.NewGuid(),
                    QuestionBody = null,
                    UserId = CurrentUser.Id,
                    Author = CurrentUser.FullName,
                    Formulas = new ObservableCollection<QuestionFormulaInfo>(),
                    Hyperlinks = new ObservableCollection<QuestionHyperlinkInfo>(),
                    Images = new ObservableCollection<QuestionImageInfo>(),
                    BaseQuestion = questionBase,
                    BaseQuestionId = questionBase.Id,
                    QuestionHtmlFormat = null,
                    AllAttachedFormulas = new List<QuestionFormulaInfo>(),
                    AllAttachedImages = new List<QuestionImageInfo>(),
                    CreatedAt = DateTimeOffset.Now,
                    UpdatedAt = DateTimeOffset.Now
                };

                questionBase.Question = question;

                await Shell.Current.GoToAsync($"{nameof(QuestionView)}", true, new Dictionary<string, object>
                {
                     {"IsNewQuestion", true },
                     {"BaseQuestion", questionBase},
                     {"Question", question}
                });
            });            
        }
    }
}
