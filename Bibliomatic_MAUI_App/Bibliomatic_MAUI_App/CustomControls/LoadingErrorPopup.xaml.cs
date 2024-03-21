using CommunityToolkit.Maui.Views;

namespace Bibliomatic_MAUI_App.CustomControls;

public partial class LoadingErrorPopup : Popup
{
	public LoadingErrorPopup()
	{
		InitializeComponent();
	}

    public LoadingErrorPopup(string errorMessage, bool isUnauthorized)
    {
        InitializeComponent();

        if (isUnauthorized) ChangeToUnauthorized(errorMessage);
        else ChangeToError(errorMessage);
    }

    public void ChangeToError(string errorMessage)
    {
        OperationLabel.Text = errorMessage + ". Please try again or later";        
    }

    public void ChangeToUnauthorized(string errorMessage)
    {
        OperationLabel.Text = errorMessage;
        RetryButton.IsVisible = false;
        ReauthorizedButton.IsVisible = true;
    }

    private async void RetryButton_Clicked(object sender, EventArgs e) => await CloseAsync(true);
    private async void CloseButton_Clicked(object sender, EventArgs e) => await CloseAsync(false);
    private async void ReauthorizedButton_Clicked(object sender, EventArgs e) => await CloseAsync(true);    
}