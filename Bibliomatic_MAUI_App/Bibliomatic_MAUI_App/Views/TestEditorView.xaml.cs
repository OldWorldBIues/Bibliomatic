using Bibliomatic_MAUI_App.ViewModels;
using Bibliomatic_MAUI_App.Models;
using Bibliomatic_MAUI_App.Helpers;
using System.Text.RegularExpressions;

namespace Bibliomatic_MAUI_App.Views;

public partial class TestEditorView : ContentPage
{
	public TestEditorView(TestEditorViewModel testEditorViewModel)
	{
		InitializeComponent();
		this.BindingContext = testEditorViewModel;		
	}

    public TestEditorViewModel TestEditorViewModel { get =>  (TestEditorViewModel)BindingContext; }

    private void SpacesTextEditor_TextChanged(object sender, TextChangedEventArgs e)
    {
		var editor = (Editor)sender;
        var viewModel = (TestEditorViewModel)BindingContext;
        var testQuestion = (TestQuestionResponse)editor.BindingContext;        

        if (!testQuestion.ChangedByControls)
        {
            int cursorPosition = editor.CursorPosition;

            string oldText = e.OldTextValue;
            string newText = e.NewTextValue;

            int oldTextLength = string.IsNullOrEmpty(oldText) ? 0 : oldText.Length;
            int newTextLength = string.IsNullOrEmpty(newText) ? 0 : newText.Length;
            int cursorOffset = newTextLength - oldTextLength;

            bool textInserted = newTextLength > oldTextLength;
            int textLength = Math.Abs(newTextLength - oldTextLength);
            cursorPosition = textInserted ? cursorPosition : Math.Abs(cursorPosition - textLength);

            string textForOperation = textInserted ? newText : oldText;
            string text = textForOperation.Substring(cursorPosition, textLength);

            for(int i = 0; i < GetRemovedTagsCount(text, textInserted); i++)
            {
                viewModel.ReturnLastDeletedTestAnswer(editor);
            }

            viewModel.UpdateTestQuestionCursorPositions(testQuestion, cursorOffset, cursorPosition);
        }
        
        testQuestion.ChangedByControls = false;        
        viewModel.ChangeHtml(testQuestion);
    }

    private int GetRemovedTagsCount(string text, bool isInserted)
    {
        if(isInserted)
        {
            return Regex.Matches(text, TextTags.SpaceRegexPattern).Count;
        }

        return 0;
    }

    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (!e.Value)
            return;

        var radioButton = (RadioButton)sender;
        var viewModel = (TestEditorViewModel)BindingContext;
        var spaceTestVariant = (SpaceTestVariantResponse)radioButton.BindingContext; 
        var testQuestion = spaceTestVariant.TestAnswer.TestQuestion;

        testQuestion.CorrectSpacesValues.Clear();
        viewModel.ChangeHtml(testQuestion);
    }

    private async Task<string> PickFormula(string existingFormula)
    {
        bool permissionsGranted = await PermissionsChecker.CheckForRequiredPermissions();
        if (!permissionsGranted) return null;

        var formula = await FormulasEditor.GetFormulaAsync(existingFormula);

        if (string.IsNullOrEmpty(formula)) return null;

        return formula;
    }
    private async void PickTestQuestionFormulaButton_Clicked(object sender, EventArgs e)
    {
        var imageButton = (ImageButton)sender;
        var testQuestion = (TestQuestionResponse)imageButton.BindingContext;

        string existingFormula = testQuestion.TestQuestionLatexFormula ?? string.Empty;
        string formula = await PickFormula(existingFormula);

        if (string.IsNullOrEmpty(formula)) return;

        await TestEditorViewModel.SetTestQuestionFormulaImage(testQuestion, formula);
    }

    private async void PickTestAnswerFormulaButton_Clicked(object sender, EventArgs e)
    {
        var imageButton = (ImageButton)sender;
        var testAnswer = (TestAnswerResponse)imageButton.BindingContext;        

        string existingFormula = testAnswer.TestAnswerLatexFormula ?? string.Empty;
        string formula = await PickFormula(existingFormula);

        if (string.IsNullOrEmpty(formula)) return;

        await TestEditorViewModel.SetTestAnswerFormulaImage(testAnswer, formula);
    }

    private async void PickTestVariantFormulaButton_Clicked(object sender, EventArgs e)
    {
        var imageButton = (ImageButton)sender;
        var testAnswer = (TestAnswerResponse)imageButton.BindingContext;

        string existingFormula = testAnswer.TestVariantLatexFormula ?? string.Empty;
        string formula = await PickFormula(existingFormula);

        if (string.IsNullOrEmpty(formula)) return;

        await TestEditorViewModel.SetTestVariantFormulaImage(testAnswer, formula);
    }

    private void RemoveTestQuestionFormulaButton_Clicked(object sender, EventArgs e)
    {
        var imageButton = (ImageButton)sender;
        var testQuestion = (TestQuestionResponse)imageButton.BindingContext;

        FormulasEditor.ClearFormulaEditorControl();
        TestEditorViewModel.RemoveTestQuestionFormulaImage(testQuestion);
    }

    private void RemoveTestAnswerFormulaButton_Clicked(object sender, EventArgs e)
    {
        var imageButton = (ImageButton)sender;
        var testAnswer = (TestAnswerResponse)imageButton.BindingContext;

        FormulasEditor.ClearFormulaEditorControl();
        TestEditorViewModel.RemoveTestAnswerFormulaImage(testAnswer);
    }

    private void RemoveTestVariantFormulaButton_Clicked(object sender, EventArgs e)
    {
        var imageButton = (ImageButton)sender;
        var testAnswer = (TestAnswerResponse)imageButton.BindingContext;

        FormulasEditor.ClearFormulaEditorControl();
        TestEditorViewModel.RemoveTestVariantFormulaImage(testAnswer);
    }
}