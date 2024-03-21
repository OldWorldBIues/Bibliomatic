namespace Bibliomatic_MAUI_App.Helpers
{
    public enum TextAction
    {
        Insert,
        Remove
    }

    public class TextManipulationCommand : ICommand
    {
        TextAction textAction;
        string text;
        int position;
        TextManipulator textManipulator;       

        public TextManipulationCommand(TextManipulator textManipulator, TextAction textAction, string text, int position)
        {
            this.textManipulator = textManipulator;
            this.textAction = textAction;
            this.text = text;
            this.position = position;
        }        

        public string Execute()
        {
            return textManipulator.Operation(textAction, text, position);
        }

        public string UnExecute()
        {
            return textManipulator.Operation(Undo(textAction), text, position);
        }

        private TextAction Undo(TextAction textAction)
        {
            if(textAction == TextAction.Insert)
                return TextAction.Remove;

            return TextAction.Insert;
        }    
    }
}
