using Bibliomatic_MAUI_App.Helpers;
using Bibliomatic_MAUI_App.Models;
using Bibliomatic_MAUI_App.Models.AttachmentInfo;
using Bibliomatic_MAUI_App.Services;
using Bibliomatic_MAUI_App.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharpMath.SkiaSharp;
using System.Text.RegularExpressions;

namespace Bibliomatic_MAUI_App.ViewModels
{
    [QueryProperty(nameof(IsNewQuestion), "IsNewQuestion")]
    [QueryProperty(nameof(Question), "Question")]
    [QueryProperty(nameof(BaseQuestion), "BaseQuestion")]
    public partial class QuestionViewModel : ObservableObject, IQueryAttributable
    {       
        private readonly IDataService<BaseQuestionResponce> dataRequestService;
        private readonly IFileService fileRequestService;
        private readonly ILocalFileKeeperService localFilesService;

        private LoadingService loadingService = new LoadingService();
        private TagsToHtmlConverter tagsToHtmlConverter = new TagsToHtmlConverter();          
        private MathPainter mathPainter = new MathPainter() { FontSize = 20 };    
        
        private bool ShouldRevalidate { get; set; }

        #region Observable Properties

        [ObservableProperty]
        public bool isNewQuestion;

        [ObservableProperty]
        public bool isClearFormula;

        [ObservableProperty]
        public QuestionResponce question;

        [ObservableProperty]
        public BaseQuestionResponce baseQuestion;

        [ObservableProperty]
        public string publishError;       

        [ObservableProperty]
        public bool publishErrorVisible;

        [ObservableProperty]
        public bool hyperlinkAddingLayoutVisible = false;

        [ObservableProperty]
        public bool imageAddingLayoutVisible = false;

        [ObservableProperty]
        public bool formulaAddingLayoutVisible = false;

        [ObservableProperty]
        public string hyperlink = "https://";

        [ObservableProperty]
        public string hyperlinkDescription;

        [ObservableProperty]
        public string imageSource = "attach_element.png";

        [ObservableProperty]
        public string imageDescription;

        [ObservableProperty]
        public string latexFormula;

        [ObservableProperty]
        public string formulaDescription;
        #endregion

        public QuestionViewModel(IDataService<BaseQuestionResponce> dataRequestService, IFileService fileRequestService, ILocalFileKeeperService localFilesService)
        {
            this.fileRequestService = fileRequestService;
            this.dataRequestService = dataRequestService;
            this.localFilesService = localFilesService;
            dataRequestService.ControllerName = "questions";            
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            BaseQuestion = (BaseQuestionResponce)query["BaseQuestion"];
            Question = (QuestionResponce)query["Question"];
            IsNewQuestion = (bool)query["IsNewQuestion"];            

            localFilesService.BaseDirectory = $"{BaseQuestion.Id}/{Question.Id}";
        }

        private bool IsNewLineOnPosition(string text, int position)
        {            
            if(!string.IsNullOrEmpty(text))
            {
                int previousPosition = position - 1;

                if (previousPosition > 0)
                    return text[previousPosition] == '\n';

                return text[position] == '\n';
            }

            return true;
        }

        public string GetNewCompositeTag(string tag, string value, string description)
        {
            string[] separatedHyperlinkTag = tag.Split(';');

            string openingTag = separatedHyperlinkTag[0];
            string valuePart = separatedHyperlinkTag[1].Insert(1, value);
            string descriptionPart = separatedHyperlinkTag[2].Insert(1, description);
            string closingTag = separatedHyperlinkTag[3];

            return openingTag + valuePart + descriptionPart + closingTag;
        }

