using Bibliomatic_MAUI_App.Helpers;

namespace Bibliomatic_MAUI_App.Services
{
    public interface IDataService<DataType> where DataType : class
    {
        string ControllerName { get; set; }       
        Task<List<DataType>> GetAllData(int pageNumber, int pageSize, string filters, string additionalQueryParameters = null);
        Task<DataType> GetData(Guid id, string additionalQueryParameters = null);
        Task<List<string>> GetFiles(Guid id);
        Task<DataType> CreateData(DataType data);          
        Task UpdateData(Guid id, DataType data);        
        Task DeleteData(Guid id);
        Task PatchData(Guid id, PatchAction patchAction, string propertyName, object value);
        Task PatchDataCollection(Guid id, PatchAction patchAction, string propertyName, Dictionary<string, object> values);
    }
}
