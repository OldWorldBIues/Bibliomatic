using Bibliomatic_MAUI_App.Services.AzureB2CAuthService;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bibliomatic_MAUI_App.Services
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() { }        
        public UnauthorizedException(string message) : base(message) { }        
    }

    public class DataNotFoundException : Exception
    {
        public DataNotFoundException() { }
        public DataNotFoundException(string message) : base(message) { }
    }

    public static class ResponseMessageValidator
    {
        public static void ValidateStatusCode(this HttpResponseMessage response, Exception exception)
        {
            var statusCode = response.StatusCode;

            if(!response.IsSuccessStatusCode)
            {
                switch (statusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new UnauthorizedException("Your authorization has expired. To perform operations with data you should re-authorize");

                    case HttpStatusCode.NotFound:
                        throw new DataNotFoundException("Requested data was not found. Data is likely to have been deleted or moved");

                    default:
                        throw exception;
                }
            }
        }
    }

    public class RequestService
    {       
        static RequestService()
        {
            DefaultHttpClient = new HttpClient();

            HttpClient = new HttpClient()
            {
                BaseAddress = new Uri($"{Constants.BaseApiRoute}/"),
            };            

            JsonSerializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                ReferenceHandler = ReferenceHandler.Preserve,
            };
        }
        
        public static HttpClient DefaultHttpClient { get; private set;}
        public static HttpClient HttpClient { get; private set; }
        public static JsonSerializerOptions JsonSerializerOptions { get; private set; }        

        public static readonly string FilesContentType = "multipart/form-data";
        public static readonly string JsonMediaType = "application/json";
        public static readonly string JsonPatchMediaType = "application/json-patch+json";
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;   
    }
}
