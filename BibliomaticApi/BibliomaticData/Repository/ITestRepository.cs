using BibliomaticData.Models;
using BibliomaticData.Models.DTOs;

namespace BibliomaticData.Repository
{
    public interface ITestRepository : IRepository<Test, TestDTO>
    {
        Task<IEnumerable<TestDTO>> GetAllByTestResult(Guid userId, bool passed);
        Task<Test> GetSummarizedById(Guid id, Guid userId);
    }
}
