using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Bibliomatic_MAUI_App.Services;
using Bibliomatic_MAUI_App.Models;
using Bibliomatic_MAUI_App.Views;
using Bibliomatic_MAUI_App.Models.AttachmentInfo;
using Bibliomatic_MAUI_App.Helpers;
using CSharpMath.SkiaSharp;
using System.Text.RegularExpressions;

namespace Bibliomatic_MAUI_App.ViewModels
{
    [QueryProperty(nameof(IsNewAnswer), "IsNewAnswer")]
    [QueryProperty(nameof(Answer), "Answer")]
    [QueryProperty(nameof(BaseQuestion), "BaseQuestion")]
    public partial class AnswerViewModel : ObservableObject, IQueryAttributable
    {        
        private readonly IRelatedDataService<BaseQuestionResponce, AnswerResponce> relatedDataRequestService;
        private readonly IFileService fileRequestService;
        private readonly ILocalFileKeeperService localFilesService;   
        
        private TagsToHtmlConverter tagsToHtmlConverter = new TagsToHtmlConverter();
        private LoadingService loadingService = new LoadingService();        
        private MathPainter mathPainter = new() { FontSize = 20 };

        private bool ShouldRevalidate { get; set; }

        #region Observable Properties

        [ObservableProperty]
        public bool isNewAnswer;

        [ObservableProperty]
        public AnswerResponce answer;

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

        [ObservableProperty]
        public bool isClearFormula;
        #endregion

        public AnswerViewModel(IRelatedDataService<BaseQuestionResponce, AnswerResponce> relatedDataRequestService, IFileService fileRequestService, ILocalFileKeeperService localFilesService)
        {
            this.fileRequestService = fileRequestService;
            this.relatedDataRequestService = relatedDataRequestService;
            this.localFilesService = localFilesService;

            relatedDataRequestService.ControllerName = "questions";
            relatedDataRequestService.RelatedRouteName = "answers";
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            BaseQuestion = (BaseQuestionResponce)query["BaseQuestion"];
            Answer = (AnswerResponce)query["Answer"];
            IsNewAnswer = (bool)query["IsNewAnswer"];

            localFilesService.BaseDirectory = $"{BaseQuestion.Id}/{Answer.Id}";
        }

        private bool IsNewLineOnPosition(string text, int position)
        {
            if (!string.IsNullOrEmpty(text))
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

            if (string.IsNullOrEmpty(Answer.AnswerBody))
            {
                errorMessage += "You need to fill answer body before publish";
            }
            
            if (!string.IsNullOrEmpty(errorMessage))
            {                
                PublishError = errorMessage;
                PublishErrorVisible = true;
            }
            else
            {
                PublishErrorVisible = false;
                await PublishAnswer();
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
            int indexOfHyperlink = Answer.Hyperlinks.Count + 1;

            string newCompositeTag = GetNewCompositeTag(TextTags.HyperlinkTag, hyperlink, description);

            Answer.Hyperlinks.Add(new AnswerHyperlinkInfo { Hyperlink = hyperlink, HyperlinkDescription = description, IndexOfHyperlink = indexOfHyperlink });

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
        public async Task AddImage(Editor answerEditor)
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

            string editorText = answerEditor?.Text ?? string.Empty;
            int cursorPosition = answerEditor.CursorPosition;
            int indexOfImage = Answer.Images.Count + 1;

            string newPath = await localFilesService.SaveFileToLocalStorage(GetImageStream(imageSource));
            string imageFilename = Path.GetRelativePath(localFilesService.LocalAppDataPath, newPath);
            string newCompositeTag = GetNewCompositeTag(TextTags.ImageTag, Path.GetFileName(imageFilename), imageDescription);

            var imageInfo = new AnswerImageInfo
            {
                ImageFilename = imageFilename,
                ImageFilepath = newPath,
                IndexOfImage = indexOfImage,
                ImageType = FileType.LocalFile
            };

            Answer.Images.Add(imageInfo);
            Answer.AllAttachedImages.Add(imageInfo);

            if (!IsNewLineOnPosition(editorText, cursorPosition)) newCompositeTag = "\n" + newCompositeTag;
           
            CancelAddingImageCommand.Execute(null);
            ShouldRevalidate = false;
            answerEditor.Text = editorText.Insert(cursorPosition, newCompositeTag);
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
        public async Task AddFormula(Editor answerEditor)
        {
            string formula = LatexFormula.Replace("\n", "");
            string formulaDescription = FormulaDescription.Replace("\n", "");

            if (string.IsNullOrEmpty(formula) && string.IsNullOrEmpty(formulaDescription))
            {
                await Application.Current.MainPage.DisplayAlert("Formula data not set", "You need to attach formula and fill formula description before adding. Please fill data and retry", "Got it");
                return;
            }               
            
            string editorText = answerEditor?.Text ?? string.Empty;
            int cursorPosition = answerEditor.CursorPosition;
            int indexOfFormula = Answer.Formulas.Count + 1;

            string newPath = await localFilesService.SaveFileToLocalStorage(Formula.GetFormulaStream(LatexFormula, mathPainter));
            string formulaFilename = Path.GetRelativePath(localFilesService.LocalAppDataPath, newPath);
            string newCompositeTag = GetNewCompositeTag(TextTags.FormulaTag, formula, formulaDescription);

            var formulaInfo = new AnswerFormulaInfo
            {
                FormulaLatex = formula,
                FormulaFilepath = newPath,
                FormulaFilename = formulaFilename,
                IndexOfFormula = indexOfFormula,
                FormulaType = FileType.LocalFile
            };

            Answer.Formulas.Add(formulaInfo);
            Answer.AllAttachedFormulas.Add(formulaInfo);

            if (!IsNewLineOnPosition(editorText, cursorPosition)) newCompositeTag = "\n" + newCompositeTag;
            
            CancelAddingFormulaCommand.Execute(null);
            ShouldRevalidate = false;
            IsClearFormula = true;
            answerEditor.Text = editorText.Insert(cursorPosition, newCompositeTag);
        }

        [RelayCommand]
        public void CancelAddingFormula()
        {            
            LatexFormula = string.Empty;
            FormulaDescription = string.Empty;
            FormulaAddingLayoutVisible = false;
            IsClearFormula = true;
        }

        [RelayCommand]
        public void AnswerEditorTextChanged(Editor answerEditor)
        {
            string text = answerEditor.Text;
            
            if (ShouldRevalidate)
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

            Answer.Hyperlinks.Clear();
            Answer.Images.Clear();
            Answer.Formulas.Clear();

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
                            RevalidateHyperlink(Answer, match);
                            break;
                        case "Image":
                            RevalidateImage(Answer, match);
                            break;
                        case "Formula":
                            RevalidateFormula(Answer, match);
                            break;
                    }

                    break;
                }
            }
        }

