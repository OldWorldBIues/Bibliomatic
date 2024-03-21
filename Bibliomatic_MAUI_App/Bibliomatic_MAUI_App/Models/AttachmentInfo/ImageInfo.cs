using System.Text.Json.Serialization;

namespace Bibliomatic_MAUI_App.Models.AttachmentInfo
{
    public class QuestionImageInfo
    {
        public Guid Id { get; set; }
        public Guid? QuestionId { get; set; }
        public string ImageFilename { get; set; }

        [JsonIgnore]
        public string ShortImageFilename { get => Path.GetFileName(ImageFilename); }

        [JsonIgnore]
        public string ImageFilepath { get; set; }

        [JsonIgnore]
        public int IndexOfImage { get; set; }

        [JsonIgnore]
        public FileType ImageType { get; set; }
    }

    public class AnswerImageInfo
    {       
        public Guid? AnswerId { get; set; }
        public string ImageFilename { get; set; }

        [JsonIgnore]
        public string ShortImageFilename { get => Path.GetFileName(ImageFilename); }

        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public string ImageFilepath { get; set; }

        [JsonIgnore]
        public int IndexOfImage { get; set; }

        [JsonIgnore]
        public FileType ImageType { get; set; }
    }    
}
