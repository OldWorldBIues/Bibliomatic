using Bibliomatic_MAUI_App.Models.AttachmentInfo;
using System.Text;
using System.Text.RegularExpressions;

namespace Bibliomatic_MAUI_App.Helpers
{
    public class TagsToHtmlConverter
    {
        public Regex GetTagsRegex(Dictionary<string, string> patterns)
        {            
            var patternsList = new List<string>();

            foreach (var pattern in patterns)
            {
                patternsList.Add($"(?<{pattern.Key}>{pattern.Value})");                
            }

            var compositePattern = string.Join('|', patternsList);
            return new Regex(compositePattern, RegexOptions.Compiled);
        }

        private string ReplaceNonAttachmentTags(string text, string key, Match match, Group tagGroup)
        {           
            return key switch
            {
                "Bold" => GetBoldReplacement(text, tagGroup),
                "Italic" => GetItalicReplacement(text, tagGroup),
                "Underline" => GetUnderlineReplacement(text, tagGroup),
                "Blockquote" => GetBlockquoteReplacement(text, tagGroup),
                "Heading" => GetHeadingReplacement(text, tagGroup, match),
                "NewLine" => GetNewLineReplacement(),
                "HorizontalRule" => GetHorizontalRuleReplacement(),
                _ => match.Value,
            };
        }

        private string ReplaceQuestionTags(string text, Dictionary<string, string> patterns, IEnumerable<QuestionImageInfo> images, IEnumerable<QuestionFormulaInfo> formulas)
        {    
            var htmlBuilder = new StringBuilder(text);
            var patternsRegex = GetTagsRegex(patterns);
            int offset = 0;

            foreach(Match currentMatch in patternsRegex.Matches(text).Cast<Match>())
            {
                foreach (var pattern in patterns)
                {
                    var tagGroup = currentMatch.Groups[pattern.Key];

                    if (!tagGroup.Success)
                        continue;

                    string replacement = pattern.Key switch
                    {
                        "Hyperlink" => GetHyperlinkReplacement(currentMatch),
                        "Image" => GetQuestionImageReplacement(currentMatch, images),
                        "Formula" => GetQuestionFormulaReplacement(currentMatch, formulas),
                        _ => ReplaceNonAttachmentTags(text, pattern.Key, currentMatch, tagGroup),
                    };

                    htmlBuilder.Remove(tagGroup.Index + offset, tagGroup.Length).Insert(tagGroup.Index + offset, replacement);
                    offset += replacement.Length - tagGroup.Length;
                    break;
                }
            }

            return htmlBuilder.ToString();            
        }

        private string ReplaceAnswerTags(string text, Dictionary<string, string> patterns, IEnumerable<AnswerImageInfo> images, IEnumerable<AnswerFormulaInfo> formulas)
        {
            var htmlBuilder = new StringBuilder(text);
            var patternsRegex = GetTagsRegex(patterns);
            int offset = 0;

            foreach (Match currentMatch in patternsRegex.Matches(text).Cast<Match>())
            {
                foreach (var pattern in patterns)
                {
                    var tagGroup = currentMatch.Groups[pattern.Key];

                    if (!tagGroup.Success)
                        continue;

                    string replacement = pattern.Key switch
                    {
                        "Hyperlink" => GetHyperlinkReplacement(currentMatch),
                        "Image" => GetAnswerImageReplacement(currentMatch, images),
                        "Formula" => GetAnswerFormulaReplacement(currentMatch, formulas),
                        _ => ReplaceNonAttachmentTags(text, pattern.Key, currentMatch, tagGroup),
                    };

                    htmlBuilder.Remove(tagGroup.Index + offset, tagGroup.Length).Insert(tagGroup.Index + offset, replacement);
                    offset += replacement.Length - tagGroup.Length;
                    break;
                }
            }

            return htmlBuilder.ToString();
        }

        private string GetQuestionImageReplacement(Match match, IEnumerable<QuestionImageInfo> images)
        {
            string filename = match.Groups["filename"].Value;
            string description = match.Groups["description"].Value;

            var imageInfo = images.FirstOrDefault(img => Path.GetFileName(img.ImageFilename).Equals(filename));

            if (imageInfo == null) return string.Empty;

            string imagePath = imageInfo.ImageFilepath;

            if (imageInfo.ImageType == FileType.LocalFile)
            {
                string imageExtension = Path.GetExtension(imagePath).Replace(".", "");
                string imageBase64 = ConvertToBase64(imagePath);
                return TextTags.GetHtmlImageFileTag(imageExtension, imageBase64, description);
            }

            return TextTags.GetHtmlImageLinkTag(imagePath, description);
        }

