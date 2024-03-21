using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Bibliomatic_MAUI_App.Models
{
    [ObservableObject]
    public partial class BaseQuestionResponce : TimeTrackedModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Author { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public bool IsSolved { get; set; }
        public int AnswersCount { get; set; }
        public QuestionResponce? Question { get; set; }
        public ObservableCollection<AnswerResponce>? Answers { get; set; }

        #region Additional properties      
        [JsonIgnore]
        public ObservableCollection<AnswerResponce> AllAnswers { get; set; }

        [ObservableProperty]
        [JsonIgnore]
        public int allAnswersCount;

        [JsonIgnore]
        public string ShortAuthorName { get => Author[..Author.LastIndexOf(' ')]; }
        #endregion
    }
}
