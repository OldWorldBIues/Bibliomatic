using Bibliomatic_MAUI_App.Helpers;
using System.Text.Json;

namespace Bibliomatic_MAUI_App.Services
{
    public class RelatedDataRequestService<TMainType, TRelatedType> : IRelatedDataService<TMainType, TRelatedType> where TRelatedType : class where TMainType : class
    {
        public string ControllerName { get; set; }
        public string RelatedRouteName { get; set; }
        
        public async Task<List<TRelatedType>> GetAllRelatedData(Guid id, int pageNumber, int pageSize, string dataFilters)
        {
            var dbException = new DataBaseException("Failed to load related data from server");
            int skippedRecords = (pageNumber - 1) * pageSize;

            var response = await RequestService.HttpClient.GetAsync($"{ControllerName}/{id}/{RelatedRouteName}?$top={pageSize}&$skip={skippedRecords}{dataFilters}");
            response.ValidateStatusCode(dbException);

            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TRelatedType>>(content, RequestService.JsonSerializerOptions);
        }

        public async Task<List<TRelatedType>> GetAllRelatedDataById(Guid id, string collectionName, int skippedRecords, int pageSize, string dataFilters)
        {
            var dbException = new DataBaseException("Failed to load related data from server");            
            
            var response = await RequestService.HttpClient.GetAsync($"{ControllerName}/{id}/{collectionName}?$top={pageSize}&$skip={skippedRecords}{dataFilters}");
            response.ValidateStatusCode(dbException);

            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TRelatedType>>(content, RequestService.JsonSerializerOptions);
        }
        
        public async Task<List<TRelatedType>> GetAllRelatedDataById(Guid id, Guid relatedId, string collectionName, int skippedRecords, int pageSize, string dataFilters)
        {
            var dbException = new DataBaseException("Failed to load related data from server");           
            
            var response = await RequestService.HttpClient.GetAsync($"{ControllerName}/{id}/{RelatedRouteName}/{relatedId}/{collectionName}?$top={pageSize}&$skip={skippedRecords}{dataFilters}");
            response.ValidateStatusCode(dbException);

            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TRelatedType>>(content, RequestService.JsonSerializerOptions);
        }

        public async Task<TRelatedType> CreateRelatedData(Guid id, TRelatedType data)
        {
            var dbException = new DataBaseException("Failed to create related data on server");
            string json = JsonSerializer.Serialize(data, RequestService.JsonSerializerOptions);
            StringContent jsonContent = new StringContent(json, RequestService.DefaultEncoding, RequestService.JsonMediaType);
            
            var response = await RequestService.HttpClient.PostAsync($"{ControllerName}/{id}/{RelatedRouteName}", jsonContent);
            response.ValidateStatusCode(dbException);

            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TRelatedType>(content, RequestService.JsonSerializerOptions);
        }
        public async Task<TRelatedType> CreateRelatedData(Guid id, TRelatedType data, string collectionName)
        {
            var dbException = new DataBaseException("Failed to create related data on server");
            string json = JsonSerializer.Serialize(data, RequestService.JsonSerializerOptions);
            StringContent jsonContent = new StringContent(json, RequestService.DefaultEncoding, RequestService.JsonMediaType);

            var response = await RequestService.HttpClient.PostAsync($"{ControllerName}/{id}/{collectionName}", jsonContent);
            response.ValidateStatusCode(dbException);

            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TRelatedType>(content, RequestService.JsonSerializerOptions);
        }

        public async Task<TRelatedType> CreateRelatedData(Guid id, Guid relatedId, TRelatedType data, string collectionName)
        {
            var dbException = new DataBaseException("Failed to create related data on server");
            string json = JsonSerializer.Serialize(data, RequestService.JsonSerializerOptions);
            StringContent jsonContent = new StringContent(json, RequestService.DefaultEncoding, RequestService.JsonMediaType);

            var response = await RequestService.HttpClient.PostAsync($"{ControllerName}/{id}/{RelatedRouteName}/{relatedId}/{collectionName}", jsonContent);
            response.ValidateStatusCode(dbException);

            string content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TRelatedType>(content, RequestService.JsonSerializerOptions);
        }

