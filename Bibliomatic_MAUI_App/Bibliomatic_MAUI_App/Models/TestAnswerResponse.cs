using Bibliomatic_MAUI_App.Models.AttachmentInfo;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Bibliomatic_MAUI_App.Models
{
    public partial class TestAnswerResponse : ObservableObject
    {
        public Guid Id { get; set; }
        public Guid TestQuestionId { get; set; }
        public TestQuestionResponse TestQuestion { get; set; }
        public string Answer { get; set; }
        public string Variant { get; set; }
        public string Message { get; set; }        
        public bool IsCorrectAnswer { get; set; }        

        [JsonPropertyName("testAnswerFilename")]
        public string TestAnswerImageFilename { get; set; }

        [JsonPropertyName("testAnswerFormulaFilename")]
        public string TestAnswerFormulaImageFilename { get; set; }

        [JsonPropertyName("testVariantFilename")]
        public string TestVariantImageFilename { get; set; }

        [JsonPropertyName("testVariantFormulaFilename")]
        public string TestVariantFormulaImageFilename { get; set; }

        #region Additional properties
        [JsonIgnore]
        public string AnswerImageFilenameBeforeEdit { get; set; }

        [JsonIgnore]
        public FileType AnswerImageFileTypeBeforeEdit { get; set; }

        [JsonIgnore]
        public string AnswerFormulaImageFilenameBeforeEdit { get; set; }

        [JsonIgnore]
        public FileType AnswerFormulaImageFileTypeBeforeEdit { get; set; }

        [JsonIgnore]
        public string VariantImageFilenameBeforeEdit { get; set; }

        [JsonIgnore]
        public FileType VariantImageFileTypeBeforeEdit { get; set; }

        [JsonIgnore]
        public string VariantFormulaImageFilenameBeforeEdit { get; set; }

        [JsonIgnore]
        public FileType VariantFormulaImageFileTypeBeforeEdit { get; set; }

        [JsonIgnore]
        public FileType TestAnswerImageFileType { get; set; }

        [JsonIgnore]
        public FileType TestAnswerFormulaImageFileType { get; set; }

        [JsonIgnore]
        public FileType TestVariantImageFileType { get; set; }

        [JsonIgnore]
        public FileType TestVariantFormulaImageFileType { get; set; }

        [JsonIgnore]
        public ObservableCollection<SpaceTestVariantResponse> SpacesTestVariants { get; set; }

        [JsonIgnore]
        public int CursorPosition { get; set; }
       
        [ObservableProperty]
        [JsonIgnore]
        public CorrectType answerCorrectType;

        [ObservableProperty]
        [JsonIgnore]
        public string testAnswerLatexFormula;

        [ObservableProperty]
        [JsonIgnore]
        public string testAnswerImageFilepath;

        [ObservableProperty]
        [JsonIgnore]
        public string testAnswerFormulaImageFilepath;

        [ObservableProperty]
        [JsonIgnore]
        public string testVariantLatexFormula;

        [ObservableProperty]
        [JsonIgnore]
        public string testVariantImageFilepath;

        [ObservableProperty]
        [JsonIgnore]
        public string testVariantFormulaImageFilepath;

        [ObservableProperty]
        [JsonIgnore]
        public int answerNumber;

        [ObservableProperty]
        [JsonIgnore]
        public bool isSelected;

        [ObservableProperty]
        [JsonIgnore]
        public TestAnswerResponse selectedTestAnswer;

        [ObservableProperty]
        [JsonIgnore]
        public SpaceTestVariantResponse selectedPickerSpaceTestVariant;
        #endregion
    }   
}
