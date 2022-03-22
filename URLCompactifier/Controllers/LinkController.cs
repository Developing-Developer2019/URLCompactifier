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
            linkDTO.CompactifyLink = RandomiseString(urlFullLink);
            linkDTO.PrimaryLink.PrimaryLink_Name = linkArray[0]; 
            linkDTO.SecondaryLink.SecondaryLink_Name = linkArray[1];

            // Return view with populated data
            return RedirectToAction("Index", linkDTO);
        }

        public string RandomiseString(string urlFullLink)
        {
            int range = 10;

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, range)
                                                    .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }
    }
}