        [RelayCommand]
        public async Task ReturnBack()
        {
            bool deleteAction = await Application.Current.MainPage.DisplayAlert("Return back", "Are you sure you want to go back? All unsaved data will be lost", "Yes", "Cancel");

            if (!deleteAction) return;

            localFilesService.DeleteAllFilesInBaseDirectory();
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task CheckForUnfilledData()
        {
            string errorMessage = string.Empty;

            if (string.IsNullOrEmpty(BaseQuestion.Header))
            {
                errorMessage += "You need to fill question header";
            }

            if (string.IsNullOrEmpty(BaseQuestion.Description))
            {
                errorMessage += string.IsNullOrEmpty(errorMessage) ? "You need to fill question description" : ", question description";
            }

            if (string.IsNullOrEmpty(BaseQuestion.Question.QuestionBody))
            {
                errorMessage += string.IsNullOrEmpty(errorMessage) ? "You need to fill question body" : " and fill question body";
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                errorMessage += " before publish";
                PublishError = errorMessage;
                PublishErrorVisible = true;
            }
            else
            {
                PublishErrorVisible = false;
                await PublishBaseQuestion();
            }
        }

        [RelayCommand]
        public void HyperlinkClicked() => HyperlinkAddingLayoutVisible = !HyperlinkAddingLayoutVisible;        

        [RelayCommand]
        public async Task AddHyperlink(Editor questionEditor)
        {
            string hyperlink = Hyperlink;
            string description = HyperlinkDescription;

            if (string.IsNullOrEmpty(hyperlink) && string.IsNullOrEmpty(description))
            {
                await Application.Current.MainPage.DisplayAlert("Hyperlink data not set", "You need to fill link and hyperlink description before adding. Please fill data and retry", "Got it");
                return;
            }
            
            string editorText = questionEditor?.Text ?? string.Empty;
            int cursorPosition = questionEditor.CursorPosition;
            int indexOfHyperlink = Question.Hyperlinks.Count + 1;

            string newCompositeTag = GetNewCompositeTag(TextTags.HyperlinkTag, hyperlink, description);

            Question.Hyperlinks.Add(new QuestionHyperlinkInfo {Hyperlink = hyperlink, HyperlinkDescription = description, IndexOfHyperlink = indexOfHyperlink });            

            if (!IsNewLineOnPosition(editorText, cursorPosition))
                newCompositeTag = "\n" + newCompositeTag;
            
            CancelAddingHyperlinkCommand.Execute(null);
            ShouldRevalidate = false;
            questionEditor.Text = editorText.Insert(cursorPosition, newCompositeTag);
        }

        [RelayCommand]
        public void CancelAddingHyperlink()
        {
            Hyperlink = "https://";
            HyperlinkDescription = string.Empty;
            HyperlinkAddingLayoutVisible = false;
        }

        [RelayCommand]
        public void ImageClicked() => ImageAddingLayoutVisible = !ImageAddingLayoutVisible;

        [RelayCommand]
        public async Task PickImageFromDevice()
        {
            bool permissionsGranted = await PermissionsChecker.CheckForRequiredPermissions();
            if (!permissionsGranted) return;

            var supportedImageExtension = new List<string> { ".jpg", ".png", ".jpeg" };
            var selectedImage = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Please select image"
            });

            if (selectedImage == null) return;

            string selectedImageExtension = Path.GetExtension(selectedImage.FileName);

            if (!supportedImageExtension.Contains(selectedImageExtension))
            {
                await Application.Current.MainPage.DisplayAlert("Wrong image extension", "You can only attach images with PNG, JPG, JPEG extensions. Please select another file to continue", "Got it");
                return;
            }            

            ImageSource = selectedImage.FullPath;
        }

        private Stream GetImageStream(string path)
        {
            return File.OpenRead(path);
        }        


