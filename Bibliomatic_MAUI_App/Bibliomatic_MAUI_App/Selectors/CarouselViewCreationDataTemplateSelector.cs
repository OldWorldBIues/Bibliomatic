using Bibliomatic_MAUI_App.Models;

namespace Bibliomatic_MAUI_App.Selectors
{
    public class CarouselViewCreationDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ArticleTemplate { get; set; }
        public DataTemplate TestTemplate { get; set; }
        public DataTemplate QuestionTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var creationElement = (CreationElement)item;
            var creationElementType = creationElement.ElementType;

            if (creationElementType == CreationElementType.Article)
                return ArticleTemplate;
            else if(creationElementType == CreationElementType.Test)
                return TestTemplate;

            return QuestionTemplate;
        }
    }
}
