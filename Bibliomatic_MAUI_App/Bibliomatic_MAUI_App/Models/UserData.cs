using System.Collections.ObjectModel;

namespace Bibliomatic_MAUI_App.Models
{
    public class UserData
    {
        public Guid Id { get; set; }
        public ObservableCollection<ArticleResponce>? Articles { get; set; }
        public ObservableCollection<TestResponse>? Tests { get; set; }
        public ObservableCollection<BaseQuestionResponce>? Questions { get; set; }     

        public void Clear()
        {
            Articles.Clear();
            Tests.Clear();
            Questions.Clear();            
        }
    }
}
