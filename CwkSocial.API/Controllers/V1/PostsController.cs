using Microsoft.AspNetCore.Mvc;

namespace CwkSocial.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok();
        }
    }
}