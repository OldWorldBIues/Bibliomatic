using Bibliomatic_MAUI_App.Helpers;
using Bibliomatic_MAUI_App.Models;
using Bibliomatic_MAUI_App.Models.AttachmentInfo;
using Bibliomatic_MAUI_App.Services;
using Bibliomatic_MAUI_App.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharpMath.SkiaSharp;
using Syncfusion.Maui.DataSource.Extensions;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Bibliomatic_MAUI_App.ViewModels
{
    [QueryProperty(nameof(IsNewTest), "IsNewTest")]
    [QueryProperty(nameof(Test), "Test")]
    public partial class TestEditorViewModel : ObservableObject, IQueryAttributable
    {           
        private readonly string defaultImagePath = "attach_element.png";
        private LoadingService loadingService = new LoadingService();
        private MathPainter mathPainter = new MathPainter() { FontSize = 20 };

        public readonly List<string> deletedFiles = new List<string>();
        public List<TestQuestionType> TestQuestionTypes { get; set; } = new List<TestQuestionType>();
        public ObservableCollection<TestQuestionTypeValues> TestQuestionTypesValues { get; set; } = new ObservableCollection<TestQuestionTypeValues>();       

        [ObservableProperty]
        public TestResponse test;

        [ObservableProperty]
        public bool isDisplayQuestionTypesPickerControl;

        [ObservableProperty]
        public bool isNewTest;

        [ObservableProperty]
        public string publishError;

        [ObservableProperty]
        public bool publishErrorVisible;

        [ObservableProperty]
        public TestQuestionTypeValues selectedQuestionTypeValue;
        
        private readonly IDataService<TestResponse> dataRequestService;
        private readonly IFileService fileRequestService;
        private readonly ILocalFileKeeperService testQuestionsKeeper;
        private readonly ILocalFileKeeperService testAnswersKeeper;
        private readonly ILocalFileKeeperService testFilesKeeper;
        private readonly ILocalObjectKeeperService<TestResponse> testKeeper;

        public TestEditorViewModel(IDataService<TestResponse> dataRequestService, IFileService fileRequestService, ILocalFileKeeperService testFilesKeeper,
                                   ILocalFileKeeperService testQuestionsKeeper, ILocalFileKeeperService testAnswersKeeper, ILocalObjectKeeperService<TestResponse> testKeeper)
        {
            this.dataRequestService = dataRequestService;
            this.fileRequestService = fileRequestService;
            this.testQuestionsKeeper = testQuestionsKeeper;
            this.testAnswersKeeper = testAnswersKeeper;
            this.testFilesKeeper = testAnswersKeeper;
            this.testKeeper = testKeeper;

            dataRequestService.ControllerName = "tests";
            FillTestQuestionTypes();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Test = (TestResponse)query["Test"];
            IsNewTest = (bool)query["IsNewTest"];

            testKeeper.BaseDirectory = $"{Test.Id}";
            testFilesKeeper.BaseDirectory = $"{Test.Id}";
        }

        private string GetBaseDirectoryForCurrentTestAnswer(TestAnswerResponse testAnswer)
        {
            return $"{Test.Id}/{testAnswer.TestQuestion.Id}/{testAnswer.Id}";
        }

        private string GetBaseDirectoryForCurrentTestQuestion(TestQuestionResponse testQuestion)
        {
            return $"{Test.Id}/{testQuestion.Id}";
        }
       
        private void FillTestQuestionTypes()
        {
            if(!TestQuestionTypesValues.Any())
            {
                List<TestQuestionType> testQuestionTypes = Enum.GetValues(typeof(TestQuestionType))
                                                          .Cast<TestQuestionType>()
                                                          .Where(tqt => tqt != TestQuestionType.SpacesQuestion)                                                          
                                                          .ToList();

                testQuestionTypes.ForEach(qt => TestQuestionTypesValues.Add(new TestQuestionTypeValues(qt)));
            }
        }
      
        [RelayCommand]
        public async Task ReturnBack()
        {
            bool deleteAction = await Application.Current.MainPage.DisplayAlert("Return back", "Are you sure you want to go back? All unsaved data will be lost", "Yes", "Cancel");

            if (!deleteAction) return;

            testFilesKeeper.DeleteAllFilesInBaseDirectoryAndSubdirectories();
            await Shell.Current.GoToAsync("..");
        }

        private string CheckForUnfilledTestQuestionAnswers(string errorMessage)
        {
            bool unfilledTestQuestionFound = false;

            foreach(var testQuestion in Test.TestQuestions)
            {
                var type = testQuestion.TestQuestionType;
                testQuestion.AnswerUnfilled = false;

                if(type == TestQuestionType.PairsQuestion || type == TestQuestionType.OpenEndedQuestion)
                {
                    if (!testQuestion.TestAnswers.Any())
                    {
                        testQuestion.AnswerUnfilled = true;
                        unfilledTestQuestionFound = true;
                    }
                }
                else
                {
                    if (!testQuestion.TestAnswers.Any(ta => ta.IsCorrectAnswer))
                    {
                        testQuestion.AnswerUnfilled = true;
                        unfilledTestQuestionFound = true;
                    }
                }                
            }

            if(unfilledTestQuestionFound)
            {
                errorMessage += string.IsNullOrEmpty(errorMessage) ? "You need to select at least one answer for each test question." : " and select at least one answer for each test question.";
                errorMessage += " All test question without answer marked with orange color";
            }

            return errorMessage;
        }

        [RelayCommand]
        public async Task CheckForUnfilledData()
        {
            string errorMessage = string.Empty;

            if (string.IsNullOrEmpty(Test.Name))
            {
                errorMessage += "You need to fill test header";
            }

            if (string.IsNullOrEmpty(Test.Description))
            {
                errorMessage += string.IsNullOrEmpty(errorMessage) ? "You need to fill test description" : ", test description";
            }

            if (!Test.TestQuestions.Any())
            {
                errorMessage += string.IsNullOrEmpty(errorMessage) ? "You need to add at least one test question" : ", add at least one test question";
            }

            errorMessage = CheckForUnfilledTestQuestionAnswers(errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
            {                
                PublishError = errorMessage;
                PublishErrorVisible = true;
            }
            else
            {
                PublishErrorVisible = false;
                await PublishTest();
            }
        }
       

        [RelayCommand] 
        public void AddTestQuestion()
        {        
            var questionToBeAdded = new TestQuestionResponse
            {
                Id = Guid.NewGuid(),
                Question = string.Empty,
                Test = Test,
                TestId = Test.Id,
                AnswerUnfilled = false,
                ChangedByControls = false, 
                PointsPerAnswer = 0,
                QuestionNumber = Test.TestQuestions.Count + 1,
                TestQuestionType = SelectedQuestionTypeValue.Type,
                TestQuestionImageFilename = string.Empty,
                TestQuestionImageFilepath = defaultImagePath,
                TestQuestionImageFileType = FileType.None,
                QuestionImageFilenameBeforeEdit = null,
                TestQuestionFormulaImageFilename = string.Empty,
                TestQuestionFormulaImageFilepath = defaultImagePath,
                TestQuestionFormulaImageFileType = FileType.None,
                QuestionFormulaImageFilenameBeforeEdit = null,
                SelectedPickerTestAnswer = null,
                TestAnswers = new ObservableCollection<TestAnswerResponse>(),
                DeletedTestAnswers = new ObservableCollection<KeyValuePair<int, TestAnswerResponse>>(),
                CorrectSpacesValues = new List<string>(),                
                AllSpacesVariants = new List<SpaceTestVariantResponse>()                
            };
                     
            Test.TestQuestions.Add(questionToBeAdded);
        }       
       

        [RelayCommand]
        public async Task DeleteTestQuestion(TestQuestionResponse testQuestion)
        {
            bool deleteAction = await Application.Current.MainPage.DisplayAlert("Delete test question", "You sure that you want to delete this test question? All test question data test will be lost", "Yes", "Cancel");

            if (!deleteAction) return;

            AddSourceIfNotDefault(deletedFiles, testQuestion.QuestionImageFilenameBeforeEdit, testQuestion.QuestionImageFileTypeBeforeEdit, FileType.Link);
            AddSourceIfNotDefault(deletedFiles, testQuestion.QuestionFormulaImageFilenameBeforeEdit, testQuestion.QuestionFormulaImageFileTypeBeforeEdit, FileType.Link);

            foreach (var testAnswer in testQuestion.TestAnswers)
            {
                AddSourceIfNotDefault(deletedFiles, testAnswer.AnswerImageFilenameBeforeEdit, testAnswer.AnswerImageFileTypeBeforeEdit, FileType.Link);
                AddSourceIfNotDefault(deletedFiles, testAnswer.AnswerFormulaImageFilenameBeforeEdit, testAnswer.AnswerFormulaImageFileTypeBeforeEdit, FileType.Link);

                AddSourceIfNotDefault(deletedFiles, testAnswer.VariantImageFilenameBeforeEdit, testAnswer.VariantImageFileTypeBeforeEdit, FileType.Link);
                AddSourceIfNotDefault(deletedFiles, testAnswer.VariantFormulaImageFilenameBeforeEdit, testAnswer.VariantFormulaImageFileTypeBeforeEdit, FileType.Link);
            }

            testQuestionsKeeper.BaseDirectory = GetBaseDirectoryForCurrentTestQuestion(testQuestion);
            testQuestionsKeeper.DeleteAllFilesInBaseDirectoryAndSubdirectories();

            Test.TestQuestions.Remove(testQuestion);
            Test.TestQuestions.Where(tq => tq.QuestionNumber > testQuestion.QuestionNumber).ForEach(tq => tq.QuestionNumber--);
        }

        [RelayCommand]
        public void AddTestAnswer(TestQuestionResponse selectedTestQuestion)
        {
            var answerToBeAdded = new TestAnswerResponse
            {
                Id = Guid.NewGuid(),
                Answer = string.Empty,
                Variant = string.Empty,
                Message = string.Empty,
                IsSelected = false,
                TestAnswerImageFilename = string.Empty,
                TestAnswerImageFilepath = defaultImagePath,
                TestAnswerImageFileType = FileType.None,
                AnswerImageFilenameBeforeEdit = null,
                TestAnswerFormulaImageFilename = string.Empty,
                TestAnswerFormulaImageFilepath = defaultImagePath,
                TestAnswerFormulaImageFileType = FileType.None,
                AnswerFormulaImageFilenameBeforeEdit = null,
                TestVariantImageFilename = string.Empty,
                TestVariantImageFilepath = defaultImagePath,
                TestVariantImageFileType = FileType.None,
                VariantImageFilenameBeforeEdit = null,
                TestVariantFormulaImageFilename = string.Empty,
                TestVariantFormulaImageFilepath = defaultImagePath,
                TestVariantFormulaImageFileType = FileType.None,
                VariantFormulaImageFilenameBeforeEdit = null,                
                SelectedPickerSpaceTestVariant = null,
                CursorPosition = 0,                
                AnswerNumber = selectedTestQuestion.TestAnswers.Count + 1,
                SelectedTestAnswer = null,
                TestQuestion = selectedTestQuestion,
                TestQuestionId = selectedTestQuestion.Id,
                IsCorrectAnswer = false,
                SpacesTestVariants = new ObservableCollection<SpaceTestVariantResponse>()                
            };

            selectedTestQuestion.TestAnswers.Add(answerToBeAdded);
        }       

        [RelayCommand]
        public async Task DeleteTestAnswer(TestAnswerResponse testAnswer)
        {
            bool deleteAction = await Application.Current.MainPage.DisplayAlert("Delete test answer", "Are you sure you want to delete this test answer? All test answer data test will be lost", "Yes", "Cancel");

            if (!deleteAction) return;

            AddSourceIfNotDefault(deletedFiles, testAnswer.AnswerImageFilenameBeforeEdit, testAnswer.AnswerImageFileTypeBeforeEdit, FileType.Link);
            AddSourceIfNotDefault(deletedFiles, testAnswer.AnswerFormulaImageFilenameBeforeEdit, testAnswer.AnswerFormulaImageFileTypeBeforeEdit, FileType.Link);

            AddSourceIfNotDefault(deletedFiles, testAnswer.VariantImageFilenameBeforeEdit, testAnswer.VariantImageFileTypeBeforeEdit, FileType.Link);
            AddSourceIfNotDefault(deletedFiles, testAnswer.VariantFormulaImageFilenameBeforeEdit, testAnswer.VariantFormulaImageFileTypeBeforeEdit, FileType.Link);

            testAnswersKeeper.BaseDirectory = GetBaseDirectoryForCurrentTestAnswer(testAnswer);
            testAnswersKeeper.DeleteAllFilesInBaseDirectory();

            var selectedTestQuestion = testAnswer.TestQuestion;
            selectedTestQuestion.TestAnswers.Remove(testAnswer);
            UpdateAnswerNumberWhenRemove(selectedTestQuestion, testAnswer);
        }

        public void UpdateAnswerNumberWhenRemove(TestQuestionResponse testQuestion, TestAnswerResponse testAnswer)
        {
            testQuestion.TestAnswers.Where(ta => ta.AnswerNumber > testAnswer.AnswerNumber)
                                    .ForEach(ta => ta.AnswerNumber--);
        }

        public void UpdateAnswerNumberWhenInsert(TestQuestionResponse testQuestion, TestAnswerResponse testAnswer)
        {
            testQuestion.TestAnswers.Where(ta => ta.AnswerNumber >= testAnswer.AnswerNumber)
                                    .ForEach(ta => ta.AnswerNumber++);
        }

        [RelayCommand]
        public async Task PickTestQuestionImage(TestQuestionResponse selectedTestQuestion)
        {
            var image = await PickImage();

            if (image == null) return;

            testQuestionsKeeper.BaseDirectory = GetBaseDirectoryForCurrentTestQuestion(selectedTestQuestion);
            selectedTestQuestion.TestQuestionImageFilepath = await testQuestionsKeeper.SaveFileToLocalStorage(image);
            selectedTestQuestion.TestQuestionImageFilename = Path.GetRelativePath(testQuestionsKeeper.LocalAppDataPath, selectedTestQuestion.TestQuestionImageFilepath);
            selectedTestQuestion.TestQuestionImageFileType = FileType.LocalFile;
        }
       
        public async Task SetTestQuestionFormulaImage(TestQuestionResponse selectedTestQuestion, string questionFormula)
        {            
            testQuestionsKeeper.BaseDirectory = GetBaseDirectoryForCurrentTestQuestion(selectedTestQuestion);
            selectedTestQuestion.TestQuestionFormulaImageFilepath = await testQuestionsKeeper.SaveFileToLocalStorage(Formula.GetFormulaStream(questionFormula, mathPainter));
            selectedTestQuestion.TestQuestionFormulaImageFilename = Path.GetRelativePath(testQuestionsKeeper.LocalAppDataPath, selectedTestQuestion.TestQuestionFormulaImageFilepath);
            selectedTestQuestion.TestQuestionFormulaImageFileType = FileType.LocalFile;
            selectedTestQuestion.TestQuestionLatexFormula = questionFormula;
        }

        [RelayCommand]
        public async Task PickTestAnswerImage(TestAnswerResponse selectedTestAnswer)
        {
            var image = await PickImage();

            if (image == null) return;

            testAnswersKeeper.BaseDirectory = GetBaseDirectoryForCurrentTestAnswer(selectedTestAnswer);
            selectedTestAnswer.TestAnswerImageFilepath = await testAnswersKeeper.SaveFileToLocalStorage(image);
            selectedTestAnswer.TestAnswerImageFilename = Path.GetRelativePath(testAnswersKeeper.LocalAppDataPath, selectedTestAnswer.TestAnswerImageFilepath);
            selectedTestAnswer.TestAnswerImageFileType = FileType.LocalFile;
        }
        
        public async Task SetTestAnswerFormulaImage(TestAnswerResponse selectedTestAnswer, string answerFormula)
        {            
            testAnswersKeeper.BaseDirectory = GetBaseDirectoryForCurrentTestAnswer(selectedTestAnswer);
            selectedTestAnswer.TestAnswerFormulaImageFilepath = await testAnswersKeeper.SaveFileToLocalStorage(Formula.GetFormulaStream(answerFormula, mathPainter));
            selectedTestAnswer.TestAnswerFormulaImageFilename = Path.GetRelativePath(testAnswersKeeper.LocalAppDataPath, selectedTestAnswer.TestAnswerFormulaImageFilepath);
            selectedTestAnswer.TestAnswerFormulaImageFileType = FileType.LocalFile;
            selectedTestAnswer.TestAnswerLatexFormula = answerFormula;
        }

        [RelayCommand]
        public async Task PickTestVariantImage(TestAnswerResponse selectedTestAnswer)
        {
            var image = await PickImage();

            if(image == null) return;

            testAnswersKeeper.BaseDirectory = GetBaseDirectoryForCurrentTestAnswer(selectedTestAnswer);
            selectedTestAnswer.TestVariantImageFilepath = await testAnswersKeeper.SaveFileToLocalStorage(image);
            selectedTestAnswer.TestVariantImageFilename = Path.GetRelativePath(testAnswersKeeper.LocalAppDataPath, selectedTestAnswer.TestVariantImageFilepath);
            selectedTestAnswer.TestVariantImageFileType = FileType.LocalFile;
        }
       
        public async Task SetTestVariantFormulaImage(TestAnswerResponse selectedTestAnswer, string variantFormula)
        {            
            testAnswersKeeper.BaseDirectory = GetBaseDirectoryForCurrentTestAnswer(selectedTestAnswer);
            selectedTestAnswer.TestVariantFormulaImageFilepath = await testAnswersKeeper.SaveFileToLocalStorage(Formula.GetFormulaStream(variantFormula, mathPainter));
            selectedTestAnswer.TestVariantFormulaImageFilename = Path.GetRelativePath(testAnswersKeeper.LocalAppDataPath, selectedTestAnswer.TestVariantFormulaImageFilepath);
            selectedTestAnswer.TestVariantFormulaImageFileType = FileType.LocalFile;
            selectedTestAnswer.TestVariantLatexFormula = variantFormula;
        }

        private async Task<FileResult> PickImage()
        {
            bool permissionsGranted = await PermissionsChecker.CheckForRequiredPermissions();
            if (!permissionsGranted) return null;

            var supportedImageExtension = new List<string> { ".jpg", ".png", ".jpeg" };
            var selectedImage = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Please select image",                
            });

            if (selectedImage == null) return null;

            string selectedImageExtension = Path.GetExtension(selectedImage.FileName);

            if(!supportedImageExtension.Contains(selectedImageExtension))
            {
                await Application.Current.MainPage.DisplayAlert("Wrong image extension", "You can only attach images with PNG, JPG, JPEG extensions. Please select another file to continue", "Got it");
                return null;
            }

            return selectedImage;
        }

        [RelayCommand]
        public void RemoveTestQuestionImage(TestQuestionResponse selectedTestQuestion)
        {
            selectedTestQuestion.TestQuestionImageFilename = string.Empty;
            selectedTestQuestion.TestQuestionImageFilepath = defaultImagePath;
            selectedTestQuestion.TestQuestionImageFileType = FileType.None;
        }
        
        public void RemoveTestQuestionFormulaImage(TestQuestionResponse selectedTestQuestion)
        {
            selectedTestQuestion.TestQuestionFormulaImageFilename = string.Empty;
            selectedTestQuestion.TestQuestionLatexFormula = string.Empty;
            selectedTestQuestion.TestQuestionFormulaImageFilepath = defaultImagePath;
            selectedTestQuestion.TestQuestionFormulaImageFileType = FileType.None;            
        }

        [RelayCommand]
        public void RemoveTestAnswerImage(TestAnswerResponse selectedTestAnswer)
        {
            selectedTestAnswer.TestAnswerImageFilename = string.Empty;
            selectedTestAnswer.TestAnswerImageFilepath = defaultImagePath;
            selectedTestAnswer.TestAnswerImageFileType = FileType.None;
        }
        
        public void RemoveTestAnswerFormulaImage(TestAnswerResponse selectedTestAnswer)
        {
            selectedTestAnswer.TestAnswerFormulaImageFilename = string.Empty;
            selectedTestAnswer.TestAnswerLatexFormula = string.Empty;
            selectedTestAnswer.TestAnswerFormulaImageFilepath = defaultImagePath;
            selectedTestAnswer.TestAnswerFormulaImageFileType = FileType.None;
        }

        [RelayCommand]
        public void RemoveTestVariantImage(TestAnswerResponse selectedTestAnswer)
        {
            selectedTestAnswer.TestVariantImageFilename = string.Empty;
            selectedTestAnswer.TestVariantImageFilepath = defaultImagePath;
            selectedTestAnswer.TestVariantImageFileType = FileType.None;
        }
       
        public void RemoveTestVariantFormulaImage(TestAnswerResponse selectedTestAnswer)
        {
            selectedTestAnswer.TestVariantFormulaImageFilename = string.Empty;
            selectedTestAnswer.TestVariantLatexFormula = string.Empty;
            selectedTestAnswer.TestVariantFormulaImageFilepath = defaultImagePath;
            selectedTestAnswer.TestVariantFormulaImageFileType = FileType.None;
        }

        private string GetSpaceNewSpaceTag(string text, ref int cursorPosition)
        {            
            string newSpaceTag = TextTags.SpaceTag;               

            if (string.IsNullOrEmpty(text))            
                return newSpaceTag += " ";

            if (cursorPosition == text.Length || !char.IsWhiteSpace(text, cursorPosition))
                newSpaceTag += " ";

            if (cursorPosition != 0 && !char.IsWhiteSpace(text, cursorPosition - 1))
            {
                newSpaceTag = " " + newSpaceTag;
                cursorPosition++;
            }          

            return newSpaceTag;
        }

        private int InsertSpaceTagWithUpdate(string text, int cursorPosition, TestQuestionResponse testQuestion, int indexOfNewElement, int oldTagLength)
        {
            int oldCursorPosition = cursorPosition;
            var testAnswer = testQuestion.TestAnswers.ElementAt(indexOfNewElement);

            string newSpaceTag = GetSpaceNewSpaceTag(text, ref cursorPosition);
            int cursorOffset = newSpaceTag.Length;

            testAnswer.CursorPosition = -1;
            UpdateTestQuestionCursorPositions(testQuestion, cursorOffset - oldTagLength, oldCursorPosition);

            testAnswer.CursorPosition = cursorPosition;
            testQuestion.ChangedByControls = true;                               

            return cursorOffset;
        }

        private int AddSpaceTestAnswer(TestQuestionResponse testQuestion, int cursorPosition, int indexOfNewElement, int oldTagLength)
        {         
            string editorText = testQuestion.SpacesEditorText;
            return InsertSpaceTagWithUpdate(editorText, cursorPosition, testQuestion, indexOfNewElement, oldTagLength);            
        }

        [RelayCommand]
        public void AddSpaceTestVariant(TestAnswerResponse testAnswer)
        {
            var spaceTestVariantToBeAdded = new SpaceTestVariantResponse
            {
                Id = Guid.NewGuid(),
                Variant = string.Empty,
                IsCorrectSpace = false,
                TestAnswerId = testAnswer.Id,
                TestAnswer = testAnswer               
            };

            testAnswer.SpacesTestVariants.Add(spaceTestVariantToBeAdded);
            testAnswer.TestQuestion.AllSpacesVariants.Add(spaceTestVariantToBeAdded);
        }

        private void SetSelectedTestAnswer(TestQuestionResponse testQuestion, TestAnswerResponse? testAnswer)
        {
            var existingTestAnswer = testQuestion.TestAnswers.FirstOrDefault(ta => ta == testAnswer);
            TestAnswerResponse newSelectedTestAnswer = existingTestAnswer;

            if (newSelectedTestAnswer == null & testQuestion.SelectedPickerTestAnswer != null)
            {
                var currentTestAnswerNumber = testQuestion.SelectedPickerTestAnswer.AnswerNumber;

                if (currentTestAnswerNumber != 1)
                {
                    PreviousSelectedTestAnswer(testQuestion);
                    return;
                }

                if (testQuestion.TestAnswers.Count != 0)
                {
                    newSelectedTestAnswer = testQuestion.TestAnswers.FirstOrDefault(ta => ta.AnswerNumber == currentTestAnswerNumber);
                }
            }           

            testQuestion.SelectedPickerTestAnswer = newSelectedTestAnswer;   
        }
        
        private void PreviousSelectedTestAnswer(TestQuestionResponse testQuestion)
        {
            int currentTestAnswerNumber = testQuestion.SelectedPickerTestAnswer.AnswerNumber;
            var newSelectedTestAnswer = testQuestion.TestAnswers.FirstOrDefault(ta => ta.AnswerNumber == currentTestAnswerNumber - 1);
            testQuestion.SelectedPickerTestAnswer = newSelectedTestAnswer;
        }        

        [RelayCommand]
        public void DeleteSpaceTestVariant(SpaceTestVariantResponse spaceTestVariant)
        {
            var selectedTestAnswer = spaceTestVariant.TestAnswer;            
            selectedTestAnswer.SpacesTestVariants.Remove(spaceTestVariant);
            selectedTestAnswer.TestQuestion.AllSpacesVariants.Remove(spaceTestVariant);

            if (spaceTestVariant.IsCorrectSpace)
            {
                var testQuestion = selectedTestAnswer.TestQuestion;                
                ChangeHtml(testQuestion);
            }
        }

        private int RemoveSpaceTagIfExist(TestQuestionResponse testQuestion, int cursorPosition, bool isDeletedTag)
        {
            int tagLength = GetSpaceTagLength(testQuestion, ref cursorPosition);
            testQuestion.ChangedByControls = true;

            if (!isDeletedTag)
            {
                UpdateTestQuestionCursorPositions(testQuestion, -tagLength, cursorPosition);
            }

            string textToRemove = testQuestion.SpacesEditorText.Substring(cursorPosition, tagLength);           

            return tagLength;
        }

        private int GetSpaceTagLength(TestQuestionResponse testQuestion, ref int cursorPosition)
        {            
            int oldCursorPosition = cursorPosition;
            bool spaceFound = false;

            var spaceTagOnPosition = GetSpaceTagMatches(testQuestion).FirstOrDefault(match => match.Index == oldCursorPosition);
            int tagLength = spaceTagOnPosition.Length;

            if (tagLength != testQuestion.SpacesEditorText.Length)
            {
                if (char.IsWhiteSpace(testQuestion.SpacesEditorText[tagLength + oldCursorPosition]))
                {
                    tagLength++;
                    spaceFound = true;
                }
            }

            if (!spaceFound & spaceTagOnPosition.Index != 0)
            {
                if (char.IsWhiteSpace(testQuestion.SpacesEditorText[oldCursorPosition - 1]))
                    cursorPosition--;
            }         
            
            return tagLength;
        }     

        private void AddSpaceTestAnswersRange(TestQuestionResponse selectedTestQuestion, List<int> cursorPositions)
        {             
            foreach(var cursorPosition in cursorPositions)
            {
                var answerToBeAdded = new TestAnswerResponse
                {
                    Id = Guid.NewGuid(),
                    Answer = string.Empty,
                    Variant = string.Empty,
                    Message = string.Empty,
                    IsSelected = false,
                    AnswerNumber = selectedTestQuestion.TestAnswers.Count + 1,
                    SelectedTestAnswer = null,
                    CursorPosition = cursorPosition,
                    TestQuestion = selectedTestQuestion,
                    TestQuestionId = selectedTestQuestion.Id,
                    IsCorrectAnswer = false,
                    SpacesTestVariants = new ObservableCollection<SpaceTestVariantResponse>()                    
                };

                selectedTestQuestion.TestAnswers.Add(answerToBeAdded);
            }
            
            ChangeHtml(selectedTestQuestion);
        }

        [RelayCommand]
        public void AddSpaceToText(Editor editor)
        {            
            var testQuestion = (TestQuestionResponse)editor.BindingContext;
            int cursorPosition = editor.CursorPosition;
            int oldTagLength = 0;

            AddTestAnswer(testQuestion);
            int indexOfNewElement = testQuestion.TestAnswers.Count - 1;

            int cursorOffset = AddSpaceTestAnswer(testQuestion, cursorPosition, indexOfNewElement, oldTagLength);
            editor.CursorPosition = cursorPosition + cursorOffset;
        }       

        [RelayCommand]
        public void RemoveSpaceFromText(TestAnswerResponse testAnswer)
        {      
            var testQuestion = testAnswer.TestQuestion;
            int cursorPosition = testAnswer.CursorPosition;                    
            
            RemoveSpaceTagIfExist(testQuestion, cursorPosition, false);            
            SetSelectedTestAnswer(testQuestion, testAnswer);        
        }

        [RelayCommand]
        public void ChangeHtml(TestQuestionResponse testQuestion)
        {
            testQuestion.SpacesEditorText ??= string.Empty;      
            var spaceMatches = GetSpaceTagMatches(testQuestion);

            int currentAnswersCount = testQuestion.TestAnswers?.Count ?? 0;
            int regexMatchesCount = spaceMatches.Count;

            if (currentAnswersCount == regexMatchesCount)
            {
                int index = 0;
                string html = null;

                html = Regex.Replace(testQuestion.SpacesEditorText, TextTags.SpaceRegexPattern, m =>
                {
                    if (testQuestion.CorrectSpacesValues.Any())
                        return testQuestion.CorrectSpacesValues[index++];

                    return GetSelectedSpaceTestValue(testQuestion, m.Index);
                });

                testQuestion.SpacesHtmlText = new HtmlWebViewSource
                {
                    Html = @$"<HTML><BODY><span>{html}</span></BODY></HTML>"
                };
            }
            else
            {                

                if (currentAnswersCount < regexMatchesCount)
                {
                    FillAddedTestAnswer(testQuestion, spaceMatches);
                }
                else
                {
                    FillDeletedTestAnswers(testQuestion, spaceMatches);
                    ChangeHtml(testQuestion);
                }                
            }
        }

        private void FillAddedTestAnswer(TestQuestionResponse testQuestion, MatchCollection spaceMatches)
        {
            var newElementsCursors = new List<int>();

            foreach (Match regexMatch in spaceMatches)
            {
                var existingElement = testQuestion.TestAnswers.FirstOrDefault(ta => ta.CursorPosition == regexMatch.Index);

                if (existingElement == null)
                    newElementsCursors.Add(regexMatch.Index);
            }

            AddSpaceTestAnswersRange(testQuestion, newElementsCursors);
        }

        private void FillDeletedTestAnswers(TestQuestionResponse testQuestion, MatchCollection spaceMatches)
        {
            for (int index = 0; index < testQuestion.TestAnswers.Count; index++)
            {
                var testAnswers = testQuestion.TestAnswers.ToList();
                var currentTestAnswer = testAnswers[index];

                var existingElement = spaceMatches.FirstOrDefault(match => match.Index == currentTestAnswer.CursorPosition);

                if (existingElement == null)
                {
                    var deletedTestAnswer = new KeyValuePair<int, TestAnswerResponse>(index, currentTestAnswer);

                    testQuestion.DeletedTestAnswers.Insert(0, deletedTestAnswer);
                    DeleteTestAnswer(currentTestAnswer);
                }
            }
        }

        private string GetSelectedSpaceTestValue(TestQuestionResponse testQuestion, int cursorPosition)
        {
            var currentTestAnswer = testQuestion.TestAnswers.FirstOrDefault(ta => ta.CursorPosition == cursorPosition);
            int indexOfElement = currentTestAnswer.AnswerNumber;
            string spaceVariant = null;            

            if (testQuestion.AllSpacesVariants.Any())
                spaceVariant = currentTestAnswer.SpacesTestVariants.FirstOrDefault(stv => stv.IsCorrectSpace)?.Variant;                       

            if (!string.IsNullOrEmpty(spaceVariant))
                return $"<button style=\"margin: 3px\">{indexOfElement}.{spaceVariant}</button>";            

            return $"<button style=\"margin: 3px\">{indexOfElement}.Space</button>";
        }

        [RelayCommand]
        public void ReturnLastDeletedTestAnswer(Editor editor)
        {
            var testQuestion = (TestQuestionResponse)editor.BindingContext;
            var lastDeletedTestAnswer = testQuestion.DeletedTestAnswers.FirstOrDefault();
            int lastDeletedCursorPosition = lastDeletedTestAnswer.Value.CursorPosition;

            int indexOfNewElement = lastDeletedTestAnswer.Key;
            var testAnswer = lastDeletedTestAnswer.Value;
            
            int oldTagLength = RemoveSpaceTagIfExist(testQuestion, lastDeletedCursorPosition, true);
            testQuestion.DeletedTestAnswers.Remove(lastDeletedTestAnswer);
            
            UpdateAnswerNumberWhenInsert(testQuestion, testAnswer);
            testQuestion.TestAnswers.Insert(indexOfNewElement, testAnswer);

            int cursorOffset = AddSpaceTestAnswer(testQuestion, lastDeletedCursorPosition, indexOfNewElement, oldTagLength);         
            editor.CursorPosition = lastDeletedCursorPosition + cursorOffset;

            SetSelectedTestAnswer(testQuestion, testAnswer);
        }
       

        public static MatchCollection GetSpaceTagMatches(TestQuestionResponse testQuestion)
        {            
            Regex spaceTagRegex = new (TextTags.SpaceRegexPattern);
            return spaceTagRegex.Matches(testQuestion.SpacesEditorText);
        }
        
        public void UpdateTestQuestionCursorPositions(TestQuestionResponse testQuestion, int cursorOffset, int currentCursorPosition)
        {
            testQuestion.TestAnswers?                        
                        .Where(ta => ta.CursorPosition >= currentCursorPosition)
                        .ForEach(ta => ta.CursorPosition = cursorOffset + ta.CursorPosition);

            testQuestion.DeletedTestAnswers?
                        .Where(ta => ta.Value.CursorPosition >= currentCursorPosition)
                        .ForEach(ta => ta.Value.CursorPosition = cursorOffset + ta.Value.CursorPosition);          
        }      

        [RelayCommand]
        public async Task NavigateToTestPreview()
        {
            await loadingService.PerformLoading("Loading test preview...", async () =>
            {
                await Shell.Current.GoToAsync($"{nameof(CreatorTestView)}", true, new Dictionary<string, object>
                {
                    {"Test", Test}
                });
            });            
        }

        [RelayCommand]
        public async Task PublishTest()
        {
            var filesList = GetTestFiles(Test);

            await loadingService.PerformLoadingWithFilesUpload("Publishing test...", "Back to main", filesList, async (filesToUpload, loadedFiles) =>
            {
                var newFiles = new List<AttachmentDTOResponse>();

                if (filesList.Count > 0)
                {
                    newFiles = await fileRequestService.UploadFiles(filesList, testKeeper.LocalAppDataPath);                    
                }

                FilesAttachmentsService.AttachTestFiles(Test, newFiles.Union(loadedFiles).ToList());
                testFilesKeeper.DeleteAllFilesInBaseDirectoryAndSubdirectories();                         

                if (IsNewTest)
                {                   
                    await dataRequestService.CreateData(Test);                    
                }
                else
                {
                    var unusedFiles = GetUnusedAttachments(Test);

                    if (unusedFiles.Count > 0)
                    {
                        await fileRequestService.DeleteFiles(unusedFiles);
                    }

                    Test.TestQuestions.ForEach(tq =>
                    {
                        tq.Test = null;
                        tq.TestAnswers.ForEach(ta => ta.TestQuestion = null);
                    });

                    await dataRequestService.UpdateData(Test.Id, Test);
                }
            });           
        } 
        
        private List<string> GetUnusedAttachments(TestResponse test)
        {
            var unusedFilesList = new List<string>(deletedFiles);

            foreach (var testQuestion in test.TestQuestions)
            {                    
                AddSourceIfUnused(unusedFilesList, testQuestion.QuestionImageFilenameBeforeEdit, testQuestion.TestQuestionImageFilename, testQuestion.QuestionImageFileTypeBeforeEdit);
                AddSourceIfUnused(unusedFilesList, testQuestion.QuestionFormulaImageFilenameBeforeEdit, testQuestion.TestQuestionFormulaImageFilename, testQuestion.QuestionFormulaImageFileTypeBeforeEdit);

                foreach (var testAnswer in testQuestion.TestAnswers)
                {                    
                    AddSourceIfUnused(unusedFilesList, testAnswer.AnswerImageFilenameBeforeEdit, testAnswer.TestAnswerImageFilename, testAnswer.AnswerImageFileTypeBeforeEdit);
                    AddSourceIfUnused(unusedFilesList, testAnswer.AnswerFormulaImageFilenameBeforeEdit, testAnswer.TestAnswerFormulaImageFilename, testAnswer.AnswerFormulaImageFileTypeBeforeEdit);

                    AddSourceIfUnused(unusedFilesList, testAnswer.VariantImageFilenameBeforeEdit, testAnswer.TestVariantImageFilename, testAnswer.VariantImageFileTypeBeforeEdit);
                    AddSourceIfUnused(unusedFilesList, testAnswer.VariantFormulaImageFilenameBeforeEdit, testAnswer.TestVariantFormulaImageFilename, testAnswer.VariantFormulaImageFileTypeBeforeEdit);
                }
            }
            
            return unusedFilesList;
        }

        private void AddSourceIfUnused(List<string> unusedFiles, string sourceBeforeEdit, string source, FileType fileTypeBeforeEdit)
        {
            if (!string.IsNullOrEmpty(sourceBeforeEdit) && fileTypeBeforeEdit == FileType.Link && source != sourceBeforeEdit)
            {
                unusedFiles.Add(sourceBeforeEdit);
            }
        }

        private void AddSourceIfNotDefault(List<string> files, string path, FileType fileType, FileType comparedFileType)
        {
            if (path != defaultImagePath && fileType == comparedFileType)
            {
                files.Add(path);
            }
        }
       
        private List<string> GetTestFiles(TestResponse test)
        {            
            var result = new List<string>();          

            foreach(var testQuestion in test.TestQuestions)
            {
                AddSourceIfNotDefault(result, testQuestion.TestQuestionImageFilepath, testQuestion.TestQuestionImageFileType, FileType.LocalFile);
                AddSourceIfNotDefault(result, testQuestion.TestQuestionFormulaImageFilepath, testQuestion.TestQuestionFormulaImageFileType, FileType.LocalFile);               

                foreach(var testAnswer in testQuestion.TestAnswers)
                {
                    AddSourceIfNotDefault(result, testAnswer.TestAnswerImageFilepath, testAnswer.TestAnswerImageFileType, FileType.LocalFile);
                    AddSourceIfNotDefault(result, testAnswer.TestAnswerFormulaImageFilepath, testAnswer.TestAnswerFormulaImageFileType, FileType.LocalFile);

                    AddSourceIfNotDefault(result, testAnswer.TestVariantImageFilepath, testAnswer.TestVariantImageFileType, FileType.LocalFile);
                    AddSourceIfNotDefault(result, testAnswer.TestVariantFormulaImageFilepath, testAnswer.TestVariantFormulaImageFileType, FileType.LocalFile);                   
                }
            }

            return result;
        }             
    }
}
