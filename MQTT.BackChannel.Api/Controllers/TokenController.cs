using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MQTT.BackChannel.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetTokenAsync()
        {
            return Ok("Get token endpoint called");
        }
    }
}