        private string GetAnswerImageReplacement(Match match, IEnumerable<AnswerImageInfo> images)
        {
            string filename = match.Groups["filename"].Value;
            string description = match.Groups["description"].Value;

            var imageInfo = images.FirstOrDefault(img => Path.GetFileName(img.ImageFilename).Equals(filename));

            if (imageInfo == null) return string.Empty;

            string imagePath = imageInfo.ImageFilepath;

            if (imageInfo.ImageType == FileType.LocalFile)
            {
                string imageExtension = Path.GetExtension(imagePath).Replace(".", "");
                string imageBase64 = ConvertToBase64(imagePath);
                return TextTags.GetHtmlImageFileTag(imageExtension, imageBase64, description);
            }

            return TextTags.GetHtmlImageLinkTag(imagePath, description);
        }

        private string GetQuestionFormulaReplacement(Match match, IEnumerable<QuestionFormulaInfo> formulas)
        {
            string latexFormula = match.Groups["latex"].Value;
            string description = match.Groups["description"].Value;

            var formulaInfo = formulas.FirstOrDefault(f => f.FormulaLatex.Equals(latexFormula));

            if (formulaInfo == null) return string.Empty;

            string formulaPath = formulaInfo.FormulaFilepath;

            if (formulaInfo.FormulaType == FileType.LocalFile)
            {
                string imageExtension = Path.GetExtension(formulaPath).Replace(".", "");
                string imageBase64 = ConvertToBase64(formulaPath);
                return TextTags.GetHtmlImageFileTag(imageExtension, imageBase64, description);
            }

            return TextTags.GetHtmlImageLinkTag(formulaPath, description);
        }

        private string GetAnswerFormulaReplacement(Match match, IEnumerable<AnswerFormulaInfo> formulas)
        {
            string latexFormula = match.Groups["latex"].Value;
            string description = match.Groups["description"].Value;

            var formulaInfo = formulas.FirstOrDefault(f => f.FormulaLatex.Equals(latexFormula));

            if (formulaInfo == null) return string.Empty;

            string formulaPath = formulaInfo.FormulaFilepath;

            if (formulaInfo.FormulaType == FileType.LocalFile)
            {
                string imageExtension = Path.GetExtension(formulaPath).Replace(".", "");
                string imageBase64 = ConvertToBase64(formulaPath);
                return TextTags.GetHtmlImageFileTag(imageExtension, imageBase64, description);
            }

            return TextTags.GetHtmlImageLinkTag(formulaPath, description);
        }


        private string GetHyperlinkReplacement(Match match)
        {
            string hyperlink = match.Groups["link"].Value;
            string hyperlinkDescription = match.Groups["description"].Value;
            string replacement = TextTags.GetHtmlHyperlinkTag(hyperlink, hyperlinkDescription);

            return replacement;
        }

        private string ConvertAllNumericListTagsToHtml(string text)
        {
            string pattern = PairsTextTags.NumericListRegexPattern;
            return ConvertAllListTagsToHtml(text, pattern);
        }

        private string ConvertAllBulletedListTagsToHtml(string text)
        {
            string pattern = PairsTextTags.BulletedListRegexPattern;
            return ConvertAllListTagsToHtml(text, pattern);
        }

        private bool IsValidListLvl(int currentLvlChars, int previousLvlChars, int levelsCount)
        {
            int lvlCharsCount = PairsTextTags.LvlCharsCount;
            int currentLvl = currentLvlChars / lvlCharsCount;

            bool lvlIsValid = currentLvlChars % lvlCharsCount == 0;
            bool newElementValid = levelsCount >= currentLvl || currentLvlChars == previousLvlChars + 2;

            return lvlIsValid && newElementValid;
        }

        private string CloseListTag(string replacement, int currentListLevelsCount, ref int closedLevels)
        {
            for (int i = closedLevels; i < currentListLevelsCount; i += PairsTextTags.LvlCharsCount)
            {
                replacement += PairsTextTags.ClosingHtmlListTag;
                closedLevels = i;
            }

            closedLevels += PairsTextTags.LvlCharsCount;
            return replacement;
        }

