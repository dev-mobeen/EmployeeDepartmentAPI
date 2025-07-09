using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDepartmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        // ✅ Public (no token needed)
        [HttpGet("public")]
        public IActionResult Public()
        {
            return Ok("This is a public endpoint — no token needed.");
        }

        // 🔐 Protected (requires JWT)
        [Authorize]
        [HttpGet("secure")]
        public IActionResult Secure()
        {
            var username = User.Identity?.Name;
            return Ok($"Hello {username}, you accessed a protected endpoint!");
        }
    }
}
