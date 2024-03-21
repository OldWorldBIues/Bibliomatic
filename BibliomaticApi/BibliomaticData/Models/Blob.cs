namespace BibliomaticData.Models
{
    public class Blob
    {
        public string? Uri { get; set; }
        public string? Name { get; set; }
        public Stream? Content { get; set; }
        public string? ContentType { get; set; }
    }
}
