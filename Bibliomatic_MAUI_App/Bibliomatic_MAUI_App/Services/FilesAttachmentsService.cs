using Bibliomatic_MAUI_App.Models;
using Bibliomatic_MAUI_App.Models.AttachmentInfo;
using System.Net.Mail;

namespace Bibliomatic_MAUI_App.Services
{
    public class FilesAttachmentsService
    {
        private static readonly FileRequestService fileService = new FileRequestService();
        private static readonly string testDefaultFileSource = "attach_element.png";
        private static readonly string baseQuestionDefaultFileSource = "attachment_missing.png";
        private static readonly string baseAttachmentPath = "https://bibliomaticfilesstorage.blob.core.windows.net/filestorage/";

        private static string GetAttachmentPath(string source, string defaultSource = null)
        {
            defaultSource ??= string.Empty;

            if (!string.IsNullOrEmpty(source))
            {
                return baseAttachmentPath + source;
            }

            return defaultSource;
        }

        private static FileType GetAttachmentFileType(string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                return FileType.Link;
            }

            return FileType.None;
        }

        private static void AddSourceIfNotDefault(List<string> files, string source, string defaultSource = null)
        {
            if (!string.IsNullOrEmpty(source))
            {
                if (defaultSource == null || (defaultSource != null & defaultSource != source))
                {
                    files.Add(source);
                }
            }
        }

        public static void GetValidArticlesAttachments(ArticleResponce article) => AttachArticleFiles(article);
        
        public static void AttachArticleFiles(ArticleResponce article, List<AttachmentDTOResponse> attachmentsDto)
        {
            var attachments = attachmentsDto.Select(a => a.Attachment).ToList();

            if (attachments.Count > 0)
            {
                article.ArticleDocumentPath = attachments.FirstOrDefault(attachment => attachment.Name == article.ArticleDocumentSource).Uri;
                article.ArticleImagePath = attachments.FirstOrDefault(attachment => attachment.Name == article.ArticleImageSource).Uri;
            }
        }

        private static void AttachArticleFiles(ArticleResponce article)
        {
            article.ArticleDocumentPath = GetAttachmentPath(article.ArticleDocumentSource);
            article.ArticleImagePath = GetAttachmentPath(article.ArticleImageSource);
        }

        public static void GetValidTestAttachments(TestResponse test) => AttachTestFiles(test);
        
        private static void AttachTestQuestion(TestQuestionResponse testQuestion, List<AttachmentResponse> attachments)
        {
            if(testQuestion.TestQuestionImageFileType == FileType.LocalFile)
            {
                var imageAttachment = attachments.FirstOrDefault(file => file.Name == testQuestion.TestQuestionImageFilename);

                testQuestion.TestQuestionImageFilename = imageAttachment.Name;
                testQuestion.TestQuestionImageFilepath = imageAttachment.Uri;
                testQuestion.TestQuestionImageFileType = FileType.Link;
            }
            
            if(testQuestion.TestQuestionFormulaImageFileType == FileType.LocalFile)
            {
                var formulaAttachment = attachments.FirstOrDefault(file => file.Name == testQuestion.TestQuestionFormulaImageFilename);

                testQuestion.TestQuestionFormulaImageFilename = formulaAttachment.Name;
                testQuestion.TestQuestionFormulaImageFilepath = formulaAttachment.Uri;
                testQuestion.TestQuestionFormulaImageFileType = FileType.Link;
            }      
        }

        private static void AttachTestAnswer(TestAnswerResponse testAnswer, List<AttachmentResponse> attachments)
        {
            if(testAnswer.TestAnswerImageFileType == FileType.LocalFile)
            {
                var answerImageAttachment = attachments.FirstOrDefault(file => file.Name == testAnswer.TestAnswerImageFilename);

                testAnswer.TestAnswerImageFilename = answerImageAttachment.Name;
                testAnswer.TestAnswerImageFilepath = answerImageAttachment.Uri;
                testAnswer.TestAnswerImageFileType = FileType.Link;
            }

            if(testAnswer.TestAnswerFormulaImageFileType == FileType.LocalFile)
            {
                var answerFormulaAttachment = attachments.FirstOrDefault(file => file.Name == testAnswer.TestAnswerFormulaImageFilename);

                testAnswer.TestAnswerFormulaImageFilename = answerFormulaAttachment.Name;
                testAnswer.TestAnswerFormulaImageFilepath = answerFormulaAttachment.Uri;
                testAnswer.TestAnswerFormulaImageFileType = FileType.Link;
            }           
        }

