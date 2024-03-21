using Bibliomatic_MAUI_App.Models;
using CommunityToolkit.Maui.Alerts;
using CSharpMath.SkiaSharp;

namespace Bibliomatic_MAUI_App.CustomControls;

public partial class FormulasEditor : ContentView
{
    public static readonly BindableProperty FormulaProperty = BindableProperty.Create(
        propertyName: nameof(Formula),
        returnType: typeof(string),
        declaringType: typeof(FormulasEditor),
        propertyChanged: FormulaPropertyChanged,
        defaultBindingMode: BindingMode.TwoWay);

    private static void FormulaPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var controls = (FormulasEditor)bindable;        
        string newFormula = (string)newValue;

        if (!string.IsNullOrEmpty(newFormula))
        {            
            controls.FormulaImage.Source = ImageSource.FromStream(() => Models.Formula.GetFormulaStream(newFormula, controls.mathPainter));  
            controls.FormulaEditor.Text = newFormula;
        }      
        else
        {            
            controls.FormulaImage.Source = "formula_default.png";
        }
    }   

    public string Formula
    {
        get => (string)GetValue(FormulaProperty);
        set => SetValue(FormulaProperty, value);
    }

    public static readonly BindableProperty ExistingFormulaProperty = BindableProperty.Create(
        propertyName: nameof(ExistingFormula),
        returnType: typeof(string),
        declaringType: typeof(FormulasEditor),
        propertyChanged: ExistingFormulaPropertyChanged,
        defaultBindingMode: BindingMode.TwoWay);

    private static void ExistingFormulaPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var controls = (FormulasEditor)bindable;        
        controls.FormulaEditor.Text = (string)newValue;
    }

    public string ExistingFormula
    {
        get => (string)GetValue(ExistingFormulaProperty);
        set => SetValue(ExistingFormulaProperty, value);
    }

    public static readonly BindableProperty IsClearFormulaProperty = BindableProperty.Create(
        propertyName: nameof(IsClearFormula),
        returnType: typeof(bool),
        declaringType: typeof(FormulasEditor),
        propertyChanged: IsClearFormulaPropertyChanged,
        defaultBindingMode: BindingMode.TwoWay);

    private static void IsClearFormulaPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var controls = (FormulasEditor)bindable;
        bool isClearFormula = (bool)newValue;

        if(isClearFormula)
        {
            controls.TextHistory.ChangedByControls = true;
            controls.FormulaEditor.Text = string.Empty;
            controls.Formula = string.Empty;
            controls.ExistingFormula = string.Empty;
            controls.IsClearFormula = false;
            controls.TextHistory.ClearTextHistory();
        }
    }

    public bool IsClearFormula
    {
        get => (bool)GetValue(IsClearFormulaProperty);
        set => SetValue(IsClearFormulaProperty, value);
    }
    
    MathPainter mathPainter = new() { FontSize = 20 }; 
   
    public FormulasEditor()
    {
        InitializeComponent();
        FormulaCategoryPicker.ItemSource = FormulasCategories.FormulaCategories;      
    }    

    private async void CopyToClipboardButton_Clicked(object sender, EventArgs e)
    {
        var selectedFormula = (Formula)LatexFormulasCollectionView.SelectedItem;
        string clipboardText = selectedFormula.Latex;

        await Toast.Make("Formula was copied to clipboard").Show();
        await Clipboard.Default.SetTextAsync(clipboardText);
    }

    private void NewLineButton_Clicked(object sender, EventArgs e)
    {
        int cursorPosition = FormulaEditor.CursorPosition;

        string newLine = @"\\";
        string editorText = FormulaEditor.Text ?? string.Empty;

        TextHistory.ClearRedoHistory();
        FormulaEditor.Text = editorText.Insert(cursorPosition, newLine);        
        FormulaEditor.CursorPosition = cursorPosition + newLine.Length;
    }

    private void InsertFormulaButton_Clicked(object sender, EventArgs e)
    {
        var selectedFormula = (Formula)LatexFormulasCollectionView.SelectedItem;
        int cursorPosition = FormulaEditor.CursorPosition;

        string latex = selectedFormula.Latex;
        string editorText = FormulaEditor.Text ?? string.Empty;

        TextHistory.ClearRedoHistory();
        FormulaEditor.Text = editorText.Insert(cursorPosition, latex);        
        FormulaEditor.CursorPosition = cursorPosition + latex.Length;
    }
    
    private void FormulaCategoryPicker_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
    {
        var currentItem = (FormulaCategory)e.NewItemValue;        
        LatexFormulasCollectionView.ItemsSource = currentItem.Formulas;

        if (HideFormulasButton.IsVisible)
            LatexFormulasCollectionView.IsVisible = true;

        HideFormulasButton.IsEnabled = true;
    }

    private void LatexFormulasCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedLatexFormula = (Formula)LatexFormulasCollectionView.SelectedItem;        
        bool descriptionVisible = false;

        if (!string.IsNullOrEmpty(selectedLatexFormula.Description))
            descriptionVisible = true;

        LatexDescriptionLabel.Text = selectedLatexFormula.Description;
        LatexFormulaLabel.Text = selectedLatexFormula.Latex;
        LatexFormulaImage.Source = selectedLatexFormula.FormulaImageSource;
        LatexFormulaImage.Scale = selectedLatexFormula.ImageScale;

        LatexDescriptionBorder.IsVisible = descriptionVisible;
        FormulaViewGrid.IsVisible = true;
        InsertFormulaButton.IsEnabled = true;
    }
    
    private void HideFormulasButton_Clicked(object sender, EventArgs e)
    {
        LatexFormulasCollectionView.IsVisible = false;
        ShowFormulasButton.IsVisible = true;
        HideFormulasButton.IsVisible = false;
    }

    private void ShowFormulasButton_Clicked(object sender, EventArgs e)
    {
        LatexFormulasCollectionView.IsVisible = true;
        HideFormulasButton.IsVisible = true;
        ShowFormulasButton.IsVisible = false;
    }

    private void FormulaEditor_TextChanged(object sender, TextChangedEventArgs e)
    {
        Formula = e.NewTextValue;    
    }    
}