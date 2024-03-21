using Bibliomatic_MAUI_App.Models;

namespace Bibliomatic_MAUI_App.Selectors
{
    public class TestQuestionDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultQuestion { get; set; }
        public DataTemplate SpacesQuestion { get; set; }       
        public DataTemplate OpenEndedQuestion { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var testQuestion = (TestQuestionResponse)item;

            if (testQuestion.TestQuestionType == TestQuestionType.SpacesQuestion)
                return SpacesQuestion;
            else if(testQuestion.TestQuestionType == TestQuestionType.OpenEndedQuestion & OpenEndedQuestion != null)
                return OpenEndedQuestion;

            return DefaultQuestion;            
        }
    }
}
