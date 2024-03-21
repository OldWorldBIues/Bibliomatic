namespace Bibliomatic_MAUI_App.Models
{
    public enum CreationElementType
    {
        Article,
        Test,
        Question
    }

    public class CreationElement
    {
        public string Image { get; set; }
        public string Text { get; set; }
        public CreationElementType ElementType { get; set; }
    }
}
