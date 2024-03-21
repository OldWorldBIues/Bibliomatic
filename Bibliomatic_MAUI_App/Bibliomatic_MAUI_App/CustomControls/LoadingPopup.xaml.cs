using CommunityToolkit.Maui.Views;

namespace Bibliomatic_MAUI_App.CustomControls;

public partial class LoadingPopup : Popup
{
    private readonly string routeToNavigate;
    private readonly Dictionary<string, object> navigationParameters;

	public LoadingPopup(string operation)
	{
		InitializeComponent();
		OperationLabel.Text = operation;      
	}	   

    public LoadingPopup(string operation, string route, Dictionary<string, object> parameters)
    {
        InitializeComponent();
        OperationLabel.Text = operation;

        routeToNavigate = route;
        navigationParameters = parameters;
    }
    
    public void ChangeToCompleted(string text)
    {
        CompletedButton.Text = text;
        OperationLabel.Text = "Successfully completed";

        CompletedButton.IsVisible = true;
        CompletedImage.IsVisible = true;
        ProcessIndicator.IsVisible = false;
    }    
   
    private async void CompletedButton_Clicked(object sender, EventArgs e)
    {
        OperationLabel.Text = "Navigating...";
        CompletedButton.IsVisible = false;
        CompletedImage.IsVisible = false;
        ProcessIndicator.IsVisible = true;
       
        if (!string.IsNullOrEmpty(routeToNavigate))
        {
            if (navigationParameters != null)
            {
                await Shell.Current.GoToAsync(routeToNavigate, true, navigationParameters);
            }
            else
            {
                await Shell.Current.GoToAsync(routeToNavigate, true);
            }
        }
        else
        {
            await Shell.Current.Navigation.PopToRootAsync(false);
        }

        Close();
    }    
}