        [RelayCommand]
        public async Task AddImage(Editor questionEditor)
        {
            bool permissionsGranted = await PermissionsChecker.CheckForRequiredPermissions();
            if (!permissionsGranted) return;

            string imageSource = ImageSource;
            string imageDescription = ImageDescription.Replace("\n", "");

            if ((imageSource == "attach_element.png" || string.IsNullOrEmpty(imageSource)) && string.IsNullOrEmpty(imageDescription))
            {
                await Application.Current.MainPage.DisplayAlert("Image data not set", "You need to attach image and fill image description before adding. Please fill data and retry", "Got it");
                return;
            }

            string editorText = questionEditor?.Text ?? string.Empty;            
            int cursorPosition = questionEditor.CursorPosition;
            int indexOfImage = Question.Images.Count + 1;

            string newPath = await localFilesService.SaveFileToLocalStorage(GetImageStream(imageSource));
            string imageFilename = Path.GetRelativePath(localFilesService.LocalAppDataPath, newPath);
            string newCompositeTag = GetNewCompositeTag(TextTags.ImageTag, Path.GetFileName(imageFilename), imageDescription);            

            var imageInfo = new QuestionImageInfo 
            {                 
                ImageFilename = imageFilename, 
                ImageFilepath = newPath, 
                IndexOfImage = indexOfImage, 
                ImageType = FileType.LocalFile
            };

            Question.Images.Add(imageInfo);
            Question.AllAttachedImages.Add(imageInfo);            

            if (!IsNewLineOnPosition(editorText, cursorPosition)) newCompositeTag = "\n" + newCompositeTag;
           
            CancelAddingImageCommand.Execute(null);
            ShouldRevalidate = false;
            questionEditor.Text = editorText.Insert(cursorPosition, newCompositeTag);
        }

        [RelayCommand]
        public void CancelAddingImage()
        {
            ImageSource = "attach_element.png";
            ImageDescription = string.Empty;
            ImageAddingLayoutVisible = false;
        }

        [RelayCommand]
        public void FormulaClicked() => FormulaAddingLayoutVisible = !FormulaAddingLayoutVisible;        

        [RelayCommand]
        public async Task AddFormula(Editor questionEditor)
        {
            bool permissionsGranted = await PermissionsChecker.CheckForRequiredPermissions();
            if (!permissionsGranted) return;

            string formula = LatexFormula.Replace("\n", ""); 
            string formulaDescription = FormulaDescription.Replace("\n", "");

            if (string.IsNullOrEmpty(formula) && string.IsNullOrEmpty(formulaDescription))
            {
                await Application.Current.MainPage.DisplayAlert("Formula data not set", "You need to attach formula and fill formula description before adding. Please fill data and retry", "Got it");
                return;
            }
            
            string editorText = questionEditor?.Text ?? string.Empty;
            int cursorPosition = questionEditor.CursorPosition;
            int indexOfFormula = Question.Formulas.Count + 1;

            string newPath = await localFilesService.SaveFileToLocalStorage(Formula.GetFormulaStream(LatexFormula, mathPainter));
            string formulaFilename = Path.GetRelativePath(localFilesService.LocalAppDataPath, newPath);
            string newCompositeTag = GetNewCompositeTag(TextTags.FormulaTag, formula, formulaDescription);

            var formulaInfo = new QuestionFormulaInfo 
            {                 
                FormulaLatex = formula, 
                FormulaFilepath = newPath, 
                FormulaFilename = formulaFilename,
                IndexOfFormula = indexOfFormula, 
                FormulaType = FileType.LocalFile               
            };

            Question.Formulas.Add(formulaInfo);
            Question.AllAttachedFormulas.Add(formulaInfo);           

            if (!IsNewLineOnPosition(editorText, cursorPosition)) newCompositeTag = "\n" + newCompositeTag;
            
            CancelAddingFormulaCommand.Execute(null);
            ShouldRevalidate = false;
            IsClearFormula = true;

            questionEditor.Text = editorText.Insert(cursorPosition, newCompositeTag);
        }

        [RelayCommand]
        public void CancelAddingFormula()
        {
            IsClearFormula = true;
            FormulaDescription = string.Empty;
            FormulaAddingLayoutVisible = false;
        }

        [RelayCommand]
        public void QuestionEditorTextChanged(Editor questionEditor)
        {
            string text = questionEditor.Text;

            if(ShouldRevalidate)
            {
                RevalidateAttachments(text);
            }

            ShouldRevalidate = true;
        }

