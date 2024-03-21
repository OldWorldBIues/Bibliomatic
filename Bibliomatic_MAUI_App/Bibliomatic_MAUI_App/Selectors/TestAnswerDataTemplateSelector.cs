using Bibliomatic_MAUI_App.Models;

namespace Bibliomatic_MAUI_App.Selectors
{
    public class TestAnswerDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate OneAnswerQuestion { get; set; }
        public DataTemplate MultipleAnswerQuestion { get; set; }
        public DataTemplate PairsQuestion { get; set; }
        public DataTemplate OpenEndedQuestion { get; set; }
        public DataTemplate SpacesQuestion { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var testAnswer = (TestAnswerResponse)item;
            var questionType = testAnswer.TestQuestion.TestQuestionType;

            switch (questionType)
            {
                case TestQuestionType.OneAnswerQuestion:
                    return OneAnswerQuestion;

                case TestQuestionType.MultipleAnswerQuestion:
                    return MultipleAnswerQuestion;

                case TestQuestionType.PairsQuestion:
                    return PairsQuestion;

                case TestQuestionType.OpenEndedQuestion:
                    return OpenEndedQuestion;

                case TestQuestionType.SpacesQuestion:
                    return SpacesQuestion;

                default:
                    return OneAnswerQuestion;
            }         
        }
    }
}
