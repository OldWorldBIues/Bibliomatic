using CommunityToolkit.Mvvm.Input;
namespace Bibliomatic_MAUI_App.CustomControls;

public partial class CollectionPaginationControl : ContentView
{
	public CollectionPaginationControl()
	{
		InitializeComponent();
	}
   
    private bool IsInitialization { get; set; } = true;
    private int CurrentPage { get; set; } = 1;
    private int TotalPagesCount { get; set; } = 1;
    private List<object> AllRecordsCollection { get; set; }


    public static readonly BindableProperty TotalRecordsProperty = BindableProperty.Create(
        propertyName: nameof(TotalRecords),
        returnType: typeof(int),
        declaringType: typeof(CollectionPaginationControl),
        defaultBindingMode: BindingMode.OneWay,
        defaultValue: -1,
        propertyChanged: TotalRecordsPropertyChanged);

    
    private static void TotalRecordsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var controls = (CollectionPaginationControl)bindable;

        if(!controls.IsInitialization)
        {
            int oldTotalRecordsCountValue = (int)oldValue;
            int newTotalRecordsCountValue = (int)newValue;

            int totalRecords = newTotalRecordsCountValue <= 0 ? 1 : newTotalRecordsCountValue;
            double pagesCountDouble = totalRecords / (double)controls.RecordsOnPage;

            int totalPagesCount = (int)Math.Ceiling(pagesCountDouble);
            controls.TotalPagesCount = totalPagesCount;

            if (oldTotalRecordsCountValue > newTotalRecordsCountValue)
            {
                if (controls.CurrentPage > totalPagesCount)
                {
                    controls.CurrentPage--;
                }

                if (newTotalRecordsCountValue == 0)
                {
                    controls.ControlsLayout.IsVisible = false;
                    controls.EmptyViewLayout.IsVisible = true;                    
                }
            }  
            else
            {
                controls.ControlsLayout.IsVisible = true;
                controls.EmptyViewLayout.IsVisible = false;          
            }
            
            controls.SetCollectionItemSource();
            controls.ChangeCurrentPage();
            controls.SetButtons();
            controls.InvalidateLayout();
        }