        private string CloseListTag(string replacement) => replacement += PairsTextTags.ClosingHtmlListTag;

        private string ConvertAllListTagsToHtml(string text, string pattern)
        {
            int previousLvlCharsCount = 0;
            int currentListLevelsCount = 0;            
            int offset = 0;
            int closedLevels = 0;

            var htmlBuilder = new StringBuilder(text);
            var listMatchesRegex = new Regex(pattern, RegexOptions.Compiled);
            Match currentMatch = listMatchesRegex.Match(text);
            Match previousMatch = null;          

            while (currentMatch.Success)
            {
                string sign = currentMatch.Groups["sign"].Value;                
                string matchValue = currentMatch.Groups["value"].Value;
                int elementLvlCharsCount = currentMatch.Groups["lvl"].Value.Length;
                string replacement = string.Empty;
                bool notValidListClosed = false;

                if (previousMatch != null)
                {
                    if (previousMatch.Index + previousMatch.Length != currentMatch.Index - 1)
                    {  
                        replacement = CloseListTag(replacement, currentListLevelsCount, ref closedLevels);
                        replacement = CloseListTag(replacement);                        
                    }
                }               

                if (IsValidListLvl(elementLvlCharsCount, previousLvlCharsCount, currentListLevelsCount))
                {
                    if (elementLvlCharsCount > previousLvlCharsCount)
                    {
                        if (elementLvlCharsCount > currentListLevelsCount) currentListLevelsCount = elementLvlCharsCount;
                        replacement = PairsTextTags.OpeningHtmlListTag + PairsTextTags.OpeningHtmlListElementTag + sign + matchValue + PairsTextTags.ClosingHtmlListElementTag;
                        closedLevels -= closedLevels == 0 ? 0 : PairsTextTags.LvlCharsCount;
                    }
                    else if (elementLvlCharsCount < previousLvlCharsCount)
                    {
                        replacement = CloseListTag(replacement, currentListLevelsCount - elementLvlCharsCount, ref closedLevels);
                        replacement += PairsTextTags.OpeningHtmlListElementTag + sign + matchValue + PairsTextTags.ClosingHtmlListElementTag;
                        if(elementLvlCharsCount == 0) closedLevels = 0;
                    }
                    else
                    {
                        if (previousMatch == null) replacement = PairsTextTags.OpeningHtmlListTag;
                        replacement += PairsTextTags.OpeningHtmlListElementTag + sign + matchValue + PairsTextTags.ClosingHtmlListElementTag;                        
                    }

                    previousLvlCharsCount = elementLvlCharsCount;
                }
                else
                {
                    replacement = CloseListTag(replacement, currentListLevelsCount, ref closedLevels);
                    replacement = previousMatch == null ? replacement : CloseListTag(replacement);
                    replacement += PairsTextTags.OpeningHtmlListTag + PairsTextTags.OpeningHtmlListElementTag + sign;
                    replacement += matchValue + PairsTextTags.ClosingHtmlListElementTag + PairsTextTags.ClosingHtmlListTag;

                    closedLevels =  0;
                    previousLvlCharsCount = 0;
                    currentListLevelsCount = 0;
                    notValidListClosed = true;
                }

                if (!currentMatch.NextMatch().Success)
                {
                    replacement = CloseListTag(replacement);
                }

                htmlBuilder.Remove(currentMatch.Index + offset, currentMatch.Length).Insert(currentMatch.Index + offset, replacement);               
                offset += replacement.Length - currentMatch.Length;
                
                previousMatch = notValidListClosed ? null : currentMatch;
                currentMatch = currentMatch.NextMatch();                
            }

            return htmlBuilder.ToString();            
        }
        
        private string GetValueInsidePairTag(string tag, string text, int tagStart, int tagEnd)
        {
            var tagParts = tag.Split(';');

            int openingTagLength = tagParts[0].Length;
            int closingTagLength = tagParts[^1].Length;

            int valueStartIndex = tagStart + openingTagLength;
            int valueEndIndex = tagEnd - (openingTagLength + closingTagLength);

            return text.Substring(valueStartIndex, valueEndIndex);
        }

        private string GetNewLineReplacement() => TextTags.GetHtmlNewLineTag();

        private string GetHorizontalRuleReplacement() => TextTags.GetHtmlHorizontalRuleTag();

