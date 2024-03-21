using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Bibliomatic_MAUI_App.Models
{
    public partial class FormulaCategory : ObservableObject
    {
        public string Category { get; set; }

        [ObservableProperty]
        public Formula selectedFormula;

        public ObservableCollection<Formula> Formulas { get; set; }
    }
}
