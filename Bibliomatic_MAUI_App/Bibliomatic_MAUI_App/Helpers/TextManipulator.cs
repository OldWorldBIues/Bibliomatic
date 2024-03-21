namespace Bibliomatic_MAUI_App.Helpers
{
    public class TextManipulator
    {
        string currentText = string.Empty;

        public string Operation(TextAction action, string text, int position)
        {
            if (action == TextAction.Insert)
                currentText = currentText.Insert(position, text);
            else
                currentText = currentText.Remove(position, text.Length);

            return currentText;
        }

        public string PairsTagOperation(PairsAction pairsAction, int openingStartPos, int closingStartPos, string openingTag, string closingTag)
        {
            bool isChanged = pairsAction == PairsAction.Changed;

            var textAction = isChanged ? TextAction.Insert : TextAction.Remove;
            int openingTagStartRemovingPosition = isChanged ? openingStartPos - openingTag.Length : openingStartPos;
            int closingTagStartRemovingPosition = isChanged ? closingStartPos + openingTag.Length : closingStartPos;
            
            Operation(textAction, openingTag, openingTagStartRemovingPosition);
            Operation(textAction, closingTag, closingTagStartRemovingPosition);

            return currentText;
        }

        public string ReplaceText(string newText) => currentText = newText;

        public void ClearCurrentText() => currentText = string.Empty;
    }
}