        private static void AttachTestVariant(TestAnswerResponse testAnswer, List<AttachmentResponse> attachments)
        {
            if (testAnswer.TestVariantImageFileType == FileType.LocalFile)
            {
                var variantImageAttachment = attachments.FirstOrDefault(file => file.Name == testAnswer.TestVariantImageFilename);

                testAnswer.TestVariantImageFilename = variantImageAttachment.Name;
                testAnswer.TestVariantImageFilepath = variantImageAttachment.Uri;
                testAnswer.TestVariantImageFileType = FileType.Link;
            }

            if (testAnswer.TestVariantFormulaImageFileType == FileType.LocalFile)
            {
                var variantFormulaAttachment = attachments.FirstOrDefault(file => file.Name == testAnswer.TestVariantFormulaImageFilename);

                testAnswer.TestVariantFormulaImageFilename = variantFormulaAttachment.Name;
                testAnswer.TestVariantFormulaImageFilepath = variantFormulaAttachment.Uri;
                testAnswer.TestVariantFormulaImageFileType = FileType.Link;
            }            
        }

        public static void AttachTestFiles(TestResponse test, List<AttachmentDTOResponse> attachmentsDto)
        {
            var attachments = attachmentsDto.Select(a => a.Attachment).ToList();
            int indexOfQuestion = 1;

            if (attachments.Count > 0)
            {
                foreach (var testQuestion in test.TestQuestions)
                {
                    AttachTestQuestion(testQuestion, attachments);

                    testQuestion.QuestionNumber = indexOfQuestion++;
                    int indexOfAnswer = 1;

                    foreach (var testAnswer in testQuestion.TestAnswers)
                    {
                        AttachTestAnswer(testAnswer, attachments);
                        AttachTestVariant(testAnswer, attachments);

                        testAnswer.AnswerNumber = indexOfAnswer++;
                    }
                }
            }
        }

        private static void AttachTestFiles(TestResponse test)
        {
            int indexOfQuestion = 1;

            foreach (var testQuestion in test.TestQuestions)
            {
                testQuestion.TestQuestionImageFilepath = GetAttachmentPath(testQuestion.TestQuestionImageFilename, testDefaultFileSource);
                testQuestion.TestQuestionImageFileType = GetAttachmentFileType(testQuestion.TestQuestionImageFilename);

                testQuestion.TestQuestionFormulaImageFilepath = GetAttachmentPath(testQuestion.TestQuestionFormulaImageFilename, testDefaultFileSource);
                testQuestion.TestQuestionFormulaImageFileType = GetAttachmentFileType(testQuestion.TestQuestionFormulaImageFilename);

                testQuestion.QuestionNumber = indexOfQuestion++;
                int indexOfAnswer = 1;

                foreach (var testAnswer in testQuestion.TestAnswers)
                {
                    testAnswer.TestAnswerImageFilepath = GetAttachmentPath(testAnswer.TestAnswerImageFilename, testDefaultFileSource);
                    testAnswer.TestAnswerImageFileType = GetAttachmentFileType(testAnswer.TestAnswerImageFilename);

                    testAnswer.TestAnswerFormulaImageFilepath = GetAttachmentPath(testAnswer.TestAnswerFormulaImageFilename, testDefaultFileSource);
                    testAnswer.TestAnswerFormulaImageFileType = GetAttachmentFileType(testAnswer.TestAnswerFormulaImageFilename);

                    testAnswer.TestVariantImageFilepath = GetAttachmentPath(testAnswer.TestVariantImageFilename, testDefaultFileSource);
                    testAnswer.TestVariantImageFileType = GetAttachmentFileType(testAnswer.TestVariantImageFilename);

                    testAnswer.TestVariantFormulaImageFilepath = GetAttachmentPath(testAnswer.TestVariantFormulaImageFilename, testDefaultFileSource);
                    testAnswer.TestVariantFormulaImageFileType = GetAttachmentFileType(testAnswer.TestVariantFormulaImageFilename);

                    testAnswer.AnswerNumber = indexOfAnswer++;
                }
            }
        }

