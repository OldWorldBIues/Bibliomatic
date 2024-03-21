using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bibliomatic_MAUI_App.Services
{
    public class LocalObjectKeeperService<TDataType> : ILocalObjectKeeperService<TDataType> where TDataType : class
    {       
        private JsonSerializerOptions serializerOptions;
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

        public LocalObjectKeeperService()
        {
            serializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                ReferenceHandler = ReferenceHandler.Preserve,
            };
        }

        private string CreateBaseDirectory(string basePath)
        {
            var baseDirectory = Directory.CreateDirectory(Path.Combine(localAppDataPath, basePath));
            return baseDirectory.FullName;
        }

        public List<TDataType> GetDataFromStorage()
        {
            var files = new List<TDataType>();
            var dataFiles = Directory.GetFiles(localAppDataPath, "*.json", SearchOption.AllDirectories);

            foreach (var file in dataFiles)
            {
                var content = File.ReadAllText(file);
                var data = DeserializeData(content);
                files.Add(data);
            }

            return files;
        }

        private byte[] GetFileContent(string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content);

            return bytes;
        }

        private string SerializeData(TDataType data)
        {
            return JsonSerializer.Serialize(data, serializerOptions);
        }

        private TDataType DeserializeData(string data)
        {
            return JsonSerializer.Deserialize<TDataType>(data, serializerOptions);
        }

        public async Task<string> SaveFileToLocalStorage(TDataType data)
        {            
            string jsonData = SerializeData(data);
            var content = GetFileContent(jsonData);

            string filename = $"{BaseDirectory}.json";
            var pathToObject = Path.Combine(fullFilePath, filename);

            await LocalFileManipulatorService.CreateNewFile(pathToObject, pathToObject, content);            

            return pathToObject;
        }
    }
}
