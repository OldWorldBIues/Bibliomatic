namespace Bibliomatic_MAUI_App.Helpers
{
    public class TextCommandManager
    {
        readonly TextManipulator textManipulator = new TextManipulator();

        readonly Stack<ICommand> undoHistoryStack = new Stack<ICommand>();
        readonly Stack<ICommand> redoHistoryStack = new Stack<ICommand>();

        public bool CanUndo { get; private set; } = false;
        public bool CanRedo { get; private set; } = false;

        public string Redo()
        {
            if (redoHistoryStack.Count <= 1)
                CanRedo = false;
           
            var command = redoHistoryStack.Pop();         
            undoHistoryStack.Push(command);

            CanUndo = true;
            return command.Execute();            
        }

        public string Undo()
        {
            if (undoHistoryStack.Count <= 1)
                CanUndo = false;

            var command = undoHistoryStack.Pop();
            redoHistoryStack.Push(command);

            CanRedo = true;
            return command.UnExecute();            
        }

        public void ClearRedoHistory()
        {
            redoHistoryStack.Clear();            
            CanRedo = false;
        }

        public void ClearUndoHistory()
        {
            undoHistoryStack.Clear();
            CanUndo = false;
        }

        public void ClearCurrentText()
        {
            textManipulator.ClearCurrentText();
        }

        public string ChangeText(TextAction action, string text, int position)
        {
            var command = new TextManipulationCommand(textManipulator, action, text, position);

            undoHistoryStack.Push(command);
            CanUndo = true;

            return command.Execute();             
        }

        public string ChangeWithTag(PairsAction action, int openingStartPos, int closingStartPos, string openingTag, string closingTag)
        {            
            var command = new PairsTagManipulationCommand(textManipulator, action, openingStartPos, closingStartPos, openingTag, closingTag);

            undoHistoryStack.Push(command);
            CanUndo = true;       

            return command.Execute();            
        }     

        public string ChangeWithListTag(string newText, string oldText)
        {
            var command = new ListTagManipulationCommand(textManipulator, oldText, newText);

            undoHistoryStack.Push(command);
            CanUndo = true;

            return command.Execute();
        }   
    }
}
