using CommunityToolkit.Mvvm.ComponentModel;

namespace Bibliomatic_MAUI_App.ViewModels
{
    [QueryProperty(nameof(Header), "Header")]
    [QueryProperty(nameof(ElementHtml), "ElementHtml")]
    [QueryProperty(nameof(QuestionHeader), "QuestionHeader")]
    [QueryProperty(nameof(QuestionDescription), "QuestionDescription")]
    public partial class QuestionBodyViewModel : ObservableObject
    {
        [ObservableProperty]
        public HtmlWebViewSource elementHtml;

        [ObservableProperty]
        public string questionHeader;

        [ObservableProperty]
        public string questionDescription;

        [ObservableProperty]
        public string header;
    }
}
