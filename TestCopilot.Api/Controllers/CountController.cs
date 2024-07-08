using Microsoft.AspNetCore.Mvc;

namespace TestCopilot.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountController : ControllerBase
    {

        [HttpGet("Next/{number}")]
        public int Next(int number)
        {
            return number + 1;
        }
    }
}
