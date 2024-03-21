using Bibliomatic_MAUI_App.Models;

namespace Bibliomatic_MAUI_App.Selectors
{
    public class UserDataDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Articles { get; set; }
        public DataTemplate Tests { get; set; }
        public DataTemplate Questions { get; set; }        

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is ArticleResponce)
            {
                return Articles;
            }
            else if (item is TestResponse)
            {
                return Tests;
            }
            else
            {
                return Questions;
            }
        }
    }
}
