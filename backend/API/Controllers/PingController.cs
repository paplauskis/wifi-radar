using Microsoft.AspNetCore.Mvc;

//backend+frontend testui
namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new { status = "backend-ok", time = DateTime.Now });
    }
}
