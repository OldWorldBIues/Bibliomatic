namespace BibliomaticData.Repository
{
    public interface IRepository<TEntity, TDto> where TEntity : class where TDto : class
    {
        Task<IEnumerable<TDto>> GetAll();     
        Task<IEnumerable<string>> GetAllFiles(Guid id);
        Task<TEntity> GetById(Guid id);
        Task<TEntity> GetSummarizedById(Guid id);
        Task<TEntity> GetTrackedById(Guid id);       
        Task Create(TEntity entity);        
        Task Update(TEntity entity);       
        Task Delete(TEntity entity);       
        Task SaveChanges();
    }
}