        public static void GetValidQuestionAttachments(QuestionResponce question) => AttachQuestionFiles(question);
        public static void GetValidAnswerAttachments(AnswerResponce answer) => AttachAnswerFiles(answer);
        
        public static async Task GetValidBaseQuestionAttachments(BaseQuestionResponce baseQuestion)
        {           
            AttachBaseQuestionFiles(baseQuestion);

            var contentTasks = new List<Task>()
            {
                Task.Run(async () =>
                {
                    string questionBodyHtml = await fileService.GetFileContent(baseQuestion.Question.QuestionHtmlDocumentPath);

                    baseQuestion.Question.QuestionHtmlFormat = new HtmlWebViewSource
                    {
                         Html = $"<HTML><BODY>{questionBodyHtml}</BODY></HTML>"
                    };
                })
            };
            
            foreach (var answer in baseQuestion.Answers)
            {
                contentTasks.Add(Task.Run(async () =>
                {
                    string answerBodyHtml = await fileService.GetFileContent(answer.AnswerHtmlDocumentPath);

                    answer.MarkedAsAnswer = answer.IsAnswer;
                    answer.AnswerHtmlFormat = new HtmlWebViewSource
                    {
                        Html = $"<HTML><BODY>{answerBodyHtml}</BODY></HTML>"
                    };
                }));                
            }
                   
            await Task.WhenAll(contentTasks);            
        }

        public static async Task GetValidAnswersAttachments(IEnumerable<AnswerResponce> answers)
        {
            var answerContentTasks = new List<Task>();

            foreach (var answer in answers)
            {
                answerContentTasks.Add(Task.Run(async () =>
                {
                    AttachAnswerFiles(answer);
                    answer.AnswerHtmlDocumentPath = GetAttachmentPath(answer.AnswerHtmlDocument);
                    string answerBodyHtml = await fileService.GetFileContent(answer.AnswerHtmlDocumentPath);

                    answer.MarkedAsAnswer = answer.IsAnswer;
                    answer.AnswerHtmlFormat = new HtmlWebViewSource
                    {
                        Html = $"<HTML><BODY>{answerBodyHtml}</BODY></HTML>"
                    };
                }));
            }

            await Task.WhenAll(answerContentTasks);
        }

        private static void AttachBaseQuestionFiles(BaseQuestionResponce baseQuestion)
        {
            AttachQuestionFiles(baseQuestion.Question);
            baseQuestion.Question.QuestionHtmlDocumentPath = GetAttachmentPath(baseQuestion.Question.QuestionHtmlDocument);

            foreach (var answer in baseQuestion.Answers)
            {
                AttachAnswerFiles(answer);
                answer.AnswerHtmlDocumentPath = GetAttachmentPath(answer.AnswerHtmlDocument);
            }
        }
       
        public static List<string> GetAnswerFiles(AnswerResponce answer)
        {
            List<string> files = new List<string>();

            AddSourceIfNotDefault(files, answer.AnswerHtmlDocument);

            foreach (var answerImage in answer.Images)
            {
                AddSourceIfNotDefault(files, answerImage.ImageFilename, baseQuestionDefaultFileSource);
            }

            foreach (var answerFormula in answer.Formulas)
            {
                AddSourceIfNotDefault(files, answerFormula.FormulaFilename, baseQuestionDefaultFileSource);
            }

            return files;
        }

        public static List<string> GetQuestionFiles(QuestionResponce question)
        {
            List<string> files = new List<string>();

            AddSourceIfNotDefault(files, question.QuestionHtmlDocument);

            foreach (var questionImage in question.Images)
            {
                AddSourceIfNotDefault(files, questionImage.ImageFilename, baseQuestionDefaultFileSource);
            }

            foreach (var questionFormula in question.Formulas)
            {
                AddSourceIfNotDefault(files, questionFormula.FormulaFilename, baseQuestionDefaultFileSource);
            }

            return files;
        }

