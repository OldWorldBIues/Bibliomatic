namespace Bibliomatic_MAUI_App.Services
{
    public interface ILocalFileKeeper<DataType> where DataType : class
    {        
        string CreateBaseDirectory(DataType element);
        Task<byte[]> GetFileContent(FileResult file);        
        Task<string> SaveFileToLocalStorage(DataType element, FileResult file);
        Task CreateNewFile(string oldFilePath, string newFilePath, byte[] content);
    }
}
