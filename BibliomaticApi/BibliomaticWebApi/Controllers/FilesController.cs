using BibliomaticData.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace BibliomaticWebApi.Controllers
{
    [ApiController]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
    [Route("api/files")]
    public class FilesController : Controller
    {
        private readonly IFileRepository fileRepository;

        public FilesController(IFileRepository fileRepository)
        {
            this.fileRepository = fileRepository;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult> GetFilesList()
        {
            var result = await fileRepository.FilesList();
            return Ok(result);
        }

        [HttpGet]        
        public async Task<ActionResult> GetFilesList([FromBody] IEnumerable<string> files)
        {
            var result = await fileRepository.FilesList(files);
            return Ok(result);
        }

        [HttpGet]
        [Route("single/{filename}")]
        public async Task<ActionResult> DownloadFile(string file)
        {            
            var result = await fileRepository.DownloadFile(file);
            return File(result.Content, result.ContentType, result.Name);
        }

        [HttpGet]
        [Route("multiple")]
        public async Task<ActionResult> DownloadFiles([FromBody] IEnumerable<string> files)
        {
            var result = await fileRepository.DownloadFiles(files);
            return File(result.Content, result.ContentType, result.Name);
        }
            
        [HttpPost]        
        [Route("single")]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            var result = await fileRepository.UploadFile(file);
            return Ok(result);            
        }

        [HttpPost]
        [Route("multiple")]
        public async Task<ActionResult> UploadFiles(IEnumerable<IFormFile> files)
        {
            var result = await fileRepository.UploadFiles(files);
            return Ok(result);
        }

        [HttpDelete]
        [Route("single/{filename}")]
        public async Task<ActionResult> DeleteFile(string file)
        {
            var result = await fileRepository.DeleteFile(file);
            return Ok(result);
        }

        [HttpDelete]
        [Route("multiple")]
        public async Task<ActionResult> DeleteFiles([FromBody]IEnumerable<string> files)
        {
            var result = await fileRepository.DeleteFiles(files);
            return Ok(result);
        }
    }
}
