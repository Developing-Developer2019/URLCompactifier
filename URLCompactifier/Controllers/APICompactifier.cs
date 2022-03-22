using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using URLCompactifier.Models;
using URLCompactifier.Models.SharedExtensions;

namespace URLCompactifier.Controllers
{
    [ApiController]
    public class APICompactifier : ControllerBase
    {
        [Route("/r/{token}")]
        [HttpGet]
        public IActionResult Get(string token)
        {
            if (token == null)
            {
                return BadRequest();
            }

            // Check if token exists
            if (!SqlLiteDataAccess.DoesTokenExist(token))
            {
                return RedirectToAction("Error", "Home", 404);
            }

            // Initialise new link model
            var link = new LinkBO();
            
            // Get link details using token
            link = SqlLiteDataAccess.GetLinkDetailsWithTokenOrName(token, true);

            // Add Http to string
            link.Link_Name = SharedExtensions.HttpInput(link.Link_Name);

            return new RedirectResult(link.Link_Name);
        }
    }
}
