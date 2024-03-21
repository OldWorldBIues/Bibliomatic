using CommunityToolkit.Mvvm.ComponentModel;
using Bibliomatic_MAUI_App.Models;

namespace Bibliomatic_MAUI_App.ViewModels
{
    [QueryProperty(nameof(Test), "Test")]
    public partial class CreatorTestViewModel : ObservableObject
    {
        [ObservableProperty]
        public TestResponse test;
    }
}
