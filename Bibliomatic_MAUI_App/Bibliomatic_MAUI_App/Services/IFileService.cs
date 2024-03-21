using Bibliomatic_MAUI_App.Models.AttachmentInfo;

namespace Bibliomatic_MAUI_App.Services
{
    public interface IFileService
    {
        Task<string> GetFileContent(string url);
        Task<List<AttachmentResponse>> GetFilesLinks(IEnumerable<string> files);
        Task<AttachmentDTOResponse> UploadFile(string file, string basePath);
        Task<List<AttachmentDTOResponse>> UploadFiles(IEnumerable<string> files, string basePath);        
        Task<List<AttachmentDTOResponse>> UploadFiles(string basePath, params string[] files);
        Task DeleteFile(string file);
        Task DeleteFiles(IEnumerable<string> files);
        Task DeleteFiles(params string[] files);
    }
}
