using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Bibliomatic_MAUI_App.Models;
using Bibliomatic_MAUI_App.Services;
using Bibliomatic_MAUI_App.Views;
using Bibliomatic_MAUI_App.Helpers;

namespace Bibliomatic_MAUI_App.ViewModels
{
    public partial class QuestionsViewModel : ObservableObject
    {        
        LoadingService loadingService = new LoadingService();
        FiltersManager filtersManager = new FiltersManager();        

        private int currentPageNumber = 1;
        private readonly int currentPageSize = 20;        
        public ObservableCollection<BaseQuestionResponce> BaseQuestionsList { get; set; }
        private List<BaseQuestionResponce> allBaseQuestions;
        
        private readonly IDataService<BaseQuestionResponce> baseQuestionService;
        private readonly IFileService fileRequestService;

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
        public bool noContentLayoutVisible;

        [ObservableProperty]
        public bool failedLayoutVisible;

        [ObservableProperty]
        public string authorFilter;

        [ObservableProperty]
        public string headerFilter;

        [ObservableProperty]
        public bool showNewQuestionsFirstFilter = false;

        [ObservableProperty]
        public bool showOldQuestionsFirstFilter = true;

        [ObservableProperty]
        public bool showAllQuestions = true;

        [ObservableProperty]
        public bool showSolvedQuestions = false;

        [ObservableProperty]
        public bool showNotSolvedQuestions = false;

        public QuestionsViewModel(IDataService<BaseQuestionResponce> baseQuestionService, IFileService fileRequestService)
        {            
            this.fileRequestService = fileRequestService;
            this.baseQuestionService = baseQuestionService;
            baseQuestionService.ControllerName = "questions";

            BaseQuestionsList = new ObservableCollection<BaseQuestionResponce>();
            LoadBaseQuestionsData();           
        }

        private async Task FillBaseQuestionsCollectionData()
        {
            try
            {
                FailedLayoutVisible = false;
                NoContentLayoutVisible = false;

                string dataFilter = filtersManager.CreateFiltersUrl();                
                allBaseQuestions = await baseQuestionService.GetAllData(currentPageNumber, currentPageSize, dataFilter);

                if (allBaseQuestions?.Count > 0)
                {
                    foreach (var baseQuestion in allBaseQuestions)
                    {
                        BaseQuestionsList.Add(baseQuestion);
                    }

                    if (allBaseQuestions.Count < currentPageSize)
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
        
        private void LoadBaseQuestionsData()
        {           
            IsBusy = true;

            Task.Run(async () =>
            {
                filtersManager.ClearCurrentFilters();
                filtersManager.AddOrderFilter("UpdatedAt", OrderBy.Ascending);
                await FillBaseQuestionsCollectionData();

                IsBusy = false;
            });            
        }       

        [RelayCommand]
        public async Task LoadMoreBaseQuestionsData()
        {
            if (IsLoading || IsBusy || IsRefreshing || IsFiltering) return;

            IsLoading = true;            
            await FillBaseQuestionsCollectionData();
            IsLoading = false;
        }

        [RelayCommand]
        public async Task RefreshBaseQuestionsData()
        {
            BaseQuestionsList.Clear();            
            currentPageNumber = 1;

            IsRefreshing = true;
            await FillBaseQuestionsCollectionData();
            IsRefreshing = false;
        }

        [RelayCommand]
        public async Task NavigateToQuestionDetailsView(BaseQuestionResponce baseQuestionDto)
        {
            await loadingService.PerformLoading("Loading question...", async () =>
            {
                var baseQuestion = await baseQuestionService.GetData(baseQuestionDto.Id);

                baseQuestion.AllAnswersCount = baseQuestion.AnswersCount;
                baseQuestion.AllAnswers = baseQuestion.Answers;

                baseQuestion.Question.AllQuestionCommentsCount = baseQuestion.Question.QuestionCommentsCount;
                baseQuestion.Question.AllQuestionComments = baseQuestion.Question.QuestionComments;

                foreach(var answer in baseQuestion.Answers)
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
        public async Task FilterData()
        {
            BaseQuestionsList.Clear();
            currentPageNumber = 1;

            IsFiltering = true;
            filtersManager.ClearCurrentFilters();

            var orderBy = ShowNewQuestionsFirstFilter ? OrderBy.Descending : OrderBy.Ascending;

            if (ShowSolvedQuestions) filtersManager.AddDefaultDataFilter("IsSolved", true, true);
            if (ShowNotSolvedQuestions) filtersManager.AddDefaultDataFilter("IsSolved", false, true);

            filtersManager.AddStringDataFilter("Author", AuthorFilter, true);
            filtersManager.AddStringDataFilter("Header", HeaderFilter, true);
            filtersManager.AddOrderFilter("UpdatedAt", orderBy);

            await FillBaseQuestionsCollectionData();
            IsFiltering = false;
        }

        [RelayCommand]
        public void ClearFilters()
        {
            AuthorFilter = string.Empty;
            HeaderFilter = string.Empty;

            ShowNewQuestionsFirstFilter = false;
            ShowOldQuestionsFirstFilter = true;

            ShowAllQuestions = true;
            ShowSolvedQuestions = false;
            ShowNotSolvedQuestions = false;
        }
    }
}
