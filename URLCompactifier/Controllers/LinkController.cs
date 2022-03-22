using Microsoft.AspNetCore.Mvc;
using URLCompactifier.Models;

namespace URLCompactifier.Controllers
{
    public class LinkController : Controller
    {

        public IActionResult Index(LinkDTO? linkDTO)
        {
            if (linkDTO == null)
            {
                linkDTO = new LinkDTO();
            }

            return View(linkDTO);
        }

        /// <summary>
        /// Input URL from view
        /// </summary>
        /// <param name="urlFullLink"></param>
        /// <returns></returns>
        public IActionResult InputURL(string urlFullLink)
        {
            // If null or empty, return to index
            if (string.IsNullOrEmpty(urlFullLink))
            {
                return RedirectToAction("Index");
            }

            // Initiate variables
            var linkArray = urlFullLink.Split(new[] { '/' }, 2);
            var linkDTO = new LinkDTO();

            // Populate DTO values
            linkDTO.PrimaryLink.PrimaryLink_Name = linkArray[0];
            linkDTO.SecondaryLink.SecondaryLink_Name = linkArray[1];
            linkDTO.SecondaryLink.SecondaryLink_Token = GenerateToken();
            linkDTO.CompactifyLink = linkDTO.SecondaryLink.SecondaryLink_Token;

            SqlLiteDataAccess.UploadLinks(linkDTO.PrimaryLink, linkDTO.SecondaryLink);

            // Return view with populated data
            return RedirectToAction("Index", linkDTO);
        }

        /// <summary>
        /// Method to generate a token
        /// </summary>
        /// <returns>Randomised token between 8-10</returns>
        public string GenerateToken()
        {
            string urlsafe = string.Empty;
            Enumerable.Range(48, 75)
              .Where(i => i < 58 || i > 64 && i < 91 || i > 96)
              .OrderBy(o => new Random().Next())
              .ToList()
              .ForEach(i => urlsafe += Convert.ToChar(i)); // Store each char into urlsafe
            var tokenValue = urlsafe.Substring(new Random().Next(0, urlsafe.Length), new Random().Next(8, 15));

            // Check if token exists, if true, run again
            if (SqlLiteDataAccess.DoesTokenExist(tokenValue))
            {
                GenerateToken();
            }

            return tokenValue;
        }
    }
}
