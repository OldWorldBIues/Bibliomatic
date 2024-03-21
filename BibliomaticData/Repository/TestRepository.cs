using BibliomaticData.AppContext;
using BibliomaticData.Models;
using BibliomaticData.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BibliomaticData.Repository
{
    public class TestRepository : ITestRepository, ISocialRepository<TestLike, TestDislike, TestComment>, IUserScoresRepository<UserScore>
    {
        private readonly BibliomaticAppContext appContext;

        public TestRepository(BibliomaticAppContext appContext)
        {
            this.appContext = appContext;
        }

        private async Task SetTestAuthor(Test test)
        {
            test.Author = await GraphApi.GetAuthor(test.UserId);
        }

        private async Task SetTestCommentsAuthor(IEnumerable<TestComment> testComments)
        {
            foreach (var testComment in testComments)
            {
                testComment.Author = await GraphApi.GetAuthor(testComment.UserId);
            }
        }
        private async Task SetUserScoresAuthor(IEnumerable<UserScore> userScores)
        {
            foreach (var userScore in userScores)
            {
                userScore.User = await GraphApi.GetAuthor(userScore.UserId);
            }
        }

        public async Task<IEnumerable<TestDTO>> GetAll()
        {
            var tests = await appContext.Tests.Select(t => new TestDTO
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                UserId = t.UserId,               
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).ToListAsync();

            foreach(var test in tests)
            {
                test.Author = await GraphApi.GetAuthor(test.UserId);
            }

            return tests;
        }

        public async Task<IEnumerable<TestDTO>> GetAllByTestResult(Guid userId, bool passed)
        {
            var tests = await appContext.Tests.Include(t => t.UserScores)
                                              .Where(t => passed == (t.UserScores.FirstOrDefault(us => us.UserId == userId) != null))
                                              .Select(t => new TestDTO
                                              {
                                                  Id = t.Id,
                                                  Name = t.Name,
                                                  Description = t.Description,
                                                  UserId = t.UserId,
                                                  CreatedAt = t.CreatedAt,
                                                  UpdatedAt = t.UpdatedAt
                                              }).ToListAsync();


            foreach (var test in tests)
            {
                test.Author = await GraphApi.GetAuthor(test.UserId);
            }

            return tests;
        }

        public async Task<Test> GetById(Guid id)
        {
            return await appContext.Tests.Include(t => t.TestComments)
                                         .Include(t => t.UserScores)
                                         .AsNoTracking()
                                         .FirstOrDefaultAsync(q => q.Id == id);
        }

        private void AddSourceToFilesIfNotDefault(List<string> files, string source)
        {
            if(!string.IsNullOrEmpty(source))
            {
                files.Add(source);
            }
        }
       
        public async Task<IEnumerable<string>> GetAllFiles(Guid id)
        {
            var testFiles = new List<string>();

            var test = await appContext.Tests.IgnoreAutoIncludes()
                                             .Include(t => t.TestQuestions).ThenInclude(tq => tq.TestAnswers)
                                             .FirstOrDefaultAsync(t => t.Id == id);

            if (test == null) return null;

            foreach(var testQuestion in test.TestQuestions)
            {
                AddSourceToFilesIfNotDefault(testFiles, testQuestion.TestQuestionFilename);
                AddSourceToFilesIfNotDefault(testFiles, testQuestion.TestQuestionFormulaFilename);

                foreach(var testAnswer in testQuestion.TestAnswers)
                {
                    AddSourceToFilesIfNotDefault(testFiles, testAnswer.TestAnswerFilename);
                    AddSourceToFilesIfNotDefault(testFiles, testAnswer.TestAnswerFormulaFilename);

                    AddSourceToFilesIfNotDefault(testFiles, testAnswer.TestVariantFilename);
                    AddSourceToFilesIfNotDefault(testFiles, testAnswer.TestVariantFormulaFilename);
                }
            }
            
            return testFiles;
        }

        public async Task<Test> GetSummarizedById(Guid id)
        {
            var test = await appContext.Tests.AsNoTracking()                                                                                     
                                             .FirstOrDefaultAsync(a => a.Id == id);

            if (test == null) return null;

            var testComments = await appContext.TestComments.Where(tc => tc.TestId == id).OrderByDescending(tc => tc.CreatedAt).Skip(0).Take(5).ToListAsync();
            var testUserScores = await appContext.UserScores.Where(us => us.TestId == id).OrderByDescending(us => us.TestEndDate).Skip(0).Take(5).ToListAsync();

            int testCommentsCount = await appContext.TestComments.Where(tc => tc.TestId == id).CountAsync();            
            int testUserScoresCount = await appContext.UserScores.Where(us => us.TestId == id).CountAsync();

            await SetTestAuthor(test);
            await SetTestCommentsAuthor(testComments);
            await SetUserScoresAuthor(testUserScores);
            
            test.TestComments = testComments;
            test.UserScores = testUserScores;

            test.TestCommentsCount = testCommentsCount;
            test.TestUserScoresCount = testUserScoresCount;

            return test;
        }

        public async Task<Test> GetSummarizedById(Guid id, Guid userId)
        {
            var test = await appContext.Tests.AsNoTracking()
                                             .FirstOrDefaultAsync(a => a.Id == id);

            var testComments = await appContext.TestComments.Where(tc => tc.TestId == id).OrderByDescending(tc => tc.CreatedAt).Skip(0).Take(5).ToListAsync();
            var testUserScores = await appContext.UserScores.Where(us => us.TestId == id & us.UserId == userId).OrderByDescending(us => us.TestEndDate).Skip(0).Take(5).ToListAsync();

            int testCommentsCount = await appContext.TestComments.Where(tc => tc.TestId == id).CountAsync();
            int testUserScoresCount = await appContext.UserScores.Where(us => us.TestId == id & us.UserId == userId).CountAsync();

            await SetTestAuthor(test);
            await SetTestCommentsAuthor(testComments);
            await SetUserScoresAuthor(testUserScores);

            test.TestComments = testComments;
            test.UserScores = testUserScores;

            test.TestCommentsCount = testCommentsCount;
            test.TestUserScoresCount = testUserScoresCount;

            return test;
        }

        public async Task<Test> GetTrackedById(Guid id)
        {
            return await appContext.Tests.Include(t => t.TestComments)
                                         .Include(t => t.UserScores)
                                         .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task Create(Test test)
        {
            await appContext.Tests.AddAsync(test);            
            await appContext.SaveChangesAsync();
        }

        public async Task Update(Test newTest)
        {
            var oldTest = appContext.Tests.Include(t => t.TestQuestions)                                           
                                          .ThenInclude(tq => tq.TestAnswers)                                          
                                          .FirstOrDefault(test => test.Id == newTest.Id);

            appContext.Entry(oldTest).CurrentValues.SetValues(newTest);
            UpdateTestQuestions(oldTest, newTest);

            await appContext.SaveChangesAsync();
        }       

        public async Task Delete(Test test)
        {
            appContext.Tests.Remove(test);
            await appContext.SaveChangesAsync();
        }
        
        private void UpdateTestQuestions(Test oldTest, Test newTest)
        {
            foreach (var testQuestion in oldTest.TestQuestions.ToList())
            {
                var existingTestQuestion = newTest.TestQuestions.FirstOrDefault(tq => tq.Id == testQuestion.Id);

                if (existingTestQuestion != null)
                {                    
                    appContext.Entry(testQuestion).CurrentValues.SetValues(existingTestQuestion);
                    UpdateTestAnswers(testQuestion, existingTestQuestion);
                }
                else
                {                    
                    appContext.Remove(testQuestion);                    
                }
            }

            foreach (var testQuestion in newTest.TestQuestions)
            {
                if (oldTest.TestQuestions.All(tq => tq.Id != testQuestion.Id))
                {
                    appContext.Add(testQuestion);                     
                }
            }
        }

        private void UpdateTestAnswers(TestQuestion oldTestQuestion, TestQuestion newTestQuestion)
        {
            foreach (var testAnswer in oldTestQuestion.TestAnswers.ToList())
            {
                var existingTestAnswer = newTestQuestion.TestAnswers.FirstOrDefault(ta => ta.Id == testAnswer.Id);

                if (existingTestAnswer != null)
                {
                    appContext.Entry(testAnswer).CurrentValues.SetValues(existingTestAnswer);
                }
                else
                {
                    appContext.Remove(testAnswer);
                }
            }

            foreach (var testAnswer in newTestQuestion.TestAnswers)
            {
                if (oldTestQuestion.TestAnswers.All(ta => ta.Id != testAnswer.Id))
                {
                    appContext.Add(testAnswer);
                }
            }
        }

        public async Task SaveChanges()
        {
            await appContext.SaveChangesAsync();
        }
       
        public async Task<IEnumerable<UserScore>> GetAllUserScores(Guid id)
        {
            var userScores = await appContext.UserScores.Where(us => us.TestId == id).ToListAsync();
            await SetUserScoresAuthor(userScores);
            return userScores;
        }

        public async Task<SummarizedUserScore> GetAllSummarizedUserScoresForUser(Guid id, Guid userId)
        {
            var userScoresForUser = await appContext.UserScores.Where(us => us.TestId == id & us.UserId == userId)
                                                               .Take(5)
                                                               .Skip(0)
                                                               .ToListAsync();

            var userScoreForUserCount = await appContext.UserScores.Where(us => us.TestId == id & us.UserId == userId).CountAsync();

            return new SummarizedUserScore { Count = userScoreForUserCount, UserScores = userScoresForUser };
        }

        public async Task AddUserScore(UserScore userScore)
        {
            appContext.UserScores.Add(userScore);
            await appContext.SaveChangesAsync();
        }

        public async Task RemoveUserScore(UserScore userScore)
        {
            appContext.UserScores.Remove(userScore);
            await appContext.SaveChangesAsync();
        }

        public async Task AddLike(TestLike like)
        {
            appContext.TestLikes.Add(like);
            await appContext.SaveChangesAsync();
        }

        public async Task RemoveLike(TestLike like)
        {
            appContext.TestLikes.Remove(like);
            await appContext.SaveChangesAsync();
        }

        public async Task AddDislike(TestDislike dislike)
        {
            appContext.TestDislikes.Add(dislike);
            await appContext.SaveChangesAsync();
        }

        public async Task RemoveDislike(TestDislike dislike)
        {
            appContext.TestDislikes.Remove(dislike);
            await appContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TestComment>> GetAllComments(Guid id)
        {
            var testComments = await appContext.TestComments.Where(tc => tc.TestId == id).ToListAsync();
            await SetTestCommentsAuthor(testComments);
            return testComments;
        }

        public async Task AddComment(TestComment comment)
        {
            appContext.TestComments.Add(comment);
            await appContext.SaveChangesAsync();
        }

        public async Task RemoveComment(TestComment comment)
        {
            appContext.TestComments.Remove(comment);
            await appContext.SaveChangesAsync();
        }          
    }
}