        private void RevalidateAttachments(string text)
        {
            var patterns = new Dictionary<string, string>()
            {
                {"Hyperlink", TextTags.HyperlinkRegexPattern},
                {"Image", TextTags.ImageRegexPattern},
                {"Formula", TextTags.FormulaRegexPattern}
            };

            var attachmentsRegex = tagsToHtmlConverter.GetTagsRegex(patterns);

            Question.Hyperlinks.Clear();
            Question.Images.Clear();
            Question.Formulas.Clear();

            foreach (Match match in attachmentsRegex.Matches(text).Cast<Match>())
            {
                foreach (var pattern in patterns)
                {
                    var attachmentGroup = match.Groups[pattern.Key];

                    if (!attachmentGroup.Success)
                        continue;

                    switch (pattern.Key)
                    {
                        case "Hyperlink":
                            RevalidateHyperlink(Question, match);
                            break;
                        case "Image":
                            RevalidateImage(Question, match);
                            break;
                        case "Formula":
                            RevalidateFormula(Question, match);
                            break;
                    }

                    break;
                }
            }
        }

        [RelayCommand]
        public async Task NavigateToQuestionBodyView(string questionText)
        {
            await loadingService.PerformLoading("Loading question preview...", async () =>
            {
                questionText ??= string.Empty;
                string html = tagsToHtmlConverter.ConvertAllQuestionTags(questionText, Question.Hyperlinks, Question.Images, Question.Formulas);

                var questionHtml = new HtmlWebViewSource
                {
                    Html = $"<HTML><BODY>{html}</BODY></HTML>"
                };

                await Shell.Current.GoToAsync($"{nameof(QuestionBodyView)}", true, new Dictionary<string, object>
                {
                    {"Header", "Question preview" },
                    {"ElementHtml", questionHtml},
                    {"QuestionHeader", BaseQuestion.Header},
                    {"QuestionDescription", BaseQuestion.Description}
                });
            });            
        }

        public void RevalidateHyperlink(QuestionResponce question, Match hyperlinkTag)
        {
            string hyperlink = hyperlinkTag.Groups["link"].Value;
            string hyperlinkDescription = hyperlinkTag.Groups["description"].Value;
            int indexOfHyperlink = question.Hyperlinks.Count + 1;

            question.Hyperlinks.Add(new QuestionHyperlinkInfo { Hyperlink = hyperlink, HyperlinkDescription = hyperlinkDescription, IndexOfHyperlink = indexOfHyperlink });
        }


        public void RevalidateImage(QuestionResponce question, Match imageTag)
        {
            string filename = imageTag.Groups["filename"].Value;
            string newImagePath = "attachment_missing.png";
            string newImageName = newImagePath;

            int indexOfImage = question.Images.Count + 1;
            var imageType = FileType.None;            

            var existingImageFile = question.AllAttachedImages.FirstOrDefault(img => img.ShortImageFilename == filename);

            if (existingImageFile != null)
            {
                newImageName = existingImageFile.ImageFilename;
                newImagePath = existingImageFile.ImageFilepath;
                imageType = existingImageFile.ImageType;                
            }

            question.Images.Add(new QuestionImageInfo
            {
                ImageFilename = newImageName,
                ImageFilepath = newImagePath,
                IndexOfImage = indexOfImage,
                ImageType = imageType,
                QuestionId = question.Id
            });
        }

        public void RevalidateFormula(QuestionResponce question, Match formulaTag)
        {
            string latex = formulaTag.Groups["latex"].Value;
            string newImage = "attachment_missing.png";
            string formulaFilename = newImage;

            int indexOfFormula = question.Formulas.Count + 1;
            var formulaType = FileType.None;            

            var existingFormulaFile = question.AllAttachedFormulas.FirstOrDefault(f => f.FormulaLatex == latex);

            if (existingFormulaFile != null)
            {
                newImage = existingFormulaFile.FormulaFilepath;
                formulaType = existingFormulaFile.FormulaType;
                formulaFilename = existingFormulaFile.FormulaFilename;                
            }

            question.Formulas.Add(new QuestionFormulaInfo
            {
                FormulaLatex = latex,
                FormulaFilepath = newImage,
                FormulaFilename = formulaFilename,
                IndexOfFormula = indexOfFormula,
                FormulaType = formulaType,
                QuestionId = question.Id
            });
        }

