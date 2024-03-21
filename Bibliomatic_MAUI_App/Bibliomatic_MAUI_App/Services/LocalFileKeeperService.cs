using System.Text;

namespace Bibliomatic_MAUI_App.Services
{
    public class LocalFileKeeperService : ILocalFileKeeperService
    {                
        private readonly List<string> extensionFilters = new() { "*.jpg", "*.png", "*.pdf" };
        private string fullFilePath;

        public readonly string localAppDataPath = FileSystem.AppDataDirectory;
        public string LocalAppDataPath
        {
            get => localAppDataPath;
        }

        private string baseDirectory;
        public string BaseDirectory
        {
            get => baseDirectory;
            set
            {
                baseDirectory = value;
                fullFilePath = CreateBaseDirectory(baseDirectory);
            }
        }       

        private string CreateBaseDirectory(string basePath)
        {
            var baseDirectory = Directory.CreateDirectory(Path.Combine(localAppDataPath, basePath));
            return baseDirectory.FullName;
        }

        public List<string> GetDataFromStorage()
        {
            return extensionFilters.SelectMany(filter => Directory.GetFiles(baseDirectory, filter, SearchOption.AllDirectories)).ToList();
        }

        private async Task<byte[]> GetFileContent(FileResult file)
        {
            var stream = await file.OpenReadAsync();
            var bytes = new byte[stream.Length];
            await stream.ReadAsync(bytes, 0, bytes.Length);

            return bytes;
        }

        private byte[] GetFileContent(string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content);

            return bytes;
        }

        private async Task<byte[]> GetFileContent(Stream content)
        {
            var bytes = new byte[content.Length];
            await content.ReadAsync(bytes, 0, bytes.Length);

            return bytes;
        }

        public async Task<string> SaveFileToLocalStorage(string content, string fileName)
        {           
            var fileContent = GetFileContent(content);
            var newPath = Path.Combine(fullFilePath, fileName);

            return await LocalFileManipulatorService.CreateNewFile(newPath, fileContent);
        }

        public async Task<string> SaveFileToLocalStorage(FileResult file, string oldPath)
        {
            string newFile = $"{Guid.NewGuid()}_attachment{Path.GetExtension(file.FileName)}";
            var fileContent = await GetFileContent(file);            
            var newPath = Path.Combine(fullFilePath, newFile);

            return await LocalFileManipulatorService.CreateNewFile(oldPath, newPath, fileContent);
        }  
        
        public async Task<string> SaveFileToLocalStorage(FileResult file)
        {
            string newFile = $"{Guid.NewGuid()}_attachment{Path.GetExtension(file.FileName)}";
            var fileContent = await GetFileContent(file);
            var newPath = Path.Combine(fullFilePath, newFile);

            return await LocalFileManipulatorService.CreateNewFile(newPath, fileContent);
        }

        public async Task<string> SaveFileToLocalStorage(Stream fileStream)
        {
            string newFile = $"{Guid.NewGuid()}_attachment.jpg";          
            var imageContent = await GetFileContent(fileStream);
            var newPath = Path.Combine(fullFilePath, newFile);

            return await LocalFileManipulatorService.CreateNewFile(newPath, imageContent);
        }

        public void DeleteAllFilesInBaseDirectory()
        {
            try
            {
                foreach (var file in Directory.GetFiles(fullFilePath))
                {
                    File.Delete(file);
                }
            }
            catch(Exception)
            {

            }            
        }

        public void DeleteAllFilesInBaseDirectoryAndSubdirectories()
        {
            try
            {
                DirectoryInfo baseDirectory = new DirectoryInfo(fullFilePath);

                foreach (var fileInfo in baseDirectory.GetFiles()) fileInfo.Delete();
                foreach (var subDirectoryInfo in baseDirectory.GetDirectories()) subDirectoryInfo.Delete(true);
            }
            catch (Exception)
            {
                
            }
        }
    }
}