        [RelayCommand]
        public async Task NavigateToQuestionBodyView(string answerText)
        {
            await loadingService.PerformLoading("Loading answer preview...", async () =>
            {
                answerText ??= string.Empty;
                string html = tagsToHtmlConverter.ConvertAllAnswerTags(answerText, Answer.Hyperlinks, Answer.Images, Answer.Formulas);

                var answerHtml = new HtmlWebViewSource
                {
                    Html = $"<HTML><BODY>{html}</BODY></HTML>"
                };

                await Shell.Current.GoToAsync($"{nameof(QuestionBodyView)}", true, new Dictionary<string, object>
                {
                    {"Header", "Answer preview" },
                    {"ElementHtml", answerHtml},
                    {"QuestionHeader", BaseQuestion.Header},
                    {"QuestionDescription", BaseQuestion.Description}
                });
            });                    
        }

        public void RevalidateHyperlink(AnswerResponce answer, Match hyperlinkTag)
        {
            string hyperlink = hyperlinkTag.Groups["link"].Value;
            string hyperlinkDescription = hyperlinkTag.Groups["description"].Value;
            int indexOfHyperlink = answer.Hyperlinks.Count + 1;

            answer.Hyperlinks.Add(new AnswerHyperlinkInfo { Hyperlink = hyperlink, HyperlinkDescription = hyperlinkDescription, IndexOfHyperlink = indexOfHyperlink });
        }


        public void RevalidateImage(AnswerResponce answer, Match imageTag)
        {
            string filename = imageTag.Groups["filename"].Value;
            string newImagePath = "attachment_missing.png";
            string newImageName = newImagePath;

            int indexOfImage = answer.Images.Count + 1;
            var imageType = FileType.None;           

            var existingImageFile = answer.AllAttachedImages.FirstOrDefault(img => img.ShortImageFilename == filename);

            if (existingImageFile != null)
            {
                newImageName = existingImageFile.ImageFilename;
                newImagePath = existingImageFile.ImageFilepath;
                imageType = existingImageFile.ImageType;               
            }

            answer.Images.Add(new AnswerImageInfo
            {
                ImageFilename = newImageName,
                ImageFilepath = newImagePath,
                IndexOfImage = indexOfImage,
                ImageType = imageType,
                AnswerId = answer.Id
            });
        }
        
        public void RevalidateFormula(AnswerResponce answer, Match formulaTag)
        {
            string latex = formulaTag.Groups["latex"].Value;
            string newImage = "attachment_missing.png";
            string formulaFilename = newImage;

            int indexOfFormula = answer.Formulas.Count + 1;
            var formulaType = FileType.None;           

            var existingFormulaFile = answer.AllAttachedFormulas.FirstOrDefault(f => f.FormulaLatex == latex);

            if (existingFormulaFile != null)
            {
                newImage = existingFormulaFile.FormulaFilepath;
                formulaType = existingFormulaFile.FormulaType;
                formulaFilename = existingFormulaFile.FormulaFilename;                
            }

            answer.Formulas.Add(new AnswerFormulaInfo
            {
                FormulaLatex = latex,
                FormulaFilepath = newImage,
                FormulaFilename = formulaFilename,
                IndexOfFormula = indexOfFormula,
                FormulaType = formulaType,
                AnswerId = answer.Id
            });
        }    

