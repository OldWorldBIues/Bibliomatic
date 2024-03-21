namespace BibliomaticData.Models.DTOs
{
    public class BlobDTO
    {
        public string? Status { get; set; }
        public bool Error { get; set; }
        public Blob? Blob { get; set; }

        public BlobDTO()
        {
            Blob = new Blob();
        }
    }
}