        public static void AttachAnswerFiles(AnswerResponce answer, List<AttachmentDTOResponse> attachmentsDto)
        {
            var attachments = attachmentsDto.Select(a => a.Attachment).ToList();
            int indexOfHyperlink = 1;

            foreach (var answerHyperlink in answer.Hyperlinks)
            {
                answerHyperlink.IndexOfHyperlink = indexOfHyperlink++;
            }

            if (attachments.Count > 0)
            {
                int indexOfImage = 1;

                foreach (var answerImage in answer.Images.Where(img => img.ImageType == FileType.LocalFile))
                {
                    var attachment = attachments.FirstOrDefault(file => file.Name == answerImage.ImageFilename);

                    answerImage.ImageFilepath = attachment.Uri;
                    answerImage.ImageType = FileType.Link;
                    answerImage.IndexOfImage = indexOfImage++;
                    answerImage.AnswerId = answer.Id;
                }

                int indexOfFormula = 1;

                foreach (var formulaImage in answer.Formulas.Where(formula => formula.FormulaType == FileType.LocalFile))
                {
                    var attachment = attachments.FirstOrDefault(file => file.Name == formulaImage.FormulaFilename);

                    formulaImage.FormulaFilepath = attachment.Uri;
                    formulaImage.FormulaType = FileType.Link;
                    formulaImage.IndexOfFormula = indexOfFormula++;
                    formulaImage.AnswerId = answer.Id;
                }
            }
        }

        private static void AttachAnswerFiles(AnswerResponce answer)
        {
            int indexOfHyperlink = 1;

            foreach (var answerHyperlink in answer.Hyperlinks)
            {
                answerHyperlink.IndexOfHyperlink = indexOfHyperlink++;
            }

            int indexOfImage = 1;

            foreach (var answerImage in answer.Images)
            {
                answerImage.ImageFilepath = GetAttachmentPath(answerImage.ImageFilename);
                answerImage.ImageType = GetAttachmentFileType(answerImage.ImageFilename);
                answerImage.IndexOfImage = indexOfImage++;
            }

            int indexOfFormula = 1;

            foreach (var formulaImage in answer.Formulas)
            {
                formulaImage.FormulaFilepath = GetAttachmentPath(formulaImage.FormulaFilename);
                formulaImage.FormulaType = GetAttachmentFileType(formulaImage.FormulaFilename);
                formulaImage.IndexOfFormula = indexOfFormula++;
            }
        }

        public static void AttachQuestionFiles(QuestionResponce question, List<AttachmentDTOResponse> attachmentsDto)
        {
            var attachments = attachmentsDto.Select(a => a.Attachment).ToList();
            int indexOfHyperlink = 1;

            foreach (var questionHyperlink in question.Hyperlinks)
            {
                questionHyperlink.IndexOfHyperlink = indexOfHyperlink++;
            }

            if (attachments.Count > 0)
            {
                int indexOfImage = 1;

                foreach (var questionImage in question.Images.Where(img => img.ImageType == FileType.LocalFile))
                {
                    var attachment = attachments.FirstOrDefault(file => file.Name == questionImage.ImageFilename);

                    questionImage.ImageFilepath = attachment.Uri;
                    questionImage.ImageType = FileType.Link;
                    questionImage.IndexOfImage = indexOfImage++;
                    questionImage.QuestionId = question.Id;
                }

                int indexOfFormula = 1;

                foreach (var formulaImage in question.Formulas.Where(formula => formula.FormulaType == FileType.LocalFile))
                {
                    var attachment = attachments.FirstOrDefault(file => file.Name == formulaImage.FormulaFilename);

                    formulaImage.FormulaFilepath = attachment.Uri;
                    formulaImage.FormulaType = FileType.Link;
                    formulaImage.IndexOfFormula = indexOfFormula++;
                    formulaImage.QuestionId = question.Id;
                }
            }
        }

        private static void AttachQuestionFiles(QuestionResponce question)
        {
            int indexOfHyperlink = 1;

            foreach (var questionHyperlink in question.Hyperlinks)
            {
                questionHyperlink.IndexOfHyperlink = indexOfHyperlink++;
            }

            int indexOfImage = 1;

            foreach (var questionImage in question.Images)
            {
                questionImage.ImageFilepath = GetAttachmentPath(questionImage.ImageFilename);
                questionImage.ImageType = GetAttachmentFileType(questionImage.ImageFilename);
                questionImage.IndexOfImage = indexOfImage++;
            }

            int indexOfFormula = 1;

            foreach (var formulaImage in question.Formulas)
            {
                formulaImage.FormulaFilepath = GetAttachmentPath(formulaImage.FormulaFilename);
                formulaImage.FormulaType = GetAttachmentFileType(formulaImage.FormulaFilename);
                formulaImage.IndexOfFormula = indexOfFormula++;
            }
        }
    }
}
