using BibliomaticData.Models;
using BibliomaticData.Models.DTOs;
using Microsoft.AspNetCore.Http;

namespace BibliomaticData.Repository
{
    public interface IFileRepository
    {
        Task<List<Blob>> FilesList();
        Task<List<Blob>> FilesList(IEnumerable<string> files);
        Task<BlobDTO> UploadFile(IFormFile blob);
        Task<List<BlobDTO>> UploadFiles(IEnumerable<IFormFile> files);
        Task<Blob?> DownloadFile(string blobFilename);
        Task<Blob?> DownloadFiles(IEnumerable<string> blobFilenames);
        Task<BlobDTO> DeleteFile(string blobFilename);
        Task<List<BlobDTO>> DeleteFiles(IEnumerable<string> blobFilenames);
    }
}
