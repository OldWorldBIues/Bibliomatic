namespace Bibliomatic_MAUI_App.Models.AttachmentInfo
{
    public class AttachmentResponse
    {
        public string? Uri { get; set; }
        public string? Name { get; set; }
        public Stream? Content { get; set; }
        public string? ContentType { get; set; }
    }
}