        [RelayCommand]
        public async Task PublishAnswer()
        {           
            var filesList = GetAnswerFiles(Answer);
            var parameters = new Dictionary<string, object> { {"BaseQuestion", BaseQuestion }, {"Answer", Answer}, { "ScrollToAnswer", true } };

            await loadingService.PerformLoadingWithFilesUpload("Publishing answer...", "Back to main", "..", parameters, filesList, async (filesToUpload, loadedFiles) =>
            {
                var newFiles = new List<AttachmentDTOResponse>();

                if (filesToUpload.Count > 0)
                {
                    newFiles = await fileRequestService.UploadFiles(filesToUpload, localFilesService.LocalAppDataPath);                    
                }
                
                FilesAttachmentsService.AttachAnswerFiles(Answer, newFiles.Union(loadedFiles).ToList());
                await SaveAnswerHtml();                
                localFilesService.DeleteAllFilesInBaseDirectory();

                if (IsNewAnswer)
                {
                    await relatedDataRequestService.CreateRelatedData(BaseQuestion.Id, Answer);
                    BaseQuestion.AllAnswersCount++;
                    BaseQuestion.AnswersCount++;
                }
                else
                {
                    var unusedFiles = GetUnusedAttachments(Answer);

                    if (unusedFiles.Count > 0)
                    {
                        await fileRequestService.DeleteFiles(unusedFiles);
                    }
                   
                    await relatedDataRequestService.UpdateRelatedData(BaseQuestion.Id, Answer.Id, Answer);                    
                }
            });           
        }

        public async Task SaveAnswerHtml()
        {
            string answerDocumentFileName = "answer.html";
            string htmlContent = tagsToHtmlConverter.ConvertAllAnswerTags(Answer.AnswerBody, Answer.Hyperlinks, Answer.Images, Answer.Formulas);
            Answer.AnswerHtmlDocumentPath = await localFilesService.SaveFileToLocalStorage(htmlContent, answerDocumentFileName);

            var documentAttachment = await fileRequestService.UploadFile(Answer.AnswerHtmlDocumentPath, localFilesService.LocalAppDataPath);
            Answer.AnswerHtmlDocumentPath = documentAttachment.Attachment.Uri;
            Answer.AnswerHtmlDocument = documentAttachment.Attachment.Name;
            Answer.AnswerHtmlFormat = new HtmlWebViewSource { Html = $"<HTML><BODY>{htmlContent}</BODY></HTML>" };
        }

        private List<string> GetUnusedAttachments(AnswerResponce answer)
        {
            var allAnswerImagesLinks = answer.AllAttachedImages.Where(ai => ai.ImageType == FileType.Link).Select(ai => ai.ImageFilename);
            var allAnswerFormulasLinks = answer.AllAttachedFormulas.Where(af => af.FormulaType == FileType.Link).Select(af => af.FormulaFilename);

            var usedAnswerImagesLinks = answer.Images.Where(qi => qi.ImageType == FileType.Link).Select(qi => qi.ImageFilename);
            var usedAnswerFormulasLinks = answer.Formulas.Where(qf => qf.FormulaType == FileType.Link).Select(qf => qf.FormulaFilename);

            var unusedImages = allAnswerImagesLinks.Except(usedAnswerImagesLinks);
            var unusedFormulas = allAnswerFormulasLinks.Except(usedAnswerFormulasLinks);

            return unusedImages.Union(unusedFormulas).ToList();
        }

        private void RemoveMissingAttachments(AnswerResponce answer)
        {
            var missingImageFiles = answer.Images.Where(img => img.ImageFilename == "attachment_missing.png");
            var missingFormulaFiles = answer.Formulas.Where(formula => formula.FormulaFilename == "attachment_missing.png");

            foreach(var missingImageFile in missingImageFiles)
            {
               answer.Images.Remove(missingImageFile);
            }

            foreach (var missingFormulaFile in missingFormulaFiles)
            {
                answer.Formulas.Remove(missingFormulaFile);
            }
        }

        private List<string> GetAnswerFiles(AnswerResponce answer)
        {
            RemoveMissingAttachments(answer);
          
            var imagesFilesList = answer.Images.Where(img => img.ImageType == FileType.LocalFile).Select(img => img.ImageFilepath);
            var formulasFilesList = answer.Formulas.Where(formula => formula.FormulaType == FileType.LocalFile).Select(img => img.FormulaFilepath);

            return imagesFilesList.Union(formulasFilesList).ToList();            
        }        
    }
}
