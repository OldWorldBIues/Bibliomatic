using CommunityToolkit.Maui.Views;
using Syncfusion.Maui.DataSource.Extensions;

namespace Bibliomatic_MAUI_App.CustomControls;

public class CurrentItemChangedEventArgs : EventArgs
{
    public object OldItemValue { get; }
    public object NewItemValue { get; }

    public CurrentItemChangedEventArgs(object oldItemValue, object newItemValue)
    {
        OldItemValue = oldItemValue;
        NewItemValue = newItemValue;
    }
}

public partial class DropDownButton : StackLayout
{
    public DropDownButton()
    {
        InitializeComponent();
    }

    private void ShuffleList<T>(IList<T> list) where T : class
    {
        var random = new Random();

        for (int i = list.Count - 1; i > 1; i--)
        {
            int randomListIndex = random.Next(i + 1);

            var listElement = list[randomListIndex];
            list[randomListIndex] = list[i];
            list[i] = listElement;
        }
    }

    IEnumerable<object> shuffledItemSource = null;

    public IEnumerable<object> ShuffledItemSource
    {
        get
        {
            if(shuffledItemSource == null)
            {
                var itemSource = ItemSource.ToList();
                ShuffleList(itemSource);
                shuffledItemSource = itemSource;
            }

            return shuffledItemSource;
        }
    }

    public event EventHandler<CurrentItemChangedEventArgs> CurrentItemChanged;

    public static readonly BindableProperty CurrentItemProperty = BindableProperty.Create(
        propertyName: nameof(CurrentItem),
        returnType: typeof(object),
        declaringType: typeof(DropDownButton),
        propertyChanged: CurrentItemPropertyChanged,
        defaultBindingMode: BindingMode.TwoWay);

    private static void CurrentItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var controls = (DropDownButton)bindable;
        bool leftControlEnabled = false, rightControlEnabled = false;

        if (newValue != null)
        {
            var args = new CurrentItemChangedEventArgs(oldValue, newValue);
            controls.CurrentItemChanged?.Invoke(controls, args);

            int indexOfCurrentItem = controls.ItemSource.ToList().IndexOf(controls.CurrentItem);

            if(indexOfCurrentItem != -1)
            {
                if (indexOfCurrentItem != 0)
                    leftControlEnabled = true;

                if (indexOfCurrentItem != controls.ItemSource.Count() - 1)
                    rightControlEnabled = true;
            }

            if (!string.IsNullOrEmpty(controls.DisplayedMember))
            {
                var displayedText = newValue.GetType().GetProperty(controls.DisplayedMember).GetValue(newValue, null).ToString();

                if (!string.IsNullOrEmpty(controls.StringFormat))                
                    displayedText = $"{controls.StringFormat}{displayedText}";              

                controls.lblDropDown.Text = displayedText;
            }
        }
        else
            controls.lblDropDown.Text = controls.Placeholder;

        controls.leftSliderButton.IsEnabled = leftControlEnabled;
        controls.rightSliderButton.IsEnabled = rightControlEnabled;
    }

    public object CurrentItem
    {
        get => (object)GetValue(CurrentItemProperty);
        set => SetValue(CurrentItemProperty, value);
    }

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
        propertyName: nameof(Placeholder),
        returnType: typeof(string),
        declaringType: typeof(DropDownButton),
        propertyChanged: PlaceholderPropertyChanged,
        defaultBindingMode: BindingMode.TwoWay);

    private static void PlaceholderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var controls = (DropDownButton)bindable;

        if (controls.CurrentItem == null)
        {
            if (newValue != null)
            {
                controls.lblDropDown.Text = newValue.ToString();
            }
        }
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public static readonly BindableProperty ItemSourceProperty = BindableProperty.Create(
        propertyName: nameof(ItemSource),
        returnType: typeof(IEnumerable<object>),
        declaringType: typeof(DropDownButton),
        defaultBindingMode: BindingMode.OneWay);   

    public IEnumerable<object> ItemSource
    {
        get => (IEnumerable<object>)GetValue(ItemSourceProperty);
        set => SetValue(ItemSourceProperty, value);
    }
    
    private static void IsDisplaySliderControlsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var controls = (DropDownButton)bindable;

        if (newValue != null)
        {
            if ((bool)newValue)
            {
                controls.leftSliderButton.IsVisible = true;
                controls.rightSliderButton.IsVisible = true;
            }
        }
    }

    public static readonly BindableProperty IsDisplaySliderControlsProperty = BindableProperty.Create(
        propertyName: nameof(IsDisplaySliderControls),
        returnType: typeof(bool),
        declaringType: typeof(DropDownButton),
        propertyChanged: IsDisplaySliderControlsPropertyChanged,
        defaultBindingMode: BindingMode.TwoWay);

    public bool IsDisplaySliderControls
    {
        get => (bool)GetValue(IsDisplaySliderControlsProperty);
        set => SetValue(IsDisplaySliderControlsProperty, value);
    }
    
    public static readonly BindableProperty ShuffleItemSourceProperty = BindableProperty.Create(
        propertyName: nameof(ShuffleItemSource),
        returnType: typeof(bool),
        declaringType: typeof(DropDownButton),       
        defaultBindingMode: BindingMode.TwoWay);

    public bool ShuffleItemSource
    {
        get => (bool)GetValue(ShuffleItemSourceProperty);
        set => SetValue(ShuffleItemSourceProperty, value);
    }

    public DataTemplate ItemTemplate { get; set; }
    public double PickerHeightRequest { get; set; }
    public string DisplayedMember { get; set; }
    public string StringFormat { get; set; }

    private async void DropDownButtonTapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {       
        var itemSource = ShuffleItemSource ? ShuffledItemSource : ItemSource;
        var response = await Application.Current.MainPage.ShowPopupAsync(new PickerControlView(itemSource, ItemTemplate, PickerHeightRequest));

        if (response != null)
        {
            CurrentItem = response;
        }
    }

    private void RightSliderButton_Clicked(object sender, EventArgs e)
    {
        int currentItemIndex = ItemSource.ToList().IndexOf(CurrentItem);
        CurrentItem = ItemSource.ElementAt(currentItemIndex + 1);        
    }

    private void LeftSliderButton_Clicked(object sender, EventArgs e)
    {
        int currentItemIndex = ItemSource.ToList().IndexOf(CurrentItem);
        CurrentItem = ItemSource.ElementAt(currentItemIndex - 1);
    }
}



