using Bibliomatic_MAUI_App.Models;
using Bibliomatic_MAUI_App.Models.AttachmentInfo;
using Bibliomatic_MAUI_App.Services;
using Bibliomatic_MAUI_App.Views;
using Bibliomatic_MAUI_App.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Alerts;

namespace Bibliomatic_MAUI_App.ViewModels
{
    [QueryProperty(nameof(BaseQuestion), "BaseQuestion")]
    [QueryProperty(nameof(Answer), "Answer")]
    [QueryProperty(nameof(ScrollToAnswer), "ScrollToAnswer")]
    public partial class QuestionDetailsViewModel : ObservableObject, IQueryAttributable
    {        
        private int currentPageNumber = 2;
        private List<AnswerResponce> allAnswersList;

        LoadingService loadingService = new LoadingService();       
        FiltersManager filtersManager = new FiltersManager();       

        private readonly IDataService<BaseQuestionResponce> baseQuestionDataRequestService;
        private readonly IFileService fileService;
        private readonly IRelatedDataService<BaseQuestionResponce, AnswerResponce> relatedAnswerDataRequestService;
        private readonly IRelatedDataService<BaseQuestionResponce, AnswerLike> relatedAnswerDataLikeService;
        private readonly IRelatedDataService<BaseQuestionResponce, AnswerDislike> relatedAnswerDataDislikeService;
        private readonly IRelatedDataService<BaseQuestionResponce, AnswerComment> relatedAnswerDataCommentService;
        private readonly IRelatedDataService<BaseQuestionResponce, QuestionResponce> relatedQuestionDataRequestService;
        private readonly IRelatedDataService<BaseQuestionResponce, QuestionLike> relatedQuestionDataLikeService;
        private readonly IRelatedDataService<BaseQuestionResponce, QuestionDislike> relatedQuestionDataDislikeService;
        private readonly IRelatedDataService<BaseQuestionResponce, QuestionComment> relatedQuestionDataCommentService;

        [ObservableProperty]
        public BaseQuestionResponce baseQuestion;

        [ObservableProperty]
        public AnswerResponce answer;

        [ObservableProperty]
        public bool scrollToAnswer;

        [ObservableProperty]
        public bool loadMoreAnswersVisible;       

        [ObservableProperty]
        public bool isLoading;

        [ObservableProperty]
        public bool isRefreshing;

        [ObservableProperty]
        public int recordsOnPage = 5;

        [ObservableProperty]
        public int answersOnPage = 1;

