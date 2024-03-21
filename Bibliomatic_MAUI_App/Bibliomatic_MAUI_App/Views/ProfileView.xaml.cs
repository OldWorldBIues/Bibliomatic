using Bibliomatic_MAUI_App.ViewModels;

namespace Bibliomatic_MAUI_App.Views;

public partial class ProfileView : ContentPage
{
	public ProfileView(ProfileViewModel profileViewModel)
	{
		InitializeComponent();
		this.BindingContext = profileViewModel;
	}

    private void UserDataEditor_TextChanged(object sender, EventArgs e)
    {
        bool firstNameEditorEmpty = string.IsNullOrEmpty(FirstNameEditor.Text);
        bool lastNameEditorEmpty = string.IsNullOrEmpty(LastNameEditor.Text);
        bool middleNameEditorEmpty = string.IsNullOrEmpty(MiddleNameEditor.Text);

        SaveEditChangesButton.IsEnabled = !firstNameEditorEmpty & !lastNameEditorEmpty & !middleNameEditorEmpty;
    }

    private async void SaveEditChangesButton_Clicked(object sender, EventArgs e)
    {
        EditProfileBorder.IsVisible = false;
        ContentScrollView.IsVisible = true;

        var viewModel = (ProfileViewModel)BindingContext;
        await viewModel.SaveEditChanges();
    }

    private void CloseEditImageButton_Clicked(object sender, EventArgs e)
    {
        EditProfileBorder.IsVisible = false;
        ContentScrollView.IsVisible = true;
    }

    private void EditProfileImageButton_Clicked(object sender, EventArgs e)
    {
        EditProfileBorder.IsVisible = true;
        ContentScrollView.IsVisible = false;
    }
}