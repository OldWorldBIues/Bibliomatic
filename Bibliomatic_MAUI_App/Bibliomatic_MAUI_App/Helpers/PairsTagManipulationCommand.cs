namespace Bibliomatic_MAUI_App.Helpers
{
    public enum PairsAction
    {
        Changed,
        Regular
    }

    public class PairsTagManipulationCommand : ICommand
    {
        string openingTag, closingTag;        
        PairsAction pairsAction;
        int openingStartPos, closingStartPos;        
        TextManipulator textManipulator;

        public PairsTagManipulationCommand(TextManipulator textManipulator, PairsAction pairsAction, 
            int openingStartPos, int closingStartPos, string openingTag, string closingTag)
        {
            this.textManipulator = textManipulator;
            this.pairsAction = pairsAction;
            this.openingTag = openingTag;
            this.closingTag = closingTag;
            this.openingStartPos = openingStartPos;
            this.closingStartPos = closingStartPos;
        }

        public string Execute()
        {            
            return textManipulator.PairsTagOperation(pairsAction, UpdateCursor(pairsAction), closingStartPos, openingTag, closingTag);
        }

        public string UnExecute()
        {
            var action = Undo(pairsAction);
            return textManipulator.PairsTagOperation(action, UpdateCursor(action), closingStartPos, openingTag, closingTag);
        }

        private PairsAction Undo(PairsAction pairsAction)
        {
            if (pairsAction == PairsAction.Changed)            
                return PairsAction.Regular;
            
            return PairsAction.Changed;
        }

        private int UpdateCursor(PairsAction action)
        {
            int offset = openingTag.Length;
            openingStartPos += action == PairsAction.Changed ? offset : -offset;            

            return openingStartPos;
        }
    }
}
