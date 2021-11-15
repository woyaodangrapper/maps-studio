using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
namespace Optical_Server.Controllers
{
   
    [Produces("application/json")]
    [ApiController]
    [Route("sys/user")]
    public class User : Controller
    {
        // GET api/auth/login
        [HttpGet, Route("login")]
        public IActionResult Login(string name)
        {
            return new ContentResult() { Content = JwtManager.GenerateToken(name) };
        }
    }
}