        controls.IsInitialization = false;
    }

    public int TotalRecords
    {
        get => (int)GetValue(TotalRecordsProperty);
        set => SetValue(TotalRecordsProperty, value);
    }

    public static readonly BindableProperty RecordsOnPageProperty = BindableProperty.Create(
        propertyName: nameof(RecordsOnPage),
        returnType: typeof(int),
        declaringType: typeof(CollectionPaginationControl),
        defaultBindingMode: BindingMode.OneWay);
   
    public int RecordsOnPage
    {
        get => (int)GetValue(RecordsOnPageProperty);
        set => SetValue(RecordsOnPageProperty, value);
    }   

    public static readonly BindableProperty LoadMoreCollectionDataProperty = BindableProperty.Create(
        propertyName: nameof(LoadMoreCollectionData),
        returnType: typeof(IAsyncRelayCommand),
        declaringType: typeof(CollectionPaginationControl),
        defaultBindingMode: BindingMode.OneWay);

    public IAsyncRelayCommand LoadMoreCollectionData
    {
        get => (IAsyncRelayCommand)GetValue(LoadMoreCollectionDataProperty);
        set => SetValue(LoadMoreCollectionDataProperty, value);
    }

    public static readonly BindableProperty LoadMoreCollectionDataParameterProperty = BindableProperty.Create(
        propertyName: nameof(LoadMoreCollectionDataParameter),
        returnType: typeof(object),
        declaringType: typeof(CollectionPaginationControl),
        defaultBindingMode: BindingMode.OneWay);

    public object LoadMoreCollectionDataParameter
    {
        get => (object)GetValue(LoadMoreCollectionDataParameterProperty);
        set => SetValue(LoadMoreCollectionDataParameterProperty, value);
    }

    private static readonly BindableProperty IsLoadingMoreDataProperty = BindableProperty.Create(
       propertyName: nameof(IsLoadingMoreData),
       returnType: typeof(bool),
       declaringType: typeof(CollectionPaginationControl),
       defaultValue: false,
       propertyChanged: IsLoadingMoreDataPropertyChanged);

    private static void IsLoadingMoreDataPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var controls = (CollectionPaginationControl)bindable;
        bool isLoading = (bool)newValue;

        if(isLoading)
        {
            controls.CurrentPageLabel.IsVisible = false;
            controls.IsLoadingMoreDataIndicator.IsVisible = true;
            controls.IsLoadingMoreDataIndicator.IsRunning = true;
            controls.MoveNextButton.IsEnabled = false;
            controls.MovePreviousButton.IsEnabled = false;
        }
        else
        {
            controls.CurrentPageLabel.IsVisible = true;
            controls.IsLoadingMoreDataIndicator.IsVisible = false;
            controls.IsLoadingMoreDataIndicator.IsRunning = false;           
        }
    }

    private bool IsLoadingMoreData
    {
        get => (bool)GetValue(IsLoadingMoreDataProperty);
        set => SetValue(IsLoadingMoreDataProperty, value);
    }

    public static readonly BindableProperty IsRefreshingCollectionProperty = BindableProperty.Create(
       propertyName: nameof(IsRefreshingCollection),
       returnType: typeof(bool),
       declaringType: typeof(CollectionPaginationControl),
       defaultValue: false,
       propertyChanged: IsRefreshingCollectionPropertyChanged);

    private static void IsRefreshingCollectionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var controls = (CollectionPaginationControl)bindable;
        bool isRefreshing = (bool)newValue;

        if (!isRefreshing)
        {
            controls.CurrentPage = 1;

            if (controls.TotalRecords == 0)
            {
                controls.TotalPagesCount = 1;
                controls.ControlsLayout.IsVisible = false;
                controls.EmptyViewLayout.IsVisible = true;
            }
            else
            {
                double pagesCountDouble = controls.TotalRecords / (double)controls.RecordsOnPage;
                int totalPagesCount = (int)Math.Ceiling(pagesCountDouble);

                if (totalPagesCount > 1) controls.MoveNextButton.IsEnabled = true;
                controls.TotalPagesCount = totalPagesCount;
            }

            controls.ChangeCurrentPage();
        }     
        else
        {
            controls.IsInitialization = true;
        }
    }

    public bool IsRefreshingCollection
    {
        get => (bool)GetValue(IsRefreshingCollectionProperty);
        set => SetValue(IsRefreshingCollectionProperty, value);
    }

    public static readonly BindableProperty CollectionHeaderProperty = BindableProperty.Create(
      propertyName: nameof(CollectionHeader),
      returnType: typeof(string),
      declaringType: typeof(CollectionPaginationControl),
      defaultBindingMode: BindingMode.OneWay,
      propertyChanged: CollectionHeaderPropertyChanged);

    private static void CollectionHeaderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var controls = (CollectionPaginationControl)bindable;
        string newHeaderValue = (string)newValue;

        if(!string.IsNullOrEmpty(newHeaderValue))
        {
            controls.PaginationCollectionHeader.Text = newHeaderValue;
        }
    }

    public string CollectionHeader
    {
        get => (string)GetValue(CollectionHeaderProperty);
        set => SetValue(CollectionHeaderProperty, value);
    }

    public static readonly BindableProperty CollectionEmptyViewTextProperty = BindableProperty.Create(
      propertyName: nameof(CollectionEmptyViewText),
      returnType: typeof(string),
      declaringType: typeof(CollectionPaginationControl),
      defaultBindingMode: BindingMode.OneWay,
      propertyChanged: CollectionEmptyViewTextPropertyChanged);

    private static void CollectionEmptyViewTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var controls = (CollectionPaginationControl)bindable;
        string newEmptyViewText = (string)newValue;

        if (!string.IsNullOrEmpty(newEmptyViewText))
        {
            controls.EmptyViewLabel.Text = newEmptyViewText;
        }
    }

    public string CollectionEmptyViewText
    {
        get => (string)GetValue(CollectionEmptyViewTextProperty);
        set => SetValue(CollectionEmptyViewTextProperty, value);
    }

    public static readonly BindableProperty CollectionItemTemplateProperty = BindableProperty.Create(
      propertyName: nameof(CollectionItemTemplate),
      returnType: typeof(DataTemplate),
      declaringType: typeof(CollectionPaginationControl),
      defaultBindingMode: BindingMode.OneWay,
      propertyChanged: CollectionItemTemplatePropertyChanged);

    private static void CollectionItemTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var controls = (CollectionPaginationControl)bindable;
        var newItemTemplateValue = (DataTemplate)newValue;

        if(newItemTemplateValue != null)
        {
            controls.PaginationCollectionView.ItemTemplate = newItemTemplateValue;
        }
    }

    public DataTemplate CollectionItemTemplate
    {
        get => (DataTemplate)GetValue(CollectionItemTemplateProperty);
        set => SetValue(CollectionItemTemplateProperty, value);
    }
    
    public static readonly BindableProperty CollectionItemSourceProperty = BindableProperty.Create(
     propertyName: nameof(CollectionItemSource),
     returnType: typeof(IEnumerable<object>),
     declaringType: typeof(CollectionPaginationControl),
     defaultBindingMode: BindingMode.OneWay,
     propertyChanged: CollectionItemSourcePropertyChanged);

    private static void CollectionItemSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var controls = (CollectionPaginationControl)bindable;
        var newItemSource = (IEnumerable<object>)newValue;

        if (newItemSource != null)
        {            
            controls.AllRecordsCollection = newItemSource.ToList();
            controls.PaginationCollectionView.ItemsSource = newItemSource;
        }
    }

    public IEnumerable<object> CollectionItemSource
    {
        get => (IEnumerable<object>)GetValue(CollectionItemSourceProperty);
        set => SetValue(CollectionItemSourceProperty, value);
    }

    public static readonly BindableProperty AllItemsCollectionProperty = BindableProperty.Create(
     propertyName: nameof(AllItemsCollection),
     returnType: typeof(IEnumerable<object>),
     declaringType: typeof(CollectionPaginationControl),
     defaultBindingMode: BindingMode.OneWay,
     propertyChanged: CollectionItemSourcePropertyChanged);
    
    public IEnumerable<object> AllItemsCollection
    {
        get => (IEnumerable<object>)GetValue(AllItemsCollectionProperty);
        set => SetValue(AllItemsCollectionProperty, value);
    }

    private async void MoveNextButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            IsLoadingMoreData = true;
            CurrentPage++;

            if (AllRecordsCollection.Count < TotalRecords & (CurrentPage * RecordsOnPage) > AllRecordsCollection.Count)
            {               
                await LoadMoreCollectionData?.ExecuteAsync(LoadMoreCollectionDataParameter);
            }           

            SetCollectionItemSource();
            ChangeCurrentPage();
            SetButtons();

            IsLoadingMoreData = false;
        }
        catch(Exception)
        {
            IsLoadingMoreData = false;            
            ControlsLayout.IsVisible = false;
            PaginationCollectionView.IsVisible = false;
            OperationFailedLayout.IsVisible = true;
        }
    }

    private void MovePreviousButton_Clicked(object sender, EventArgs e)
    {
        IsLoadingMoreData = true;
        CurrentPage--;       
        
        SetCollectionItemSource();
        ChangeCurrentPage();
        SetButtons();

        IsLoadingMoreData = false;
    }

    private void SetButtons()
    {
        if (CurrentPage > 1) MovePreviousButton.IsEnabled = true;
        if (CurrentPage < TotalPagesCount) MoveNextButton.IsEnabled = true;
    }

    private void SetCollectionItemSource()
    {
        int skippedRecords = (CurrentPage - 1) * RecordsOnPage;

        var newCollectionValues = AllItemsCollection.Skip(skippedRecords).Take(RecordsOnPage);
        PaginationCollectionView.ItemsSource = newCollectionValues; 
    }

    private void ChangeCurrentPage()
    {
        CurrentPageLabel.Text = $"{CurrentPage}/{TotalPagesCount}";
    }    

    private void ContentView_Loaded(object sender, EventArgs e)
    {
        if (TotalRecords == 0)
        {
            TotalPagesCount = 1;
            ControlsLayout.IsVisible = false;
            EmptyViewLayout.IsVisible = true;
        }
        else
        {
            double pagesCountDouble = TotalRecords / (double)RecordsOnPage;
            int totalPagesCount = (int)Math.Ceiling(pagesCountDouble);

            if(totalPagesCount > 1) MoveNextButton.IsEnabled = true;
            TotalPagesCount = totalPagesCount;
        }

        ChangeCurrentPage();
    }

    private void RetryButton_Clicked(object sender, EventArgs e)
    {
        ControlsLayout.IsVisible = true;
        PaginationCollectionView.IsVisible = true;
        OperationFailedLayout.IsVisible = false;
        CurrentPage--;
        MoveNextButton_Clicked(sender, e);
    }

    private void BackToPreviousButton_Clicked(object sender, EventArgs e)
    {
        ControlsLayout.IsVisible = true;
        PaginationCollectionView.IsVisible = true;
        OperationFailedLayout.IsVisible = false;
        MovePreviousButton_Clicked(sender, e);
    }
}