        public QuestionDetailsViewModel(IDataService<BaseQuestionResponce> baseQuestionDataRequestService, IFileService fileService, 
                                        IRelatedDataService<BaseQuestionResponce, AnswerResponce> relatedAnswerDataRequestService,
                                        IRelatedDataService<BaseQuestionResponce, AnswerLike> relatedAnswerDataLikeService,
                                        IRelatedDataService<BaseQuestionResponce, AnswerDislike> relatedAnswerDataDislikeService,
                                        IRelatedDataService<BaseQuestionResponce, AnswerComment> relatedAnswerDataCommentService,                                        
                                        IRelatedDataService<BaseQuestionResponce, QuestionResponce> relatedQuestionDataRequestService,
                                        IRelatedDataService<BaseQuestionResponce, QuestionLike> relatedQuestionDataLikeService,
                                        IRelatedDataService<BaseQuestionResponce, QuestionDislike> relatedQuestionDataDislikeService,
                                        IRelatedDataService<BaseQuestionResponce, QuestionComment> relatedQuestionDataCommentService)
        {
            this.fileService = fileService;
            this.baseQuestionDataRequestService = baseQuestionDataRequestService;        
            
            this.relatedAnswerDataRequestService = relatedAnswerDataRequestService;
            this.relatedAnswerDataLikeService = relatedAnswerDataLikeService;
            this.relatedAnswerDataDislikeService = relatedAnswerDataDislikeService;
            this.relatedAnswerDataCommentService = relatedAnswerDataCommentService;

            this.relatedQuestionDataRequestService = relatedQuestionDataRequestService;
            this.relatedQuestionDataLikeService = relatedQuestionDataLikeService;
            this.relatedQuestionDataDislikeService = relatedQuestionDataDislikeService;
            this.relatedQuestionDataCommentService = relatedQuestionDataCommentService;
            

            baseQuestionDataRequestService.ControllerName = "questions";
            
            relatedQuestionDataRequestService.ControllerName = "questions";
            relatedQuestionDataRequestService.RelatedRouteName = "question";

            relatedQuestionDataLikeService.ControllerName = "questions";
            relatedQuestionDataLikeService.RelatedRouteName = "question";

            relatedQuestionDataDislikeService.ControllerName = "questions";
            relatedQuestionDataDislikeService.RelatedRouteName = "question";

            relatedQuestionDataCommentService.ControllerName = "questions";
            relatedQuestionDataCommentService.RelatedRouteName = "question";

            relatedAnswerDataRequestService.ControllerName = "questions";
            relatedAnswerDataRequestService.RelatedRouteName = "answers";

            relatedAnswerDataLikeService.ControllerName = "questions";
            relatedAnswerDataLikeService.RelatedRouteName = "answers";

            relatedAnswerDataDislikeService.ControllerName = "questions";
            relatedAnswerDataDislikeService.RelatedRouteName = "answers";

            relatedAnswerDataCommentService.ControllerName = "questions";
            relatedAnswerDataCommentService.RelatedRouteName = "answers";
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            ScrollToAnswer = (bool)query["ScrollToAnswer"];            
            
            if(ScrollToAnswer)
            {
                Answer = (AnswerResponce)query["Answer"];
                var existingAnswer = BaseQuestion.Answers.FirstOrDefault(a => a.Id == Answer.Id);

                if(existingAnswer != null)
                {
                    BaseQuestion.Answers.Remove(existingAnswer);
                }

                BaseQuestion.Answers.Insert(0, Answer);                
            }
            else
            {
                BaseQuestion = (BaseQuestionResponce)query["BaseQuestion"];
            }

            LoadMoreAnswersVisible = BaseQuestion.AnswersCount > RecordsOnPage;
        }

        [RelayCommand]
        public async Task LoadMoreAnswers()
        {            
            try
            {
                LoadMoreAnswersVisible = false;
                IsLoading = true;

                filtersManager.ClearCurrentFilters();
                filtersManager.AddOrderFilter("CreatedAt", OrderBy.Descending);
                string filter = filtersManager.CreateFiltersUrl();
                bool canLoadMoreAnswers = true;

                allAnswersList = await relatedAnswerDataRequestService.GetAllRelatedData(BaseQuestion.Id, currentPageNumber, RecordsOnPage, filter);

                if (allAnswersList?.Count > 0)
                {
                    await FilesAttachmentsService.GetValidAnswersAttachments(allAnswersList);

                    foreach (var answer in allAnswersList)
                    {
                        answer.BaseQuestion = BaseQuestion;
                        answer.AllAnswerCommentsCount = answer.AnswerCommentsCount;
                        answer.AllAnswerComments = answer.AnswerComments;
                        BaseQuestion.Answers.Add(answer);
                    }

                    if (allAnswersList.Count < RecordsOnPage) canLoadMoreAnswers = false;

                    currentPageNumber++;
                }

                IsLoading = false;
                LoadMoreAnswersVisible = canLoadMoreAnswers;
            }
            catch (Exception ex)
            {
                await Toast.Make(ex.Message).Show();
            }
        }
        

