using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using URLCompactifier.Models;

namespace URLCompactifier.Controllers
{
    [ApiController]
    public class APICompactifier : ControllerBase
    {
        [Route("api/{token}")]
        [HttpGet]
        public IActionResult Get(string token)
        {
            LinkDTO linkDTO = new LinkDTO();
            linkDTO.SecondaryLink = SqlLiteDataAccess.GetSecondaryLink(token);

            return new NotFoundResult();

            //return new RedirectResult("https://www.google.com");
        }
    }
}
