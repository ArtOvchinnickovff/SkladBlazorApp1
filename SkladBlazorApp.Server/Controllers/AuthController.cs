using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkladBlazorApp.Shared.ModelsDTO;

namespace SkladBlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] CreateUsersDto dto)
        {
            
        }
    }
}
