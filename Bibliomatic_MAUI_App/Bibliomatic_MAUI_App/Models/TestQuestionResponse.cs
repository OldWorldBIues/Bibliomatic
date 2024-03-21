using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;
using Bibliomatic_MAUI_App.Models.AttachmentInfo;

namespace Bibliomatic_MAUI_App.Models
{    
    public partial class TestQuestionResponse : ObservableObject
    { 
        public Guid Id { get; set; }
        public Guid TestId { get; set; }
        public TestResponse Test { get; set; }
        public ObservableCollection<TestAnswerResponse>? TestAnswers { get; set; }
        public string Question { get; set; }       
        public double PointsPerAnswer { get; set; }        
        public TestQuestionType TestQuestionType { get; set; }       

        [JsonPropertyName("testQuestionFilename")]
        public string TestQuestionImageFilename { get; set; }

        [JsonPropertyName("testQuestionFormulaFilename")]
        public string TestQuestionFormulaImageFilename { get; set; }

        #region Additional properties
        [JsonIgnore]
        public string QuestionImageFilenameBeforeEdit { get; set; }

        [JsonIgnore]
        public FileType QuestionImageFileTypeBeforeEdit { get; set; }

        [JsonIgnore]
        public string QuestionFormulaImageFilenameBeforeEdit { get; set; }

        [JsonIgnore]
        public FileType QuestionFormulaImageFileTypeBeforeEdit { get; set; }

        [JsonIgnore]
        public FileType TestQuestionImageFileType { get; set; }

        [JsonIgnore]
        public FileType TestQuestionFormulaImageFileType { get; set; }

        [JsonIgnore]
        public bool ChangedByControls { get; set; }

        [JsonIgnore]
        public ObservableCollection<KeyValuePair<int, TestAnswerResponse>> DeletedTestAnswers { get; set; }

        [JsonIgnore]
        public List<SpaceTestVariantResponse> AllSpacesVariants { get; set; }

        [JsonIgnore]
        public List<string> CorrectSpacesValues { get; set; }

        [ObservableProperty]
        [JsonIgnore]
        public bool answerUnfilled;

        [ObservableProperty]
        [JsonIgnore]
        public CorrectType questionCorrectType; 
        

        [ObservableProperty]
        [JsonIgnore]
        public TestAnswerResponse selectedPickerTestAnswer;

        [ObservableProperty]
        [JsonIgnore]
        public bool isDisplaySpacePicker;

        [ObservableProperty]
        [JsonIgnore]
        public string userAnswer;

        [ObservableProperty]
        [JsonIgnore]
        public int questionNumber;

        [ObservableProperty]
        [JsonIgnore]
        public string testQuestionLatexFormula;

        [ObservableProperty]
        [JsonIgnore]
        public string testQuestionImageFilepath;

        [ObservableProperty]
        [JsonIgnore]
        public string testQuestionFormulaImageFilepath;

        [ObservableProperty]
        [JsonIgnore]
        public string spacesEditorText;

        [ObservableProperty]
        [JsonIgnore]
        public HtmlWebViewSource spacesHtmlText;
        #endregion
    }
    
    public enum TestQuestionType
    {
        OneAnswerQuestion,
        MultipleAnswerQuestion,
        PairsQuestion,
        OpenEndedQuestion,
        SpacesQuestion
    }

    public class TestQuestionTypeValues
    {
        public TestQuestionType Type { get; private set; }

        public string TypeValue
        {
            get => typesDictionary[Type];
        }

        private static readonly Dictionary<TestQuestionType, string> typesDictionary = new Dictionary<TestQuestionType, string>
        {
            {TestQuestionType.OneAnswerQuestion, "One answer question" },
            {TestQuestionType.MultipleAnswerQuestion, "Multiple answer question" },
            {TestQuestionType.PairsQuestion, "Pairs question" },
            {TestQuestionType.OpenEndedQuestion, "Open ended question" },
            {TestQuestionType.SpacesQuestion, "Spaces question" }
        };

        public TestQuestionTypeValues(TestQuestionType questionType)
        {
            Type = questionType;
        }
    }
}