        [RelayCommand]
        public async Task PublishBaseQuestion()
        {            
            var filesList = GetQuestionFiles(Question);

            await loadingService.PerformLoadingWithFilesUpload("Publishing question...", "Back to main", filesList, async (filesToUpload, loadedFiles) =>
            {
                var newFiles = new List<AttachmentDTOResponse>();

                if (filesList.Count > 0)
                {
                    newFiles = await fileRequestService.UploadFiles(filesList, localFilesService.LocalAppDataPath);                    
                }
                
                FilesAttachmentsService.AttachQuestionFiles(Question, newFiles.Union(loadedFiles).ToList());
                await SaveQuestionHtml();
                localFilesService.DeleteAllFilesInBaseDirectory();

                if (IsNewQuestion)
                {
                    await dataRequestService.CreateData(BaseQuestion);                   
                }
                else
                {
                    var unusedFiles = GetUnusedAttachments(Question);

                    if (unusedFiles.Count > 0)
                    {
                        await fileRequestService.DeleteFiles(unusedFiles);
                    }

                    await dataRequestService.UpdateData(BaseQuestion.Id, BaseQuestion);
                }
            });           
        }

        public async Task SaveQuestionHtml()
        {
            string questionDocumentFileName = "question.html";
            string htmlContent = tagsToHtmlConverter.ConvertAllQuestionTags(Question.QuestionBody, Question.Hyperlinks, Question.Images, Question.Formulas);
            Question.QuestionHtmlDocumentPath = await localFilesService.SaveFileToLocalStorage(htmlContent, questionDocumentFileName);

            var documentAttachment = await fileRequestService.UploadFile(Question.QuestionHtmlDocumentPath, localFilesService.LocalAppDataPath);
            Question.QuestionHtmlDocumentPath = documentAttachment.Attachment.Uri;
            Question.QuestionHtmlDocument = documentAttachment.Attachment.Name;
            Question.QuestionHtmlFormat = new HtmlWebViewSource { Html = $"<HTML><BODY>{htmlContent}</BODY></HTML>" };
        }

        private List<string> GetUnusedAttachments(QuestionResponce question)
        {
            var allQuestionImagesLinks = question.AllAttachedImages.Where(ai => ai.ImageType == FileType.Link).Select(ai => ai.ImageFilename);
            var allQuestionFormulasLinks = question.AllAttachedFormulas.Where(af => af.FormulaType == FileType.Link).Select(af => af.FormulaFilename);

            var usedQuestionImagesLinks = question.Images.Where(qi => qi.ImageType == FileType.Link).Select(qi => qi.ImageFilename);
            var usedQuestionFormulasLinks = question.Formulas.Where(qf => qf.FormulaType == FileType.Link).Select(qf => qf.FormulaFilename);

            var unusedImages = allQuestionImagesLinks.Except(usedQuestionImagesLinks);
            var unusedFormulas = allQuestionFormulasLinks.Except(usedQuestionFormulasLinks);

            return unusedImages.Union(unusedFormulas).ToList();
        }

        private void RemoveMissingAttachments(QuestionResponce question)
        {
            var missingImageFiles = question.Images.Where(img => img.ImageFilename == "attachment_missing.png");
            var missingFormulaFiles = question.Formulas.Where(formula => formula.FormulaFilename == "attachment_missing.png");

            foreach (var missingImageFile in missingImageFiles)
            {
                question.Images.Remove(missingImageFile);
            }

            foreach (var missingFormulaFile in missingFormulaFiles)
            {
                question.Formulas.Remove(missingFormulaFile);
            }
        }

        private List<string> GetQuestionFiles(QuestionResponce question)
        {
            RemoveMissingAttachments(question);
           
            var imagesFilesList = question.Images.Where(img => img.ImageType == FileType.LocalFile).Select(img => img.ImageFilepath);
            var formulasFilesList = question.Formulas.Where(formula => formula.FormulaType == FileType.LocalFile).Select(img => img.FormulaFilepath);

            return imagesFilesList.Union(formulasFilesList).ToList();           
        }       
    }
}
