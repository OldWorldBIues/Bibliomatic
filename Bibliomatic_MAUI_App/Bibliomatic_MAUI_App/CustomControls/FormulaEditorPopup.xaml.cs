namespace Bibliomatic_MAUI_App.CustomControls;

public partial class FormulaEditorPopup : ContentView
{
    private TaskCompletionSource<string> formulaEditTask;

    public FormulaEditorPopup()
	{
		InitializeComponent();		
	}

    public static readonly BindableProperty IsControlVisibleProperty = BindableProperty.Create(
        propertyName: nameof(IsControlVisible),
        returnType: typeof(bool),
        declaringType: typeof(FormulaEditorPopup),
        propertyChanged: IsControlVisiblePropertyChanged,
        defaultBindingMode: BindingMode.TwoWay);

    private static void IsControlVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var controls = (FormulaEditorPopup)bindable;
        controls.ContentGrid.IsVisible = (bool)newValue;
    }

    public bool IsControlVisible
    {
        get => (bool)GetValue(IsControlVisibleProperty);
        set => SetValue(IsControlVisibleProperty, value);
    }

    private void AddFormulaButton_Clicked(object sender, EventArgs e)
    {
        string result = FormulaEditor.Formula;
        IsControlVisible = false;
        FormulaEditor.IsClearFormula = true;        

        formulaEditTask?.TrySetResult(result);        
    }
	
    private void ClosePopupButton_Clicked(object sender, EventArgs e)
    {
        string result = null;
        IsControlVisible = false;
        FormulaEditor.IsClearFormula = true;        

        formulaEditTask?.TrySetResult(result);
    }    

    public async Task<string> GetFormulaAsync(string existingFormula)
    {
        IsControlVisible = true;
    
        if(!string.IsNullOrEmpty(existingFormula))
        {
            FormulaEditor.ExistingFormula = existingFormula;
        }

        formulaEditTask = new TaskCompletionSource<string>();
        return await formulaEditTask.Task;        
    }

    public void ClearFormulaEditorControl() => FormulaEditor.IsClearFormula = true;
}