        [RelayCommand]
        public async Task RefreshBaseQuestionData()
        {
            IsRefreshing = false;

            await loadingService.PerformLoading("Refreshing question...", async () =>
            {
                currentPageNumber = 2;

                var baseQuestion = await baseQuestionDataRequestService.GetData(BaseQuestion.Id);
                await FilesAttachmentsService.GetValidBaseQuestionAttachments(baseQuestion);

                baseQuestion.AllAnswersCount = baseQuestion.AnswersCount;
                baseQuestion.AllAnswers = baseQuestion.Answers;

                baseQuestion.Question.AllQuestionCommentsCount = baseQuestion.Question.QuestionCommentsCount;
                baseQuestion.Question.AllQuestionComments = baseQuestion.Question.QuestionComments;

                foreach (var answer in baseQuestion.Answers)
                {
                    answer.AllAnswerCommentsCount = answer.AnswerCommentsCount;
                    answer.AllAnswerComments = answer.AnswerComments;
                }

                BaseQuestion = baseQuestion;
            });       
        }

        [RelayCommand]
        public async Task LoadMoreQuestionComments()
        {         
            var question = BaseQuestion.Question;
            var allRecordsCollection = question.AllQuestionComments;
            int skippedRecords = allRecordsCollection.Count;

            filtersManager.ClearCurrentFilters();
            filtersManager.AddOrderFilter("CreatedAt", OrderBy.Descending);
            string filter = filtersManager.CreateFiltersUrl();

            var questionCommentsResponse = await relatedQuestionDataCommentService.GetAllRelatedDataById(question.Id, "comments", skippedRecords, RecordsOnPage, filter);

            foreach (var questionComment in questionCommentsResponse)
            {                
                allRecordsCollection.Add(questionComment);
            }
        }

        [RelayCommand]
        public async Task LoadMoreAnswerComments(AnswerResponce answer)
        {            
            var allRecordsCollection = answer.AllAnswerComments;
            int skippedRecords = allRecordsCollection.Count;

            filtersManager.ClearCurrentFilters();
            filtersManager.AddOrderFilter("CreatedAt", OrderBy.Descending);
            string filter = filtersManager.CreateFiltersUrl();

            var answerCommentsResponse = await relatedAnswerDataCommentService.GetAllRelatedDataById(BaseQuestion.Id, answer.Id, "comments", skippedRecords, RecordsOnPage, filter);

            foreach (var answerComment in answerCommentsResponse)
            {           
                allRecordsCollection.Add(answerComment);
            }
        }       

        [RelayCommand]
        public async Task RemoveAnswer(AnswerResponce answer)
        {
            bool deleteAction = await Application.Current.MainPage.DisplayAlert("Delete answer", "Are you sure you want to delete this answer? All attachments and data will be lost", "Yes", "Cancel");

            if (!deleteAction) return;

            await loadingService.PerformLoading("Removing answer...", async () =>
            {
                var files = FilesAttachmentsService.GetAnswerFiles(answer);
                await fileService.DeleteFiles(files);
                await relatedAnswerDataRequestService.DeleteRelatedData(BaseQuestion.Id, answer.Id);

                BaseQuestion.Answers.Remove(answer);
                BaseQuestion.AnswersCount--;
                BaseQuestion.AllAnswersCount--;
            });

            await Toast.Make("Answer was successfully deleted").Show();
        }

        [RelayCommand]
        public async Task MarkAsAnswer(AnswerResponce answerToMark)
        {
            try
            {
                bool isAnswer = !answerToMark.IsAnswer;

                answerToMark.IsAnswer = isAnswer;
                answerToMark.MarkedAsAnswer = isAnswer;

                await relatedAnswerDataRequestService.PatchRelatedData(BaseQuestion.Id, answerToMark.Id, PatchAction.Replace, nameof(answerToMark.IsAnswer), isAnswer);

                if (BaseQuestion.IsSolved)
                {
                    if (!BaseQuestion.Answers.Any(a => a.IsAnswer))
                    {
                        await baseQuestionDataRequestService.PatchData(BaseQuestion.Id, PatchAction.Replace, nameof(BaseQuestion.IsSolved), false);
                    }
                }
                else
                {
                    if (BaseQuestion.Answers.Any(a => a.IsAnswer))
                    {
                        await baseQuestionDataRequestService.PatchData(BaseQuestion.Id, PatchAction.Replace, nameof(BaseQuestion.IsSolved), true);
                    }
                }

                if(isAnswer) await Toast.Make("Answer was successfully marked as solution").Show();
            }
            catch(Exception)
            {
                await Toast.Make("An error occurred while setting this answer as solution. Please try again or later").Show();
            }
        }

