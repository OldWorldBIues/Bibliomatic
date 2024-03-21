namespace Bibliomatic_MAUI_App.Services
{
    public static class LocalFileManipulatorService
    {
        public static async Task<string> CreateNewFile(string oldFilePath, string newFilePath, byte[] content)
        {
            DeleteFileIfExist(oldFilePath);
            return await CreateFile(newFilePath, content);
        }

        public static async Task<string> CreateNewFile(string newFilePath, byte[] content)
        {
            return await CreateFile(newFilePath, content);            
        }

        private static async Task<string> CreateFile(string path, byte[] content)
        {
            await File.WriteAllBytesAsync(path, content);
            return path;
        }

        private static void DeleteFileIfExist(string path)
        {
            if (string.IsNullOrEmpty(path))
                return;

            File.Delete(path);
        }

        public static string GetLocalFileContent(string path)
        {
            if(string.IsNullOrEmpty(path)) 
                return null;

            return File.ReadAllText(path);
        }
    }
}
