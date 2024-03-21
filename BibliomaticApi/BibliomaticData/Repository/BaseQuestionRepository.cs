using BibliomaticData.AppContext;
using BibliomaticData.Models;
using BibliomaticData.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BibliomaticData.Repository
{
    public class BaseQuestionRepository : IRepository<BaseQuestion, BaseQuestionDTO>, ISocialRepository<QuestionLike, QuestionDislike, QuestionComment>
    {       
        private readonly BibliomaticAppContext appContext;

        public BaseQuestionRepository(BibliomaticAppContext appContext)
        {
            this.appContext = appContext;
        }

        private async Task SetBaseQuestionAuthor(BaseQuestion baseQuestion)
        {
            baseQuestion.Author = await GraphApi.GetAuthor(baseQuestion.UserId);
            baseQuestion.Question.Author = baseQuestion.Author;

            foreach(var answer in baseQuestion.Answers)
            {
                answer.Author = await GraphApi.GetAuthor(answer.UserId);
            }
        }
      
        private async Task SetQuestionCommentsAuthor(IEnumerable<QuestionComment> questionComments)
        {
            foreach (var questionComment in questionComments)
            {
                questionComment.Author = await GraphApi.GetAuthor(questionComment.UserId);
            }
        }

        private async Task SetAnswerCommentsAuthor(IEnumerable<AnswerComment> answerComments)
        {
            foreach (var answerComment in answerComments)
            {
                answerComment.Author = await GraphApi.GetAuthor(answerComment.UserId);
            }
        }

        public async Task<IEnumerable<BaseQuestionDTO>> GetAll()
        {
            var baseQuestions = await appContext.BaseQuestions.Select(bq => new BaseQuestionDTO
            {
                Id = bq.Id,
                Header = bq.Header,
                Description = bq.Description,
                IsSolved = bq.IsSolved,
                UserId = bq.UserId,
                CreatedAt = bq.CreatedAt,
                UpdatedAt = bq.UpdatedAt,                      
            }).ToListAsync();

            foreach(var baseQuestion in baseQuestions)
            {
                baseQuestion.Author = await GraphApi.GetAuthor(baseQuestion.UserId);
            }

            return baseQuestions;
        }

        public async Task Create(BaseQuestion question)
        {
            await appContext.BaseQuestions.AddAsync(question);
            await appContext.SaveChangesAsync();    
        }
       
        public async Task Delete(BaseQuestion question)
        {
            appContext.BaseQuestions.Remove(question);
            await appContext.SaveChangesAsync();
        }

        public async Task<BaseQuestion> GetById(Guid id)
        {
            return await appContext.BaseQuestions.AsNoTracking()                                                 
                                                 .Include(bq => bq.Question).ThenInclude(q => q.QuestionComments)                                                
                                                 .Include(bq => bq.Answers).ThenInclude(a => a.AnswerComments)
                                                 .FirstOrDefaultAsync(bq => bq.Id == id);
        }
        private void AddSourceToFilesIfNotDefault(List<string> files, string source)
        {
            if (!string.IsNullOrEmpty(source))
            {
                files.Add(source);
            }
        }

        public async Task<IEnumerable<string>> GetAllFiles(Guid id)
        {
            var baseQuestionFiles = new List<string>();

            var baseQuestion = await appContext.BaseQuestions.IgnoreAutoIncludes()
                                                             .Include(bq => bq.Question).ThenInclude(q => q.Images)
                                                             .Include(bq => bq.Question).ThenInclude(q => q.Formulas)
                                                             .Include(bq => bq.Answers).ThenInclude(a => a.Images)
                                                             .Include(bq => bq.Answers).ThenInclude(a => a.Formulas)
                                                             .FirstOrDefaultAsync(bq => bq.Id == id);

            if (baseQuestion == null) return null;

            AddSourceToFilesIfNotDefault(baseQuestionFiles, baseQuestion.Question.QuestionHtmlDocument);

            foreach (var image in baseQuestion.Question.Images)
            {
                AddSourceToFilesIfNotDefault(baseQuestionFiles, image.ImageFilename);                
            }

            foreach (var formula in baseQuestion.Question.Formulas)
            {
                AddSourceToFilesIfNotDefault(baseQuestionFiles, formula.FormulaFilename);
            }

            foreach(var answer in baseQuestion.Answers)
            {
                AddSourceToFilesIfNotDefault(baseQuestionFiles, answer.AnswerHtmlDocument);

                foreach (var image in answer.Images)
                {
                    AddSourceToFilesIfNotDefault(baseQuestionFiles, image.ImageFilename);
                }

                foreach (var formula in answer.Formulas)
                {
                    AddSourceToFilesIfNotDefault(baseQuestionFiles, formula.FormulaFilename);
                }
            }

            return baseQuestionFiles;
        }

        public async Task<BaseQuestion> GetSummarizedById(Guid id)
        {            
            var baseQuestion = await appContext.BaseQuestions.AsNoTracking().FirstOrDefaultAsync(bq => bq.Id == id);

            if (baseQuestion == null) return null;

            var questionId = baseQuestion.Question.Id;

            baseQuestion.Answers = await appContext.Answers.Where(a => a.BaseQuestionId == id).OrderByDescending(a => a.CreatedAt).Skip(0).Take(5).ToListAsync();
            baseQuestion.AnswersCount = await appContext.Answers.Where(a => a.BaseQuestionId == id).CountAsync();

            var questionComments = await appContext.QuestionComments.Where(qc => qc.QuestionId == questionId).OrderByDescending(qc => qc.CreatedAt).Skip(0).Take(5).ToListAsync();
            var questionCommentsCount = await appContext.QuestionComments.Where(qc => qc.QuestionId == questionId).CountAsync();

            await SetBaseQuestionAuthor(baseQuestion);
            await SetQuestionCommentsAuthor(questionComments);

            baseQuestion.Question.QuestionComments = questionComments;
            baseQuestion.Question.QuestionCommentsCount = questionCommentsCount;

            foreach (var answer in baseQuestion.Answers)
            {
                var answerComments = await appContext.AnswerComments.Where(ac => ac.AnswerId == answer.Id).OrderByDescending(ac => ac.CreatedAt).Skip(0).Take(5).ToListAsync();
                var answerCommentsCount = await appContext.AnswerComments.Where(ac => ac.AnswerId == answer.Id).CountAsync();

                await SetAnswerCommentsAuthor(answerComments);

                answer.BaseQuestion = baseQuestion;
                answer.AnswerComments = answerComments;
                answer.AnswerCommentsCount = answerCommentsCount;               
            }

            return baseQuestion;
        }

        public async Task<BaseQuestion> GetTrackedById(Guid id)
        {
            return await appContext.BaseQuestions.Include(bq => bq.Question).ThenInclude(q => q.QuestionComments)
                                                 .Include(bq => bq.Answers).ThenInclude(a => a.AnswerComments)
                                                 .FirstOrDefaultAsync(bq => bq.Id == id);
        }
       

        public async Task Update(BaseQuestion newQuestion)
        {
            var oldQuestion = appContext.BaseQuestions.IgnoreAutoIncludes()
                                                      .Include(bq => bq.Question)
                                                      .ThenInclude(q => q.Hyperlinks)
                                                      .Include(bq => bq.Question)
                                                      .ThenInclude(q => q.Images)
                                                      .Include(bq => bq.Question)
                                                      .ThenInclude(q => q.Formulas)                                                      
                                                      .FirstOrDefault(bq => bq.Id == newQuestion.Id);

            if (oldQuestion != null)
            {
                appContext.Entry(oldQuestion).CurrentValues.SetValues(newQuestion);
                appContext.Entry(oldQuestion.Question).CurrentValues.SetValues(newQuestion.Question);

                UpdateQuestionHyperlinks(oldQuestion.Question, newQuestion.Question);
                UpdateQuestionImages(oldQuestion.Question, newQuestion.Question);
                UpdateQuestionFormulas(oldQuestion.Question, newQuestion.Question);

                await appContext.SaveChangesAsync();
            }
        }
       
        private void UpdateQuestionHyperlinks(Question oldQuestion, Question newQuestion)
        {
            foreach (var hyperlink in oldQuestion.Hyperlinks)
            {
                var existingHyperlink = newQuestion.Hyperlinks.FirstOrDefault(qh => qh.Id == hyperlink.Id);

                if (existingHyperlink == null)
                {
                    appContext.Remove(hyperlink);
                }                
            }

            foreach (var hyperlink in newQuestion.Hyperlinks)
            {
                if (oldQuestion.Hyperlinks.All(qh => qh.Id != hyperlink.Id))
                {
                    appContext.Add(hyperlink);                    
                }
            }
        }

        private void UpdateQuestionImages(Question oldQuestion, Question newQuestion)
        {
            foreach (var image in oldQuestion.Images)
            {
                var existingImage = newQuestion.Images.FirstOrDefault(qi => qi.Id == image.Id);

                if (existingImage == null)
                {
                    appContext.Remove(image);
                }               
            }

            foreach (var image in newQuestion.Images)
            {
                if (oldQuestion.Images.All(qi => qi.Id != image.Id))
                {
                    appContext.Add(image);                    
                }
            }
        }

        private void UpdateQuestionFormulas(Question oldQuestion, Question newQuestion)
        {
            foreach (var formula in oldQuestion.Formulas)
            {
                var existingFormula = newQuestion.Formulas.FirstOrDefault(qf => qf.Id == formula.Id);

                if (existingFormula == null)
                {
                    appContext.Remove(formula);
                }               
            }

            foreach (var formula in newQuestion.Formulas)
            {
                if (oldQuestion.Formulas.All(qf => qf.Id != formula.Id))
                {
                    appContext.Add(formula);
                }
            }
        }

        public async Task SaveChanges()
        {
            await appContext.SaveChangesAsync();
        }

        public async Task AddLike(QuestionLike like)
        {
            appContext.QuestionLikes.Add(like);
            await appContext.SaveChangesAsync();
        }

        public async Task RemoveLike(QuestionLike like)
        {
            appContext.QuestionLikes.Remove(like);
            await appContext.SaveChangesAsync();
        }
        public async Task AddDislike(QuestionDislike dislike)
        {
            appContext.QuestionDislikes.Add(dislike);
            await appContext.SaveChangesAsync();
        }

        public async Task RemoveDislike(QuestionDislike dislike)
        {
            appContext.QuestionDislikes.Remove(dislike);
            await appContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<QuestionComment>> GetAllComments(Guid id)
        {
            var questionComments = await appContext.QuestionComments.Where(qc => qc.QuestionId == id).ToListAsync();
            await SetQuestionCommentsAuthor(questionComments);
            return questionComments;
        }

        public async Task AddComment(QuestionComment comment)
        {
            appContext.QuestionComments.Add(comment);
            await appContext.SaveChangesAsync();
        }

        public async Task RemoveComment(QuestionComment comment)
        {
            appContext.QuestionComments.Remove(comment);
            await appContext.SaveChangesAsync();
        }         
    }
}
