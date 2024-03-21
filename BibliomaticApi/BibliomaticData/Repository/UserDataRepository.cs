using BibliomaticData.AppContext;
using BibliomaticData.Models;
using BibliomaticData.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BibliomaticData.Repository
{
    public class UserDataRepository : IUserDataRepository
    {
        private readonly BibliomaticAppContext appContext;

        public UserDataRepository(BibliomaticAppContext appContext)
        {
            this.appContext = appContext;
        }        

        public async Task<UserDataDTO> GetUserDataById(Guid id)
        {
            var userTests = await appContext.Tests.AsNoTracking()
                                                  .Where(t => t.UserId == id)
                                                  .Select(t => new TestDTO
                                                  {
                                                      Id = t.Id,
                                                      Name = t.Name,
                                                      Description = t.Description,
                                                      CreatedAt = t.CreatedAt,
                                                      UpdatedAt = t.UpdatedAt,                                                      
                                                      UserId = t.UserId                                                      
                                                  })
                                                  .ToListAsync();

            foreach (var userTest in userTests)
            {
                userTest.Author = await GraphApi.GetAuthor(userTest.UserId);
            }

            var userQuestions = await appContext.BaseQuestions.AsNoTracking()
                                                              .Where(q => q.UserId == id)
                                                              .Select(bq => new BaseQuestionDTO
                                                              {
                                                                  Id = bq.Id,
                                                                  Header = bq.Header,
                                                                  Description = bq.Description,
                                                                  IsSolved = bq.IsSolved,
                                                                  UserId = bq.UserId,
                                                                  CreatedAt = bq.CreatedAt,
                                                                  UpdatedAt = bq.UpdatedAt,
                                                              })
                                                              .ToListAsync();

            foreach (var userQuestion in userQuestions)
            {
                userQuestion.Author = await GraphApi.GetAuthor(userQuestion.UserId);
            }

            var userArticles = await appContext.Articles.AsNoTracking()
                                                        .Where(a => a.UserId == id)
                                                        .Select(a => new ArticleDTO
                                                        {
                                                            Id = a.Id,
                                                            Title = a.Title,
                                                            UserId = a.UserId,
                                                            CreatedAt = a.CreatedAt,
                                                            UpdatedAt = a.UpdatedAt
                                                        })
                                                        .ToListAsync();

            foreach (var userArticle in userArticles)
            {
                userArticle.Author = await GraphApi.GetAuthor(userArticle.UserId);
            }
          
            return new UserDataDTO { Id = id, Articles = userArticles, Questions = userQuestions, Tests = userTests };
        }

        public async Task<UserData> GetTrackedUserDataById(Guid id)
        {
            var userTests = await appContext.Tests.Where(t => t.UserId == id)
                                                  .ToListAsync();

            var userQuestions = await appContext.BaseQuestions.Where(q => q.UserId == id)
                                                              .ToListAsync();

            var userArticles = await appContext.Articles.Where(a => a.UserId == id)
                                                        .ToListAsync();
            

            return new UserData { Id = id, Articles = userArticles, Questions = userQuestions, Tests = userTests };
        }     
        
        public async Task<Author> UpdateUserData(Author author)
        {
            return await GraphApi.ChangeUser(author);
        }
    }
}
