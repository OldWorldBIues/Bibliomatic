using Bibliomatic_MAUI_App.Helpers;
using System.Text.Json;

namespace Bibliomatic_MAUI_App.Services
{
    public class DataBaseException : Exception
    {
        public DataBaseException() { }
        public DataBaseException(string message) : base(message) { }
    }

    public class DataRequestService<TDataType> : IDataService<TDataType> where TDataType : class
    {
        public string ControllerName { get; set; }         
      
        public async Task<List<TDataType>> GetAllData(int pageNumber, int pageSize, string dataFilters, string additionalQueryParameters = null)
        {
            var dbException = new DataBaseException("Failed to load all data from server");
            additionalQueryParameters ??= string.Empty;
            int skippedRecords = (pageNumber - 1) * pageSize;  
            
            var response = await RequestService.HttpClient.GetAsync($"{ControllerName}?$top={pageSize}&$skip={skippedRecords}{additionalQueryParameters}{dataFilters}");
            response.ValidateStatusCode(dbException);

            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TDataType>>(content, RequestService.JsonSerializerOptions);
        }

        public async Task<TDataType> GetData(Guid id, string additionalQueryParameters = null)
        {
            var dbException = new DataBaseException("Failed to load data from server");            
            additionalQueryParameters ??= string.Empty;

            var response = await RequestService.HttpClient.GetAsync($"{ControllerName}/{id}{additionalQueryParameters}");
            response.ValidateStatusCode(dbException);

            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TDataType>(content, RequestService.JsonSerializerOptions);
        }

        public async Task<List<string>> GetFiles(Guid id)
        {
            var dbException = new DataBaseException("Failed to load data from server");

            var response = await RequestService.HttpClient.GetAsync($"{ControllerName}/{id}/files");
            response.ValidateStatusCode(dbException);

            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<string>>(content, RequestService.JsonSerializerOptions);
        }

        public async Task<TDataType> CreateData(TDataType data)
        {
            var dbException = new DataBaseException("Failed to create data on server");
            string json = JsonSerializer.Serialize(data, RequestService.JsonSerializerOptions);
            StringContent jsonContent = new StringContent(json, RequestService.DefaultEncoding, RequestService.JsonMediaType);
            
            var response = await RequestService.HttpClient.PostAsync($"{ControllerName}", jsonContent);
            response.ValidateStatusCode(dbException);

            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TDataType>(content, RequestService.JsonSerializerOptions);
        }        

        public async Task UpdateData(Guid id, TDataType data)
        {
            var dbException = new DataBaseException("Failed to update data on server");
            string json = JsonSerializer.Serialize(data, RequestService.JsonSerializerOptions);
            StringContent jsonContent = new StringContent(json, RequestService.DefaultEncoding, RequestService.JsonMediaType);
            
            var response = await RequestService.HttpClient.PutAsync($"{ControllerName}/{id}", jsonContent);
            response.ValidateStatusCode(dbException);            
        }        

        public async Task DeleteData(Guid id)
        {
            var dbException = new DataBaseException("Failed to delete data on server");

            var response = await RequestService.HttpClient.DeleteAsync($"{ControllerName}/{id}");
            response.ValidateStatusCode(dbException);
        }        

        public async Task PatchData(Guid id, PatchAction patchAction, string propertyName, object value)
        {
            var dbException = new DataBaseException("Failed to patch data on server");
            string json = JsonPatchExtension.CreateJsonPatchDocument(patchAction, propertyName, value);
            StringContent jsonContent = new StringContent(json, RequestService.DefaultEncoding, RequestService.JsonPatchMediaType);
            
            var response = await RequestService.HttpClient.PatchAsync($"{ControllerName}/{id}", jsonContent);
            response.ValidateStatusCode(dbException);
        }       

        public async Task PatchDataCollection(Guid id, PatchAction patchAction, string propertyName, Dictionary<string, object> values)
        {
            var dbException = new DataBaseException("Failed to patch data collection on server");
            string json = JsonPatchExtension.CreateJsonPatchDocument(patchAction, propertyName, values);
            StringContent jsonContent = new StringContent(json, RequestService.DefaultEncoding, RequestService.JsonPatchMediaType);
            
            var response = await RequestService.HttpClient.PatchAsync($"{ControllerName}/{id}", jsonContent);
            response.ValidateStatusCode(dbException);
        }
    }
}
