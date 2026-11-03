using Microsoft.AspNetCore.Mvc;

namespace EcobinSellerService.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : Controller
    {
        [HttpGet("random")]
        public IActionResult Index()
        {
            return Ok(new
            {
                message = "Chal sala Ecobin workking"
            });
        }
    }
}
