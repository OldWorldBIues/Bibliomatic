using System.Text.Json.Serialization;

namespace Bibliomatic_MAUI_App.Models.AttachmentInfo
{
    public class AttachmentDTOResponse
    {
        public string? Status { get; set; }
        public bool Error { get; set; }

        [JsonPropertyName("blob")]
        public AttachmentResponse? Attachment { get; set; }
    }
}
