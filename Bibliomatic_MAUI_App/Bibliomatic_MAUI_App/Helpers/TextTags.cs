namespace Bibliomatic_MAUI_App.Helpers
{ 
    public static class TextTags
    {
        public static readonly string SpaceTag = @"$'space'/";
        public static readonly string SpaceRegexPattern = @"\$'space'\/";
        public static string GetHtmlSpaceTag(string value) => $"<button>{value}</button>";

        public static readonly string ImageTag = @"$'image;[];[];'/";     
        public static readonly string ImageRegexPattern = @"\$'image\[(?<filename>.+)\]\[(?<description>.+)\]'\/";

        public static readonly string FormulaTag = @"$'formula;[];[];'/";
        public static readonly string FormulaRegexPattern = @"\$'formula\[(?<latex>.+)\]\[(?<description>.+)\]'\/";
        public static string GetHtmlImageFileTag(string imageExtension, string imageBase64, string description) => $@"<img style="" height:300px; display:block; margin:auto;"" src=""data:image/{imageExtension};base64,{imageBase64}"" alt=""{description}""/>";
        public static string GetHtmlImageLinkTag(string imageLink, string description) => $@"<img style="" height:300px; display:block; margin:auto;"" src=""{imageLink}"" alt=""{description}""/>";

        public static readonly string HyperlinkTag = @"$'hyperlink;[];[];'/";
        public static readonly string HyperlinkRegexPattern = @"\$'hyperlink\[(?<link>.+)\]\[(?<description>.+)\]'\/";
        public static string GetHtmlHyperlinkTag(string hyperlink, string hyperlinkDescription) => $"<a href=\"{hyperlink}\">{hyperlinkDescription}</a>";

        public static readonly string HorizontalRuleTag = @"==========";
        public static readonly string HorizontalRuleRegexPattern = @"={10}";
        public static string GetHtmlHorizontalRuleTag() => $"<hr>";

        public static readonly string NewLineTag = "\n";
        public static readonly string NewLineRegexPattern = "\n+";
        public static string GetHtmlNewLineTag() => "<br>";
    }
    
    public static class PairsTextTags
    {
        public static readonly int LvlCharsCount = 2;
        public static readonly char LvlSymbol = '-';
        public static readonly string LvlRegexPattern = @"-+";

        public static readonly string BoldTag = @"!;Text;!";
        public static readonly string BoldRegexPattern = @"(?<!\[.*)(!([^!]+)!)(?![^\[]*\])";
        public static string GetHtmlBoldTag(string value) => $"<b>{value}</b>";

        public static readonly string ItalicTag = @"~;Text;~";
        public static readonly string ItalicRegexPattern = @"(?<!\[.*)(~([^~]+)~)(?![^\[]*\])";
        public static string GetHtmlItalicTag(string value) => $"<i>{value}</i>";

        public static readonly string UnderlineTag = @"_;Text;_";
        public static readonly string UnderlineRegexPattern = @"(?<!\[.*)(_([^_]+)_)(?![^\[]*\])";
        public static string GetHtmlUnderlineTag(string value) => $"<u>{value}</u>";

        public static readonly string BlockquoteTag = @"@;Text;@";
        public static readonly string BlockquoteRegexPattern = @"(?<!\[.*)(@([^@]+)@)(?![^\[]*\])";
        public static string GetHtmlBlockquoteTag(string value) => $"<blockquote>{value}</blockquote>";

        public static readonly string HeadingTag = @"1#;Text;#";
        public static readonly string HeadingRegexPattern = @"(?<!\[.*)((?<heading_index>[1-6])#([^#]+)#)(?![^\[]*\])";
        public static string GetHtmlHeadingTag(string value, string headingIndex) => $"<h{headingIndex}>{value}</h{headingIndex}>";

        public static readonly string OpeningHtmlListTag = "<ol style=\"list-style-type: none;\">";
        public static readonly string ClosingHtmlListTag = "</ol>";
        public static readonly string OpeningHtmlListElementTag = "<li>";
        public static readonly string ClosingHtmlListElementTag = "</li>";

        public static readonly string NumericListTag = @"1. ;List element";
        public static readonly string NumericListRegexPattern = @"(?<lvl>-*)(?<sign>\d+\.{1})(?<value>\s.*)";

        public static readonly string BulletedListTag = @"* ;List element";
        public static readonly string BulletedListRegexPattern = @"(?<lvl>-*)(?<sign>\*{1})(?<value>\s.*)";        
    }
}
