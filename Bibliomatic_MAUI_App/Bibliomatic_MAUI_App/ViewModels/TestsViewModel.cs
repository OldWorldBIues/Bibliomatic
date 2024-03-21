using Bibliomatic_MAUI_App.Models;
using Bibliomatic_MAUI_App.Services;
using Bibliomatic_MAUI_App.Views;
using Bibliomatic_MAUI_App.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Bibliomatic_MAUI_App.ViewModels
{
    public partial class TestsViewModel : ObservableObject
    {       
        LoadingService loadingService = new LoadingService();
        FiltersManager filtersManager = new FiltersManager();

        private int currentPageNumber = 1;
        private readonly int currentPageSize = 20;

        public ObservableCollection<TestResponse> TestsList { get; set; }
        public List<TestResponse> allTestsList;

        private readonly ILocalObjectKeeperService<TestResponse> localTestKeeper;
        private readonly IDataService<TestResponse> testService;
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
        public bool noContentLayoutVisible;

        [ObservableProperty]
        public bool failedLayoutVisible;

        [ObservableProperty]
        public string authorFilter;

        [ObservableProperty]
        public string nameFilter;

        [ObservableProperty]
        public bool showNewTestsFirstFilter = false;

        [ObservableProperty]
        public bool showOldTestsFirstFilter = true;

        [ObservableProperty]
        public bool showAllTests = true;

        [ObservableProperty]
        public bool showPassedTests = false;

        [ObservableProperty]
        public bool showNotPassedTests = false;

        public TestsViewModel(ILocalObjectKeeperService<TestResponse> localTestKeeper, IDataService<TestResponse> testService, IFileService fileService)
        {
            this.localTestKeeper = localTestKeeper;
            this.testService = testService;
            this.fileService = fileService;          

            testService.ControllerName = "tests";           

            TestsList = new ObservableCollection<TestResponse>();
            LoadTestData();          
        }

        private async Task FillTestsCollectionData(string additionalQueryParameters = null)
        {
            try
            {
                FailedLayoutVisible = false;
                NoContentLayoutVisible = false;

                string dataFilter = filtersManager.CreateFiltersUrl();                
                allTestsList = await testService.GetAllData(currentPageNumber, currentPageSize, additionalQueryParameters, dataFilter);

                if (allTestsList?.Count > 0)
                {
                    foreach (var test in allTestsList)
                    {
                        TestsList.Add(test);
                    }

                    if (allTestsList.Count < currentPageSize)
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
        
        private void LoadTestData()
        {           
            IsBusy = true;

            Task.Run(async () =>
            {
                filtersManager.ClearCurrentFilters();
                filtersManager.AddOrderFilter("UpdatedAt", OrderBy.Ascending);
                await FillTestsCollectionData();
                IsBusy = false;
            });            
        }

        [RelayCommand]
        public async Task LoadMoreTestData()
        {
            if (IsLoading || IsBusy || IsRefreshing || IsFiltering) return;

            IsLoading = true;
            await FillTestsCollectionData();
            IsLoading = false;
        }

        [RelayCommand]
        public async Task RefreshTestData()
        {
            TestsList.Clear();            
            currentPageNumber = 1;

            IsRefreshing = true;
            await FillTestsCollectionData();
            IsRefreshing = false;
        }
        

        [RelayCommand]
        public async Task FilterData()
        {
            TestsList.Clear();
            currentPageNumber = 1;

            IsFiltering = true;
            filtersManager.ClearCurrentFilters();

            var orderBy = ShowNewTestsFirstFilter ? OrderBy.Descending : OrderBy.Ascending;
            string additionalQueryParameters = null;

            if (ShowPassedTests || ShowNotPassedTests)
            {
                additionalQueryParameters = $"&userId={CurrentUser.Id}&passed=";

                if (ShowPassedTests) additionalQueryParameters += "true";
                if (ShowNotPassedTests) additionalQueryParameters += "false";

                filtersManager.AddDefaultDataFilter("UserId", CurrentUser.Id, false);
            }           

            filtersManager.AddStringDataFilter("Author", AuthorFilter, true);
            filtersManager.AddStringDataFilter("Name", NameFilter, true);            
            filtersManager.AddOrderFilter("UpdatedAt", orderBy);

            await FillTestsCollectionData(additionalQueryParameters);
            IsFiltering = false;
        }

        [RelayCommand]
        public void ClearFilters()
        {
            AuthorFilter = string.Empty;
            NameFilter = string.Empty;

            ShowNewTestsFirstFilter = false;
            ShowOldTestsFirstFilter = true;

            ShowAllTests = true;
            ShowPassedTests = false;
            ShowNotPassedTests = false;
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
                    test = await testService.GetData(selectedTestDto.Id, additionalQueryParameters);
                }
                else
                {
                    test = await testService.GetData(selectedTestDto.Id);
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
    }
}
