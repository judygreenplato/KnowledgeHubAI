
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    namespace KnowledgeHub.API.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetProfile()
        {
            return Ok(new
            {
                Message = "You are authenticated"
            });
        }
    }