        [RelayCommand]
        public async Task ChangeQuestionLikes(QuestionResponce questionToChange)
        {
            try
            {
                var userLike = questionToChange.QuestionLikes.FirstOrDefault(al => al.UserId == CurrentUser.Id);

                if (userLike == null)
                {
                    userLike = new QuestionLike() { Id = Guid.NewGuid(), QuestionId = questionToChange.Id, UserId = CurrentUser.Id };                    
                    await relatedQuestionDataLikeService.CreateRelatedData(BaseQuestion.Id, userLike, "likes");
                    var userDislike = questionToChange.QuestionDislikes.FirstOrDefault(al => al.UserId == CurrentUser.Id);

                    if (userDislike != null) await ChangeQuestionDislikesCommand.ExecuteAsync(questionToChange);                    

                    questionToChange.QuestionLikes.Add(userLike);
                }
                else
                {
                    await relatedQuestionDataLikeService.DeleteRelatedData(BaseQuestion.Id, userLike.Id, "likes");
                    questionToChange.QuestionLikes.Remove(userLike);
                }
            }
            catch(Exception)
            {
                await Toast.Make("An error occurred while adding like. Please try again or later").Show();
            }
        }

        [RelayCommand]
        public async Task ChangeQuestionDislikes(QuestionResponce questionToChange)
        {
            try
            {
                var userDislike = questionToChange.QuestionDislikes.FirstOrDefault(al => al.UserId == CurrentUser.Id);

                if (userDislike == null)
                {
                    userDislike = new QuestionDislike() { Id = Guid.NewGuid(), QuestionId = questionToChange.Id, UserId = CurrentUser.Id };
                    await relatedQuestionDataDislikeService.CreateRelatedData(BaseQuestion.Id, userDislike, "dislikes");
                    var userLike = questionToChange.QuestionLikes.FirstOrDefault(al => al.UserId == CurrentUser.Id);

                    if (userLike != null) await ChangeQuestionLikesCommand.ExecuteAsync(questionToChange);                    

                    questionToChange.QuestionDislikes.Add(userDislike);
                }
                else
                {
                    await relatedQuestionDataDislikeService.DeleteRelatedData(BaseQuestion.Id, userDislike.Id, "dislikes");
                    questionToChange.QuestionDislikes.Remove(userDislike);
                }
            }
            catch(Exception)
            {
                await Toast.Make("An error occurred while adding dislike. Please try again or later").Show();
            }
        }

        [RelayCommand]
        public async Task AddCommentToQuestion(StackLayout commentStackLayout)
        {
            try
            {
                var question = BaseQuestion.Question;
                if (string.IsNullOrEmpty(question.QuestionComment)) return;

                var comment = new QuestionComment() 
                { 
                    Id = Guid.NewGuid(), 
                    QuestionId = question.Id, 
                    UserId = CurrentUser.Id,
                    Author = CurrentUser.FullName,
                    Comment = question.QuestionComment,
                    CreatedAt = DateTimeOffset.Now 
                };
                
                await relatedQuestionDataCommentService.CreateRelatedData(BaseQuestion.Id, comment, "comments");
                CancelAddingCommentCommand.Execute(commentStackLayout);

                question.QuestionComment = string.Empty;
                question.AllQuestionComments.Insert(0, comment);

                question.QuestionCommentsCount++;
                question.AllQuestionCommentsCount++;

                await Toast.Make("Question comment was successfully added").Show();
            }
            catch(Exception)
            {
                await Toast.Make("An error occurred while adding question comment. Please try again or later").Show();
            }
        }