        public async Task UpdateRelatedData(Guid id, Guid relatedId, TRelatedType data)
        {
            var dbException = new DataBaseException("Failed to update related data on server");
            string json = JsonSerializer.Serialize(data, RequestService.JsonSerializerOptions);
            StringContent jsonContent = new StringContent(json, RequestService.DefaultEncoding, RequestService.JsonMediaType);
            
            var response = await RequestService.HttpClient.PutAsync($"{ControllerName}/{id}/{RelatedRouteName}/{relatedId}", jsonContent);
            response.ValidateStatusCode(dbException);
        }

        public async Task UpdateRelatedData(Guid id, Guid relatedId, TRelatedType data, string collectionName)
        {
            var dbException = new DataBaseException("Failed to update related data on server");
            string json = JsonSerializer.Serialize(data, RequestService.JsonSerializerOptions);
            StringContent jsonContent = new StringContent(json, RequestService.DefaultEncoding, RequestService.JsonMediaType);
            
            var response = await RequestService.HttpClient.PutAsync($"{ControllerName}/{id}/{collectionName}/{relatedId}", jsonContent);
            response.ValidateStatusCode(dbException);
        }

        public async Task UpdateRelatedData(Guid id, Guid relatedId, Guid childId, TRelatedType data, string collectionName)
        {
            var dbException = new DataBaseException("Failed to update related data on server");
            string json = JsonSerializer.Serialize(data, RequestService.JsonSerializerOptions);
            StringContent jsonContent = new StringContent(json, RequestService.DefaultEncoding, RequestService.JsonMediaType);
            
            var response = await RequestService.HttpClient.PutAsync($"{ControllerName}/{id}/{RelatedRouteName}/{relatedId}/{collectionName}/{childId}", jsonContent);
            response.ValidateStatusCode(dbException);
        }

        public async Task DeleteRelatedData(Guid id, Guid relatedId)
        {
            var dbException = new DataBaseException("Failed to delete related data on server");
            
            var response = await RequestService.HttpClient.DeleteAsync($"{ControllerName}/{id}/{RelatedRouteName}/{relatedId}");
            response.ValidateStatusCode(dbException);
        }

        public async Task DeleteRelatedData(Guid id, Guid relatedId, string collectionName)
        {
            var dbException = new DataBaseException("Failed to delete related data on server");
            
            var response = await RequestService.HttpClient.DeleteAsync($"{ControllerName}/{id}/{collectionName}/{relatedId}");
            response.ValidateStatusCode(dbException);
        }

        public async Task DeleteRelatedData(Guid id, Guid relatedId, Guid childId, string collectionName)
        {
            var dbException = new DataBaseException("Failed to delete related data on server");
           
            var response = await RequestService.HttpClient.DeleteAsync($"{ControllerName}/{id}/{RelatedRouteName}/{relatedId}/{collectionName}/{childId}");
            response.ValidateStatusCode(dbException);
        }

        public async Task PatchRelatedData(Guid id, Guid relatedId, PatchAction patchAction, string propertyName, object value)
        {
            var dbException = new DataBaseException("Failed to patch related data on server");
            string json = JsonPatchExtension.CreateJsonPatchDocument(patchAction, propertyName, value);
            StringContent jsonContent = new StringContent(json, RequestService.DefaultEncoding, RequestService.JsonPatchMediaType);

            var response = await RequestService.HttpClient.PatchAsync($"{ControllerName}/{id}/{RelatedRouteName}/{relatedId}", jsonContent);
            response.ValidateStatusCode(dbException);
        }

        public async Task PatchRelatedDataCollection(Guid id, Guid relatedId, PatchAction patchAction, string propertyName, Dictionary<string, object> values)
        {
            var dbException = new DataBaseException("Failed to patch related data on server");
            string json = JsonPatchExtension.CreateJsonPatchDocument(patchAction, propertyName, values);
            StringContent jsonContent = new StringContent(json, RequestService.DefaultEncoding, RequestService.JsonPatchMediaType);

            var response = await RequestService.HttpClient.PatchAsync($"{ControllerName}/{id}/{RelatedRouteName}/{relatedId}", jsonContent);
            response.ValidateStatusCode(dbException);
        }
    }
}
