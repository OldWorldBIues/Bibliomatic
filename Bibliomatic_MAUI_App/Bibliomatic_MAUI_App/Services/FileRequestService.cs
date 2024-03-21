using Bibliomatic_MAUI_App.Models.AttachmentInfo;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


namespace Bibliomatic_MAUI_App.Services
{
    public class FileStorageException : Exception
    {
        public FileStorageException() { }
        public FileStorageException(string message) : base(message) { }
    }

    public class FileRequestService : IFileService
    {
        private string ControllerName { get; } = "files";

        private readonly string singleFileParamName = "file";
        private readonly string singleFileRoute = "single";

        private readonly string multipleFilesParamName = "files";
        private readonly string multipleFilesRoute = "multiple"; 

        private string GetRequestUrl(string baseUrl, IEnumerable<string> files, string queryName)
        {
            var requestUrlBuilder = new StringBuilder(baseUrl);
            
            requestUrlBuilder.Append($"?{queryName}=");
            requestUrlBuilder.AppendJoin($"&{queryName}=", files.ToArray());   
            
            return requestUrlBuilder.ToString();
        }

        public async Task<string> GetFileContent(string url)
        {
            return await RequestService.DefaultHttpClient.GetStringAsync(url);
        }

        public async Task<List<AttachmentResponse>> GetFilesLinks(IEnumerable<string> files)
        {
            var fileStorageException = new FileStorageException("Failed to load files from server");
            var baseUri = new Uri($"{RequestService.HttpClient.BaseAddress}/{ControllerName}");
            var requestUrl = GetRequestUrl(baseUri.AbsoluteUri, files, multipleFilesParamName);
            
            var response = await RequestService.HttpClient.GetAsync(requestUrl);
            response.ValidateStatusCode(fileStorageException);

            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<AttachmentResponse>>(content, RequestService.JsonSerializerOptions);
        }

        public async Task<AttachmentDTOResponse> UploadFile(string file, string basePath)
        {
            var fileStorageException = new FileStorageException("Failed to upload file to server");
            var form = new MultipartFormDataContent();
            using var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(file));

            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(RequestService.FilesContentType);
            form.Add(fileContent, singleFileParamName, Path.GetRelativePath(basePath, file));
            
            var response = await RequestService.HttpClient.PostAsync($"{ControllerName}/{singleFileRoute}", form);
            response.ValidateStatusCode(fileStorageException);

            string content = await response.Content.ReadAsStringAsync();
            var attachment = JsonSerializer.Deserialize<AttachmentDTOResponse>(content, RequestService.JsonSerializerOptions);

            if(response.IsSuccessStatusCode & attachment.Error)
            {
                throw fileStorageException;
            }

            return attachment;
        }  
        
        public async Task<List<AttachmentDTOResponse>> UploadFiles(IEnumerable<string> files, string basePath)
        {
            var fileStorageException = new FileStorageException("Failed to upload files to server");
            var form = new MultipartFormDataContent();

            foreach (var file in files)
            {
                var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(file));
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(RequestService.FilesContentType);
                form.Add(fileContent, multipleFilesParamName, Path.GetRelativePath(basePath, file));
            }
            
            var response = await RequestService.HttpClient.PostAsync($"{ControllerName}/{multipleFilesRoute}", form);
            response.ValidateStatusCode(fileStorageException);            

            string content = await response.Content.ReadAsStringAsync();
            var attachments = JsonSerializer.Deserialize<List<AttachmentDTOResponse>>(content, RequestService.JsonSerializerOptions);

            if (response.IsSuccessStatusCode & attachments.Any(a => a.Error))
            {
                var loadedFilesList = attachments.Where(a => !a.Error).ToList();
                var failedFilesList = attachments.Where(a => a.Error).Select(a => a.Attachment.Name).ToList();

                fileStorageException.Data.Add("LoadedFilesList", loadedFilesList);
                fileStorageException.Data.Add("FailedFilesList", failedFilesList);

                throw fileStorageException;
            }

            return attachments;
        }

        public async Task<List<AttachmentDTOResponse>> UploadFiles(string basePath, params string[] files) => await UploadFiles(files.ToList(), basePath);

        public async Task DeleteFile(string file)
        {
            var fileStorageException = new FileStorageException("Failed to upload file on server");
            
            var response = await RequestService.HttpClient.DeleteAsync($"{ControllerName}/{singleFileRoute}/{file}");
            response.ValidateStatusCode(fileStorageException);
        }

        public async Task DeleteFiles(IEnumerable<string> files)
        {
            var fileStorageException = new FileStorageException("Failed to delete files on server");

            var baseUri = new Uri($"{RequestService.HttpClient.BaseAddress}{ControllerName}/{multipleFilesRoute}");
            var request = new HttpRequestMessage(HttpMethod.Delete, baseUri);

            string json = JsonSerializer.Serialize(files, RequestService.JsonSerializerOptions);
            request.Content = new StringContent(json, RequestService.DefaultEncoding, RequestService.JsonMediaType);
           
            var response = await RequestService.HttpClient.SendAsync(request);
            response.ValidateStatusCode(fileStorageException);
        }

        public async Task DeleteFiles(params string[] files) => await DeleteFiles(files.ToList());
    }
}