        [RelayCommand]
        public async Task RemoveCommentFromQuestion(QuestionComment comment)
        {
            try
            {
                bool deleteAction = await Application.Current.MainPage.DisplayAlert("Delete comment", "Are you sure you want to delete this comment?", "Yes", "Cancel");

                if (!deleteAction) return;

                var question = BaseQuestion.Question;

                await relatedQuestionDataCommentService.DeleteRelatedData(BaseQuestion.Id, comment.Id, "comments");
                question.AllQuestionComments.Remove(comment);

                question.QuestionCommentsCount--;
                question.AllQuestionCommentsCount--;               

                await Toast.Make("Question comment was successfully deleted").Show();
            }
            catch(Exception)
            {
                await Toast.Make("An error occurred while deleting question comment. Please try again or later").Show();
            }
        }

        [RelayCommand]
        public async Task ChangeAnswerLikes(AnswerResponce answerToChange)
        {
            try
            {
                var userLike = answerToChange.AnswerLikes.FirstOrDefault(al => al.UserId == CurrentUser.Id);

                if (userLike == null)
                {
                    userLike = new AnswerLike() { Id = Guid.NewGuid(), AnswerId = answerToChange.Id, UserId = CurrentUser.Id };
                    await relatedAnswerDataLikeService.CreateRelatedData(BaseQuestion.Id, answerToChange.Id, userLike, "likes");
                    var userDislike = answerToChange.AnswerDislikes.FirstOrDefault(al => al.UserId == CurrentUser.Id);

                    if (userDislike != null) await ChangeAnswerDislikesCommand.ExecuteAsync(answerToChange);                    

                    answerToChange.AnswerLikes.Add(userLike);
                }
                else
                {
                    await relatedAnswerDataLikeService.DeleteRelatedData(BaseQuestion.Id, answerToChange.Id, userLike.Id, "likes");
                    answerToChange.AnswerLikes.Remove(userLike);
                }
            }
            catch(Exception)
            {
                await Toast.Make("An error occurred while adding like. Please try again or later").Show();
            }
        }

