using System.Text.Json.Serialization;

namespace Bibliomatic_MAUI_App.Models.AttachmentInfo
{
    public class QuestionFormulaInfo
    {
        public Guid Id { get; set; }
        public Guid? QuestionId { get; set; }
        public string FormulaLatex { get; set; }
        public string FormulaFilename { get; set; }

        [JsonIgnore]
        public string FormulaFilepath { get; set; }

        [JsonIgnore]
        public int IndexOfFormula { get; set; }

        [JsonIgnore]
        public FileType FormulaType { get; set; }
    }

    public class AnswerFormulaInfo
    {        
        public Guid? AnswerId { get; set; }
        public string FormulaLatex { get; set; }
        public string FormulaFilename { get; set; }

        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public string FormulaFilepath { get; set; }

        [JsonIgnore]
        public int IndexOfFormula { get; set; }

        [JsonIgnore]
        public FileType FormulaType { get; set; }
    }    
}
