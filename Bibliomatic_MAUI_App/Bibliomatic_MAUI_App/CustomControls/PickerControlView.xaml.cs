using CommunityToolkit.Maui.Views;
using System.Collections;

namespace Bibliomatic_MAUI_App.CustomControls;

public partial class PickerControlView : Popup
{
	public PickerControlView(IEnumerable itemSource, DataTemplate itemTemplate, double pickerControlHeight)
	{
		InitializeComponent();

		clPickerView.ItemsSource = itemSource;
		clPickerView.ItemTemplate = itemTemplate;
		clPickerView.HeightRequest = pickerControlHeight;
	}

    private async void clPickerView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
		var currentItem = e.CurrentSelection.FirstOrDefault();
		await CloseAsync(currentItem);		
    }
}