        [RelayCommand]
        public async Task ChangeAnswerDislikes(AnswerResponce answerToChange)
        {
            try
            {
                var userDislike = answerToChange.AnswerDislikes.FirstOrDefault(al => al.UserId == CurrentUser.Id);

                if (userDislike == null)
                {
                    userDislike = new AnswerDislike() { Id = Guid.NewGuid(), AnswerId = answerToChange.Id, UserId = CurrentUser.Id };                    
                    await relatedAnswerDataDislikeService.CreateRelatedData(BaseQuestion.Id, answerToChange.Id, userDislike, "dislikes");
                    var userLike = answerToChange.AnswerLikes.FirstOrDefault(al => al.UserId == CurrentUser.Id);

                    if (userLike != null) await ChangeAnswerLikesCommand.ExecuteAsync(answerToChange);                    

                    answerToChange.AnswerDislikes.Add(userDislike);
                }
                else
                {
                    await relatedAnswerDataDislikeService.DeleteRelatedData(BaseQuestion.Id, answerToChange.Id, userDislike.Id, "dislikes");
                    answerToChange.AnswerDislikes.Remove(userDislike);
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
        public async Task AddCommentToAnswer(StackLayout commentStackLayout)
        {
            try
            {
                var answer = (AnswerResponce)commentStackLayout.BindingContext;
                if (string.IsNullOrEmpty(answer.AnswerComment)) return;

                var comment = new AnswerComment() 
                { 
                    Id = Guid.NewGuid(), 
                    AnswerId = answer.Id, 
                    UserId = CurrentUser.Id,
                    Author = CurrentUser.FullName,
                    Comment = answer.AnswerComment, 
                    CreatedAt = DateTimeOffset.Now 
                };
                
                await relatedAnswerDataCommentService.CreateRelatedData(BaseQuestion.Id, answer.Id, comment, "comments");
                CancelAddingCommentCommand.Execute(commentStackLayout);

                answer.AnswerComment = string.Empty;
                answer.AllAnswerComments.Insert(0, comment);

                answer.AnswerCommentsCount++;
                answer.AllAnswerCommentsCount++;

                await Toast.Make("Answer comment was successfully added").Show();
            }
            catch(Exception)
            {
                await Toast.Make("An error occurred while adding answer comment. Please try again or later").Show();
            }
        }

        [RelayCommand]
        public async Task RemoveCommentFromAnswer(AnswerComment comment)
        {
            try
            {
                bool deleteAction = await Application.Current.MainPage.DisplayAlert("Delete comment", "Are you sure you want to delete this comment?", "Yes", "Cancel");

                if (!deleteAction) return;

                var answer = BaseQuestion.Answers.FirstOrDefault(a => a.Id == comment.AnswerId);

                await relatedAnswerDataCommentService.DeleteRelatedData(BaseQuestion.Id, answer.Id, comment.Id, "comments");
                answer.AllAnswerComments.Remove(comment);

                answer.AnswerCommentsCount--;
                answer.AllAnswerCommentsCount--;

                await Toast.Make("Answer comment was successfully deleted").Show();
            }
            catch(Exception)
            {
                await Toast.Make("An error occurred while deleting answer comment. Please try again or later").Show();
            }
        }

        [RelayCommand]
        public async Task NavigateToAnswerEditor(AnswerResponce answer)
        {
            await loadingService.PerformLoading("Loading answer editor...", async () =>
            {
                var baseQuestion = BaseQuestion;
                bool isNewAnswer = false;

                if (answer == null)
                {
                    isNewAnswer = true;
                    answer = new AnswerResponce
                    {
                        Id = Guid.NewGuid(),
                        AnswerBody = null,
                        BaseQuestion = null,
                        Formulas = new ObservableCollection<AnswerFormulaInfo>(),
                        Hyperlinks = new ObservableCollection<AnswerHyperlinkInfo>(),
                        Images = new ObservableCollection<AnswerImageInfo>(),
                        AnswerLikes = new ObservableCollection<AnswerLike>(),
                        AnswerDislikes = new ObservableCollection<AnswerDislike>(),
                        AnswerComments = new ObservableCollection<AnswerComment>(),
                        BaseQuestionId = baseQuestion.Id,
                        UserId = CurrentUser.Id,
                        Author = CurrentUser.FullName,
                        AnswerHtmlFormat = null,
                        AllAttachedFormulas = new List<AnswerFormulaInfo>(),
                        AllAttachedImages = new List<AnswerImageInfo>(),
                        IsAnswer = false,                        
                        CreatedAt = DateTimeOffset.Now,
                        UpdatedAt = DateTimeOffset.Now
                    };
                }
                else
                {
                    FilesAttachmentsService.GetValidAnswerAttachments(answer);                    
                    answer.AllAttachedImages = answer.Images.ToList();
                    answer.AllAttachedFormulas = answer.Formulas.ToList();
                    answer.UpdatedAt = DateTimeOffset.Now;
                }

                await Shell.Current.GoToAsync($"{nameof(AnswerView)}", true, new Dictionary<string, object>
                {
                    {"IsNewAnswer", isNewAnswer},
                    {"BaseQuestion", baseQuestion},
                    {"Answer", answer}
                });
            });    
        }
        
        private async Task OpenHtmlInFullResolution(HtmlWebViewSource elementHtml, string header)
        {
            await loadingService.PerformLoading("Loading full resolution...", async () =>
            {
                await Shell.Current.GoToAsync($"{nameof(QuestionBodyView)}", true, new Dictionary<string, object>
                {
                    {"Header", header },
                    {"ElementHtml", elementHtml},
                    {"QuestionHeader", BaseQuestion.Header},
                    {"QuestionDescription", BaseQuestion.Description}
                });
            });            
        }

        [RelayCommand]
        public async Task OpenQuestionHtmlInFullResolution(HtmlWebViewSource elementHtml)
        {
            await OpenHtmlInFullResolution(elementHtml, "Question view");
        }

        [RelayCommand]
        public async Task OpenAnswerHtmlInFullResolution(HtmlWebViewSource elementHtml)
        {
            await OpenHtmlInFullResolution(elementHtml, "Answer view");
        }        
    }
}
