namespace Bibliomatic_MAUI_App.Helpers
{

    public class ListTagManipulationCommand : ICommand
    {
        string oldText, newText;        
        TextManipulator textManipulator;

        public ListTagManipulationCommand(TextManipulator textManipulator, string oldText, string newText)
        {
            this.textManipulator = textManipulator;            
            this.oldText = oldText;
            this.newText = newText;
        }

        public string Execute()
        {
            return textManipulator.ReplaceText(newText);
        }

        public string UnExecute()
        {
            return textManipulator.ReplaceText(oldText);
        }
    }
}
