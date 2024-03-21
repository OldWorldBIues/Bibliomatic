namespace Bibliomatic_MAUI_App.Services
{
    public interface ILocalFileKeeperService
    {
        string LocalAppDataPath { get; }
        string BaseDirectory { get; set; }
        Task<string> SaveFileToLocalStorage(FileResult file, string oldPath);
        Task<string> SaveFileToLocalStorage(FileResult file);
        Task<string> SaveFileToLocalStorage(Stream fileStream);
        Task<string> SaveFileToLocalStorage(string content, string extension);
        List<string> GetDataFromStorage();
        void DeleteAllFilesInBaseDirectory();
        void DeleteAllFilesInBaseDirectoryAndSubdirectories();
    }
}