        private string GetBoldReplacement(string text, Group match)
        {
            string tag = PairsTextTags.BoldTag;
            string valueInside = GetValueInsidePairTag(tag, text, match.Index, match.Length);
            return PairsTextTags.GetHtmlBoldTag(valueInside);
        }

        private string GetItalicReplacement(string text, Group match)
        {
            string tag = PairsTextTags.ItalicTag;
            string valueInside = GetValueInsidePairTag(tag, text, match.Index, match.Length);
            return PairsTextTags.GetHtmlItalicTag(valueInside);
        }

        private string GetUnderlineReplacement(string text, Group match)
        {
            string tag = PairsTextTags.UnderlineTag;
            string valueInside = GetValueInsidePairTag(tag, text, match.Index, match.Length);
            return PairsTextTags.GetHtmlUnderlineTag(valueInside);
        }

        private string GetBlockquoteReplacement(string text, Group match)
        {
            string tag = PairsTextTags.BlockquoteTag;
            string valueInside = GetValueInsidePairTag(tag, text, match.Index, match.Length);
            return PairsTextTags.GetHtmlBlockquoteTag(valueInside);
        }

        private string GetHeadingReplacement(string text, Group group, Match match)
        {
            string tag = PairsTextTags.HeadingTag;
            string headingIndex = match.Groups["heading_index"].Value;
            string valueInside = GetValueInsidePairTag(tag, text, group.Index, group.Length);
            return PairsTextTags.GetHtmlHeadingTag(valueInside, headingIndex);           
        }

        private string ConvertToBase64(string path)
        {
            var imageArray = File.ReadAllBytes(path);
            return Convert.ToBase64String(imageArray);
        }
       
        public string ConvertAllQuestionTags(string text, IEnumerable<QuestionHyperlinkInfo> hyperlinks, IEnumerable<QuestionImageInfo> images, IEnumerable<QuestionFormulaInfo> formulas)
        {
            string result = text;           
            var patterns = new Dictionary<string, string>
            {
                { "Bold", PairsTextTags.BoldRegexPattern },
                { "Italic", PairsTextTags.ItalicRegexPattern },
                { "Underline", PairsTextTags.UnderlineRegexPattern },
                { "Blockquote", PairsTextTags.BlockquoteRegexPattern },
                { "Heading", PairsTextTags.HeadingRegexPattern },
                { "HorizontalRule", TextTags.HorizontalRuleRegexPattern },
                { "NewLine", TextTags.NewLineRegexPattern }                
            };

            if (hyperlinks != null & hyperlinks.Any()) patterns.Add("Hyperlink", TextTags.HyperlinkRegexPattern);
            if (images != null & images.Any()) patterns.Add("Image", TextTags.ImageRegexPattern);
            if (formulas != null & formulas.Any()) patterns.Add("Formula", TextTags.FormulaRegexPattern);

            result = ConvertAllBulletedListTagsToHtml(result);
            result = ConvertAllNumericListTagsToHtml(result);            
            result = ReplaceQuestionTags(result, patterns, images, formulas);            

            return result;
        }

        public string ConvertAllAnswerTags(string text, IEnumerable<AnswerHyperlinkInfo> hyperlinks, IEnumerable<AnswerImageInfo> images, IEnumerable<AnswerFormulaInfo> formulas)
        {
            string result = text;          
            var patterns = new Dictionary<string, string>
            {
                { "Bold", PairsTextTags.BoldRegexPattern },
                { "Italic", PairsTextTags.ItalicRegexPattern },
                { "Underline", PairsTextTags.UnderlineRegexPattern },
                { "Blockquote", PairsTextTags.BlockquoteRegexPattern },
                { "Heading", PairsTextTags.HeadingRegexPattern },
                { "HorizontalRule", TextTags.HorizontalRuleRegexPattern },
                { "NewLine", TextTags.NewLineRegexPattern }
            };

            if (hyperlinks != null & hyperlinks.Any()) patterns.Add("Hyperlink", TextTags.HyperlinkRegexPattern);
            if (images != null & images.Any()) patterns.Add("Image", TextTags.ImageRegexPattern);
            if (formulas != null & formulas.Any()) patterns.Add("Formula", TextTags.FormulaRegexPattern);

            result = ConvertAllBulletedListTagsToHtml(result);
            result = ConvertAllNumericListTagsToHtml(result);            
            result = ReplaceAnswerTags(result, patterns, images, formulas);

            return result;
        }        
    }
}

