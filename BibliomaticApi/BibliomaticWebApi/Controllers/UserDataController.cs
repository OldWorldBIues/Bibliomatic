using BibliomaticData.Models;
using BibliomaticData.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace BibliomaticWebApi.Controllers
{
    [ApiController]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
    [Route("api/userData")]
    public class UserDataController : Controller
    {       
        private readonly IUserDataRepository userDataRepository;

        public UserDataController(IUserDataRepository userDataRepository)
        {
            this.userDataRepository = userDataRepository;            
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetUserData(Guid id)
        {
            var result = await userDataRepository.GetUserDataById(id);

            if (result == null)
                return NotFound();
            
            return Ok(result);
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<ActionResult> UpdateUserData(Guid id, Author author)
        {
            if(author.Id != id)
                return BadRequest();

            var result = await userDataRepository.UpdateUserData(author);

            if (result == null)
                return NotFound();

            return Ok(result);
        }        
    }
}
