using BibliomaticData.AppContext;
using BibliomaticData.Models;
using BibliomaticData.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BibliomaticData.Repository
{
    public class AnswerRepository : IRelatedRepository<Answer>, ISocialRepository<AnswerLike, AnswerDislike, AnswerComment>
    {
        private readonly BibliomaticAppContext appContext;

        public AnswerRepository(BibliomaticAppContext appContext)
        {
            this.appContext = appContext;
        }

        private async Task SetAnswerCommentsAuthor(IEnumerable<AnswerComment> answerComments)
        {
            foreach (var answerComment in answerComments)
            {
                answerComment.Author = await GraphApi.GetAuthor(answerComment.UserId);
            }
        }

        public async Task<IEnumerable<Answer>> GetAllRelated(Guid id)
        {
            var answers = await appContext.Answers.Where(a => a.BaseQuestionId == id).ToListAsync();

            foreach (var answer in answers)
            {
                answer.Author = await GraphApi.GetAuthor(answer.UserId);

                var answerComments = await appContext.AnswerComments.Where(ac => ac.AnswerId == answer.Id).OrderByDescending(ac => ac.CreatedAt).Skip(0).Take(5).ToListAsync();
                var answerCommentsCount = await appContext.AnswerComments.Where(ac => ac.AnswerId == answer.Id).CountAsync();

                await SetAnswerCommentsAuthor(answerComments);

                answer.AnswerComments = answerComments;
                answer.AnswerCommentsCount = answerCommentsCount;
            }

            return answers;
        }
       
        public async Task CreateRelated(Answer answer)
        {
            appContext.Add(answer);
            await appContext.SaveChangesAsync();
        }

        public async Task DeleteRelated(Answer answer)
        {
            appContext.Remove(answer);
            await appContext.SaveChangesAsync();
        }

        public async Task<Answer> GetRelatedById(Guid id, Guid relatedId)
        {
            var baseQuestion = await appContext.BaseQuestions.Include(bq => bq.Answers).ThenInclude(a => a.AnswerComments).FirstOrDefaultAsync(bq => bq.Id == id);

            if (baseQuestion == null) return null;

            return baseQuestion.Answers.FirstOrDefault(a => a.Id == relatedId);
        }
        
        public async Task UpdateRelated(Answer newAnswer)
        {
            var oldAnswer = appContext.Answers.IgnoreAutoIncludes()
                                              .Include(a => a.Hyperlinks)
                                              .Include(a => a.Images)
                                              .Include(a => a.Formulas)
                                              .FirstOrDefault(a => a.Id == newAnswer.Id);

            if(oldAnswer != null)
            {
                appContext.Entry(oldAnswer).CurrentValues.SetValues(newAnswer);

                UpdateAnswerHyperlinks(oldAnswer, newAnswer);
                UpdateAnswerImages(oldAnswer, newAnswer);
                UpdateAnswerFormulas(oldAnswer, newAnswer);

                await appContext.SaveChangesAsync();
            }
        }
        
        private void UpdateAnswerHyperlinks(Answer oldAnswer, Answer newAnswer)
        {
            foreach (var hyperlink in oldAnswer.Hyperlinks)
            {
                var existingHyperlink = newAnswer.Hyperlinks.FirstOrDefault(ah => ah.Id == hyperlink.Id);

                if (existingHyperlink == null)
                {
                    appContext.Remove(hyperlink);
                }               
            }

            foreach (var hyperlink in newAnswer.Hyperlinks)
            {
                if (oldAnswer.Hyperlinks.All(ah => ah.Id != hyperlink.Id))
                {
                    appContext.Add(hyperlink);
                }
            }
        }
        private void UpdateAnswerImages(Answer oldAnswer, Answer newAnswer)
        {
            foreach (var image in oldAnswer.Images.ToList())
            {
                var existingImage = newAnswer.Images.FirstOrDefault(ai => ai.Id == image.Id);

                if (existingImage == null)
                {
                    appContext.Remove(image);
                }                
            }

            foreach (var image in newAnswer.Images)
            {
                if (oldAnswer.Images.All(ai => ai.Id != image.Id))
                {
                    appContext.Add(image);
                }
            }
        }

        private void UpdateAnswerFormulas(Answer oldAnswer, Answer newAnswer)
        {
            foreach (var formula in oldAnswer.Formulas.ToList())
            {
                var existingFormula = newAnswer.Formulas.FirstOrDefault(af => af.Id == formula.Id);

                if (existingFormula == null)
                {
                    appContext.Remove(formula);
                }                
            }

            foreach (var formula in newAnswer.Formulas)
            {
                if (oldAnswer.Formulas.All(af => af.Id != formula.Id))
                {
                    appContext.Add(formula);
                }
            }
        }

        public async Task SaveChanges()
        {
            await appContext.SaveChangesAsync();
        }

        public async Task AddLike(AnswerLike like)
        {
            appContext.AnswerLikes.Add(like);
            await appContext.SaveChangesAsync();
        }

        public async Task RemoveLike(AnswerLike like)
        {
            appContext.AnswerLikes.Remove(like);
            await appContext.SaveChangesAsync();
        }

        public async Task AddDislike(AnswerDislike dislike)
        {
            appContext.AnswerDislikes.Add(dislike);
            await appContext.SaveChangesAsync();
        }

        public async Task RemoveDislike(AnswerDislike dislike)
        {
            appContext.AnswerDislikes.Remove(dislike);
            await appContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AnswerComment>> GetAllComments(Guid id)
        {
            var answerComments = await appContext.AnswerComments.Where(ac => ac.AnswerId == id).ToListAsync();
            await SetAnswerCommentsAuthor(answerComments);

            return answerComments;
        }

        public async Task AddComment(AnswerComment comment)
        {
            appContext.AnswerComments.Add(comment);
            await appContext.SaveChangesAsync();
        }

        public async Task RemoveComment(AnswerComment comment)
        {
            appContext.AnswerComments.Remove(comment);
            await appContext.SaveChangesAsync();
        }        
    }
}
