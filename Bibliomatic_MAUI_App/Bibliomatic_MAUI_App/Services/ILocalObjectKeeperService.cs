namespace Bibliomatic_MAUI_App.Services
{
    public interface ILocalObjectKeeperService<DataType> where DataType : class
    {
        string LocalAppDataPath { get; }
        string BaseDirectory { get; set; }
        Task<string> SaveFileToLocalStorage(DataType element);
        List<DataType> GetDataFromStorage();
    }
}
