using Bibliomatic_MAUI_App.Helpers;

namespace Bibliomatic_MAUI_App.Services
{    
    public interface IRelatedDataService<MainType, RelatedType> where RelatedType : class where MainType : class
    {
        string ControllerName { get; set; }
        string RelatedRouteName { get; set; }
        Task<List<RelatedType>> GetAllRelatedData(Guid id, int pageNumber, int pageSize, string dataFilters);
        Task<List<RelatedType>> GetAllRelatedDataById(Guid id, string collectionName, int skippedRecords, int pageSize, string dataFilters);        
        Task<List<RelatedType>> GetAllRelatedDataById(Guid id, Guid relatedId, string collectionName, int skippedRecords, int pageSize, string dataFilters);
        Task<RelatedType> CreateRelatedData(Guid id, RelatedType data);        
        Task<RelatedType> CreateRelatedData(Guid id, RelatedType data, string collectionName);
        Task<RelatedType> CreateRelatedData(Guid id, Guid relatedId, RelatedType data, string collectionName);
        Task UpdateRelatedData(Guid id, Guid relatedId, RelatedType data);
        Task UpdateRelatedData(Guid id, Guid relatedId, RelatedType data, string collectionName);
        Task UpdateRelatedData(Guid id, Guid relatedId, Guid childId, RelatedType data, string collectionName);
        Task DeleteRelatedData(Guid id, Guid relatedId);
        Task DeleteRelatedData(Guid id, Guid relatedId, string collectionName);
        Task DeleteRelatedData(Guid id, Guid relatedId, Guid childId, string collectionName);
        Task PatchRelatedData(Guid id, Guid relatedId, PatchAction patchAction, string propertyName, object value);
        Task PatchRelatedDataCollection(Guid id, Guid relatedId, PatchAction patchAction, string propertyName, Dictionary<string, object> values);
    }
}
