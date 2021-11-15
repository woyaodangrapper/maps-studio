using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Analytics.UserId;

namespace Optical_Server.Controllers.WebsiteBusiness
{
    [Authorize]
    [ApiController]
    [Route("api/File")]
    public class FileController : Controller
    {
        
    }
}
