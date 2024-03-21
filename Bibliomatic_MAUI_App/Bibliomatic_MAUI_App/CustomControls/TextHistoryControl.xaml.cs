using Bibliomatic_MAUI_App.Helpers;
using System.Text.RegularExpressions;

namespace Bibliomatic_MAUI_App.CustomControls;

public partial class TextHistoryControl : ContentView
{
    public static readonly BindableProperty ObservableTextProperty = BindableProperty.Create(
       propertyName: nameof(ObservableText),
       returnType: typeof(string),
       declaringType: typeof(TextHistoryControl),
       propertyChanged: ObservableTextPropertyChanged,
       defaultBindingMode: BindingMode.TwoWay);  

    private int GetCursorPosition(string oldTextValue, string newTextValue)
    {
        var oldTextValueChars = oldTextValue.ToCharArray();
        var newTextValueChars = newTextValue.ToCharArray();
        int length = oldTextValueChars.Length < newTextValueChars.Length ? oldTextValueChars.Length : newTextValueChars.Length;

        for(int i = 0; i < length; i++)
        {
            if (oldTextValueChars[i] != newTextValueChars[i])
            {
                return i;
            }
        }

        return length;
    }

    private static void ObservableTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var controls = (TextHistoryControl)bindable;

        try
        {
            string oldTextValue = (string)oldValue ?? string.Empty;
            string newTextValue = (string)newValue ?? string.Empty;

            if (!controls.ChangedByControls)
            {
                if (controls.ShouldClearRedoHistory) controls.textCommandManager.ClearRedoHistory();

                int cursorPosition = controls.GetCursorPosition(oldTextValue, newTextValue);
                int oldTextLength = oldTextValue?.Length ?? 0;
                int newTextLength = newTextValue?.Length ?? 0;

                bool textInserted = newTextLength > oldTextLength;
                TextAction action = textInserted ? TextAction.Insert : TextAction.Remove;

                int textLength = Math.Abs(newTextLength - oldTextLength);
                string textForOperation = textInserted ? newTextValue : oldTextValue;

                string text = textForOperation.Substring(cursorPosition, textLength);
                controls.textCommandManager.ChangeText(action, text, cursorPosition);
            }

            controls.UndoButton.IsEnabled = controls.textCommandManager.CanUndo;
            controls.RedoButton.IsEnabled = controls.textCommandManager.CanRedo;

            controls.ChangedByControls = false;
            controls.ShouldClearRedoHistory = false;
        }
        catch
        {
            controls.ClearTextHistory();
        }
    }

    public string ObservableText
    {
        get => (string)GetValue(ObservableTextProperty);
        set => SetValue(ObservableTextProperty, value);
    }

    public static readonly BindableProperty CursorPositionProperty = BindableProperty.Create(
       propertyName: nameof(CursorPosition),
       returnType: typeof(int),
       declaringType: typeof(TextHistoryControl), 
       defaultValue: 0,
       defaultBindingMode: BindingMode.TwoWay);

    public int CursorPosition
    {
        get => (int)GetValue(CursorPositionProperty);
        set => SetValue(CursorPositionProperty, value);
    }

    public static readonly BindableProperty SelectionLengthProperty = BindableProperty.Create(
       propertyName: nameof(SelectionLength),
       returnType: typeof(int),
       declaringType: typeof(TextHistoryControl),
       defaultValue: 0,
       defaultBindingMode: BindingMode.TwoWay);

    public int SelectionLength
    {
        get => (int)GetValue(SelectionLengthProperty);
        set => SetValue(SelectionLengthProperty, value);
    }

    public static readonly BindableProperty ShowAdditionalTextControlsProperty = BindableProperty.Create(
       propertyName: nameof(ShowAdditionalTextControls),
       returnType: typeof(bool),
       declaringType: typeof(TextHistoryControl),
       propertyChanged: ShowAdditionalControlsPropertyChanged,
       defaultBindingMode: BindingMode.TwoWay);

    private static void ShowAdditionalControlsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var controls = (TextHistoryControl)bindable;
        controls.AdditionalControlsLayout.IsVisible = (bool)newValue;
    }

    public bool ShowAdditionalTextControls
    {
        get => (bool)GetValue(ShowAdditionalTextControlsProperty);
        set => SetValue(ShowAdditionalTextControlsProperty, value);
    }

    public static readonly BindableProperty ShowFormattingTipsProperty = BindableProperty.Create(
       propertyName: nameof(ShowFormattingTips),
       returnType: typeof(bool),
       declaringType: typeof(TextHistoryControl),
       propertyChanged: ShowFormattingTipsPropertyChanged,
       defaultBindingMode: BindingMode.TwoWay);

    private static void ShowFormattingTipsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var controls = (TextHistoryControl)bindable;
        controls.FormattingTipsExpander.IsVisible = (bool)newValue;
        controls.FormattingTipsBoxView.IsVisible = (bool)newValue;
    }

    public bool ShowFormattingTips
    {
        get => (bool)GetValue(ShowFormattingTipsProperty);
        set => SetValue(ShowFormattingTipsProperty, value);
    }

    public static readonly BindableProperty ChangedByControlsProperty = BindableProperty.Create(
      propertyName: nameof(ChangedByControls),
      returnType: typeof(bool),
      declaringType: typeof(TextHistoryControl),      
      defaultBindingMode: BindingMode.TwoWay);

    public bool ChangedByControls
    {
        get => (bool)GetValue(ChangedByControlsProperty);
        set => SetValue(ChangedByControlsProperty, value);
    }

    public event EventHandler<EventArgs> HyperlinkAddingEvent;

    public static readonly BindableProperty HyperlinkAddingCommandProperty = BindableProperty.Create(
        propertyName: nameof(HyperlinkAddingCommand),
        returnType: typeof(System.Windows.Input.ICommand),
        declaringType: typeof(TextHistoryControl),
        defaultBindingMode: BindingMode.OneWay);

    public System.Windows.Input.ICommand HyperlinkAddingCommand
    {
        get => (System.Windows.Input.ICommand)GetValue(HyperlinkAddingCommandProperty);
        set => SetValue(HyperlinkAddingCommandProperty, value);
    }

    public event EventHandler<EventArgs> ImageAddingEvent;

    public static readonly BindableProperty ImageAddingCommandProperty = BindableProperty.Create(
        propertyName: nameof(ImageAddingCommand),
        returnType: typeof(System.Windows.Input.ICommand),
        declaringType: typeof(TextHistoryControl),
        defaultBindingMode: BindingMode.OneWay);

    public System.Windows.Input.ICommand ImageAddingCommand
    {
        get => (System.Windows.Input.ICommand)GetValue(ImageAddingCommandProperty);
        set => SetValue(ImageAddingCommandProperty, value);
    }

    public event EventHandler<EventArgs> FormulaAddingEvent;

    public static readonly BindableProperty FormulaAddingCommandProperty = BindableProperty.Create(
        propertyName: nameof(FormulaAddingCommand),
        returnType: typeof(System.Windows.Input.ICommand),
        declaringType: typeof(TextHistoryControl),
        defaultBindingMode: BindingMode.OneWay);

    public System.Windows.Input.ICommand FormulaAddingCommand
    {
        get => (System.Windows.Input.ICommand)GetValue(FormulaAddingCommandProperty);
        set => SetValue(FormulaAddingCommandProperty, value);
    }

    private bool ShouldClearRedoHistory { get; set; }    
    private readonly TextCommandManager textCommandManager = new TextCommandManager();

    public TextHistoryControl()
	{
		InitializeComponent();        
	}

    private void RedoButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            ChangedByControls = true;
            ObservableText = textCommandManager.Redo();
            CursorPosition = ObservableText?.Length ?? 0;
        }
        catch
        {
            ClearTextHistory();
        }
    }

    private void UndoButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            ChangedByControls = true;
            ShouldClearRedoHistory = true;

            ObservableText = textCommandManager.Undo();
            CursorPosition = ObservableText?.Length ?? 0;
        }
        catch
        {
            ClearTextHistory();
        }
    }

    private string[] TagSplitter(string tag)
    {
        char separator = ';';
        var tagParts = tag.Split(separator);

        return tagParts;
    }

    private string GetOpeningTag(string tag)
    {
        var tagParts = TagSplitter(tag);
        return tagParts[0];
    }

    private string GetClosingTag(string tag)
    {
        var tagParts = TagSplitter(tag);
        return tagParts[^1];
    }

    private readonly Dictionary<string, string> acceptableTags = new()
    {
        {PairsTextTags.BoldTag, PairsTextTags.BoldRegexPattern},
        {PairsTextTags.ItalicTag, PairsTextTags.ItalicRegexPattern},
        {PairsTextTags.UnderlineTag, PairsTextTags.UnderlineRegexPattern},
        {PairsTextTags.BlockquoteTag, PairsTextTags.BlockquoteRegexPattern},
        {PairsTextTags.HeadingTag, PairsTextTags.HeadingRegexPattern }
    };
    
    private bool IsTagExistOnPosition(KeyValuePair<string, string> tag, string text, ref int start, ref int length)
    {
        int openingTagLength = GetOpeningTag(tag.Key).Length;
        int closingTagLength = GetClosingTag(tag.Key).Length;

        int tagStartPosition = start - openingTagLength;
        int fullMatchLength = length + openingTagLength + closingTagLength;        

        var tagMatches = Regex.Matches(text, tag.Value);
        bool tagFound = tagMatches.FirstOrDefault(m => m.Index == tagStartPosition & m.Length == fullMatchLength) != null;

        if(tagFound)
        {
            start = tagStartPosition;
            length = fullMatchLength;
        }

        return tagFound;
    }

    private bool AnyOfTagsExistOnPosition(List<KeyValuePair<string, string>> tags, string text, ref int start, ref int length)
    {
        foreach(var tag in tags)
        {
            if(IsTagExistOnPosition(tag, text, ref start, ref length))
            {
                tags.Remove(tag);
                return true;
            }
        }

        return false;
    }

    private bool IsTagFound(string text, string mainTagKey, int selectionLength, int cursorPosition, 
        out int tagStartPosition, out int tagFullLength)
    {
        var mainTag = acceptableTags.FirstOrDefault(t => t.Key == mainTagKey);
        var otherTags = acceptableTags.Where(t => t.Key != mainTagKey).ToList();
        int currentStartPosition = cursorPosition, currentFullMatchLength = selectionLength;

        tagStartPosition = -1;
        tagFullLength = -1;      

        for(int i = 0; i < acceptableTags.Count; i++)
        {
            if(IsTagExistOnPosition(mainTag, text, ref currentStartPosition, ref currentFullMatchLength))
            {
                tagStartPosition = currentStartPosition;
                tagFullLength = currentFullMatchLength;
                return true;
            }

            if (!AnyOfTagsExistOnPosition(otherTags, text, ref currentStartPosition, ref currentFullMatchLength))
                break;            
        }

        return false;          
    }

    private string AddPairTagToText(string tag, string text, int cursorPosition, int selectionLength)
    {
        string openingTag = GetOpeningTag(tag);
        string closingTag = GetClosingTag(tag);

        int openingTagStartRemovingPos = cursorPosition;
        int closingTagStartRemovingPos = cursorPosition + selectionLength;

        bool tagExistOnPosition = IsTagFound(text, tag, selectionLength, cursorPosition, out int tagPosition, out int tagLength);
        var action = tagExistOnPosition ? PairsAction.Regular : PairsAction.Changed;

        if (tagExistOnPosition)
        {
            openingTagStartRemovingPos = tagPosition + openingTag.Length;
            closingTagStartRemovingPos = tagPosition + tagLength - (openingTag.Length + closingTag.Length);
        }

        ChangedByControls = true;
        textCommandManager.ClearRedoHistory();

        return textCommandManager.ChangeWithTag(action, openingTagStartRemovingPos, closingTagStartRemovingPos, openingTag, closingTag);
    }     

    private void BoldButton_Clicked(object sender, EventArgs e)
    {
        if (SelectionLength == 0)
            return;        

        ObservableText = AddPairTagToText(PairsTextTags.BoldTag, ObservableText, CursorPosition, SelectionLength);
        CursorPosition = ObservableText?.Length ?? 0;      
    }

    private void ItalicButton_Clicked(object sender, EventArgs e)
    {
        if (SelectionLength == 0)
            return;

        ObservableText = AddPairTagToText(PairsTextTags.ItalicTag, ObservableText, CursorPosition, SelectionLength);
        CursorPosition = ObservableText?.Length ?? 0;
    }

    private void UnderlineButton_Clicked(object sender, EventArgs e)
    {
        if (SelectionLength == 0)
            return;

        ObservableText = AddPairTagToText(PairsTextTags.UnderlineTag, ObservableText, CursorPosition, SelectionLength);
        CursorPosition = ObservableText?.Length ?? 0;
    }

    private void BlockquoteButton_Clicked(object sender, EventArgs e)
    {
        if (SelectionLength == 0)
            return;

        ObservableText = AddPairTagToText(PairsTextTags.BlockquoteTag, ObservableText, CursorPosition, SelectionLength);
        CursorPosition = ObservableText?.Length ?? 0;
    }

    private void HeadingButton_Clicked(object sender, EventArgs e)
    {
        if (SelectionLength == 0)
            return;

        ObservableText = AddPairTagToText(PairsTextTags.HeadingTag, ObservableText, CursorPosition, SelectionLength);
        CursorPosition = ObservableText?.Length ?? 0;
    }

    private void HyperlinkButton_Clicked(object sender, EventArgs e)
    {
        HyperlinkAddingEvent?.Invoke(sender, e);
        HyperlinkAddingCommand?.Execute(null);
        ClearRedoHistory();
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        ImageAddingEvent?.Invoke(sender, e);
        ImageAddingCommand?.Execute(null);
        ClearRedoHistory();
    }

    private void FormulaButton_Clicked(object sender, EventArgs e)
    {
        FormulaAddingEvent?.Invoke(sender, e);
        FormulaAddingCommand?.Execute(null);
        ClearRedoHistory();
    }

    private void HorizontalRuleButton_Clicked(object sender, EventArgs e)
    {
        if (SelectionLength > 0)
            return;

        string tag = TextTags.HorizontalRuleTag;

        if (!IsNewLineOnPosition(ObservableText, CursorPosition - 1))
            tag = "\n" + tag;

        ChangedByControls = true;
        textCommandManager.ClearRedoHistory();

        ObservableText = textCommandManager.ChangeText(TextAction.Insert, tag, CursorPosition);        
    }    

    private Match GetListElementByPosition(string text, string tagPattern, int position, int selectionLength)
    {
        var matches = Regex.Matches(text, tagPattern);
        int endPosition = position + selectionLength;

        var elementOnPosition = matches.FirstOrDefault(match => match.Groups["value"].Index <= position &
                                                       match.Groups["value"].Index + match.Groups["value"].Length >= endPosition);        

        return elementOnPosition;
    }

    private string AddBulletedListTagToText(string text, string tag, string tagPattern, int positionForTag, int selectionLength)
    {
        text ??= string.Empty;
        string newText = text;
        
        var elementOnPosition = GetListElementByPosition(newText, tagPattern, positionForTag, selectionLength);    

        if (selectionLength > 0 & elementOnPosition != null)
            newText = RemoveListTag(newText, elementOnPosition);
        else
            newText = InsertListTag(newText, tag, positionForTag, selectionLength, elementOnPosition);
        
        string result = RemoveExtraNewLines(newText);

        ChangedByControls = true;
        textCommandManager.ClearRedoHistory();

        return textCommandManager.ChangeWithListTag(result, text);
    }   

    private bool IsFullListValueSelected(Match elementOnPosition, int selectionLength)
    {
        if(elementOnPosition == null)
            return false;

        int lengthWithSpace = elementOnPosition.Groups["value"].Length;
        int lengthWithoutSpace = elementOnPosition.Groups["value"].Length - 1;

        return lengthWithSpace == selectionLength || lengthWithoutSpace == selectionLength;
    }

    private string AddNumericListTagToText(string text, string tag, string tagPattern, int positionForTag, int selectionLength)
    {
        text ??= string.Empty;
        string newText = text;       
        
        var elementOnPosition = GetListElementByPosition(newText, tagPattern, positionForTag, selectionLength);        

        if (IsFullListValueSelected(elementOnPosition, selectionLength))        
            newText = RemoveListTag(newText, elementOnPosition);     
        else        
            newText = InsertListTag(newText, tag, positionForTag, selectionLength, elementOnPosition); 
        
        string validNewText = RevalidateAllListsIndexes(newText, tagPattern);
        string result = RemoveExtraNewLines(validNewText);

        ChangedByControls = true;
        textCommandManager.ClearRedoHistory();

        return textCommandManager.ChangeWithListTag(result, text);       
    }

    private string RemoveListTag(string text, Match listElementMatch)
    {        
        string elementLvl = listElementMatch.Groups["lvl"].Value;
        string elementSign = listElementMatch.Groups["sign"].Value;

        int elementLvlIndex = listElementMatch.Groups["lvl"].Index;
        string textToRemove = elementLvl + elementSign;

        return text.Remove(elementLvlIndex, textToRemove.Length);             
    }    

    private int GetNewLinesCountBetweenElements(string text, int position)
    {
        int count = 0;

        while (position > 0 && text[position - 1] == '\n')
        {
            count++;
            position--;
        }

        return count;
    }    

    private bool IsNewLineOnPosition(string text, int position)
    {
        if(position >= 0)
            return text[position] == '\n';

        return false;
    }

    private int GetLvlCharsCount(string text, int cursorPosition)
    {
        int lvlCharsCount = 0;

        while (cursorPosition >= 0 && text[cursorPosition] == PairsTextTags.LvlSymbol)
        {
            cursorPosition--;
            lvlCharsCount++;
        }

        return lvlCharsCount; 
    }

    private string InsertListTag(string text, string tag, int positionForTag, int selectionLength, Match elementOnPosition)
    {
        int positionBeforeTag = positionForTag - 1;
        int positionWithoutLvl = positionBeforeTag - GetLvlCharsCount(text, positionBeforeTag);         

        string tagToInsert = selectionLength > 0 ? GetOpeningTag(tag) : tag.Replace(";", "");
        bool shouldInsertNewLine = text.Length > 0 && positionWithoutLvl >= 0 && !IsNewLineOnPosition(text, positionWithoutLvl);

        tagToInsert = shouldInsertNewLine ? "\n" + tagToInsert : tagToInsert + "\n";        

        if (selectionLength > 0)
        {
            if(elementOnPosition != null)
            {
                int elementValueStart = elementOnPosition.Groups["value"].Index;
                int elementValueEnd = elementOnPosition.Groups["value"].Length + elementValueStart;

                string selection = text.Substring(positionForTag, selectionLength);
                text = text.Remove(positionForTag, selectionLength);

                text = text.Insert(elementValueEnd - selectionLength, tagToInsert);
                return text.Insert(elementValueEnd - selectionLength + tagToInsert.Length, selection);
            }
            else
            {
                if(shouldInsertNewLine)
                {
                    text = text.Insert(positionForTag, tagToInsert);
                    return text.Insert(positionForTag + selectionLength + tagToInsert.Length, "\n");
                }
                else
                {
                    var insertedParts = tagToInsert.Split(' ');
                    string insertedAtStart = "\n" + insertedParts[0] + " ";
                    string insertedAtEnd = insertedParts[1];

                    text = text.Insert(positionForTag, insertedAtStart);
                    return text.Insert(positionForTag + selectionLength + insertedAtStart.Length + 1, insertedAtEnd);
                }
            }
        }
        
        return text.Insert(positionForTag, tagToInsert); 
    }

    private bool IsValidLvl(int currentLvlChars, int previousLvlChars, int levelsCount)
    {
       int lvlCharsCount = PairsTextTags.LvlCharsCount;
       int currentLvl = currentLvlChars / lvlCharsCount;

       bool lvlIsValid = currentLvlChars % lvlCharsCount == 0;
       bool newElementValid = levelsCount >= currentLvl || currentLvlChars == previousLvlChars + 2;

       return lvlIsValid && newElementValid;
    }   

    private string RemoveExtraNewLines(string text)
    {
        return Regex.Replace(text, "\n+", "\n");
    }

    private string RevalidateAllListsIndexes(string text, string tagPattern)
    {
        int currentCursorPosition = -1;
        int previousLvlCharsCount = 0;
        List<int> levelsList = new List<int>();

        return Regex.Replace(text, tagPattern, m =>
        {
            string matchLvl = m.Groups["lvl"].Value;
            string matchValue = m.Groups["value"].Value;

            int cursor = m.Index - 1;
            cursor -= GetNewLinesCountBetweenElements(text, cursor);                  

            int elementLvlCharsCount = matchLvl.Length;
            int currentIndex = 1;
            int currentCount = elementLvlCharsCount / PairsTextTags.LvlCharsCount;           

            if (IsValidLvl(elementLvlCharsCount, previousLvlCharsCount, levelsList.Count))
            {                            
                currentIndex = GetIndexForCurrentElement(levelsList, cursor, currentCursorPosition, elementLvlCharsCount, currentCount, currentIndex);     
                previousLvlCharsCount = elementLvlCharsCount;
                currentCursorPosition = m.Index + m.Length;
            }
            else
            {
                levelsList.Clear();
                previousLvlCharsCount = 0;
            }      

            return $"{matchLvl}{currentIndex}.{matchValue}";
        });
    }    

    private int GetIndexForCurrentElement(List<int> levelsList, int elementCursor, int currentCursor, int elementChars, int currentCount, int currentIndex)
    {
        if (!levelsList.Any() || levelsList.Count == currentCount)
        {
            levelsList.Add(currentIndex);
        }
        else if (elementCursor == currentCursor)
        {            
            if (currentCount > 0)
            {
                for(int levelIndex = currentCount + 1; levelIndex < levelsList.Count; levelIndex++)
                {
                    levelsList[levelIndex] = 0;
                }
            }
            
            currentIndex = levelsList[currentCount] + 1;
            levelsList[currentCount] = currentIndex;

            if (elementChars == 0)
            {
                levelsList.Clear();
                levelsList.Add(currentIndex);
            }            
        }
        else
        {
            levelsList.Clear();
            levelsList.Add(currentIndex);
        }

        return currentIndex;
    }    

    private void NumericListButton_Clicked(object sender, EventArgs e)
    {
        ObservableText = AddNumericListTagToText(ObservableText, PairsTextTags.NumericListTag, PairsTextTags.NumericListRegexPattern, CursorPosition, SelectionLength);        
        CursorPosition = ObservableText?.Length ?? 0;
    }

    private void BulletedListButton_Clicked(object sender, EventArgs e)
    {
        ObservableText = AddBulletedListTagToText(ObservableText, PairsTextTags.BulletedListTag, PairsTextTags.BulletedListRegexPattern, CursorPosition, SelectionLength);
        CursorPosition = ObservableText?.Length ?? 0;
    }

    public void ClearRedoHistory() => textCommandManager.ClearRedoHistory();

    public void ClearTextHistory()
    {
        textCommandManager.ClearUndoHistory();
        textCommandManager.ClearRedoHistory();
        textCommandManager.ClearCurrentText();

        UndoButton.IsEnabled = textCommandManager.CanUndo;
        UndoButton.IsEnabled = textCommandManager.CanRedo;
    }
}